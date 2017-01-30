﻿using HeroesStatTracker.Data.Databases;
using HeroesStatTracker.Data.Models.Replays;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace HeroesStatTracker.Data.Queries.Replays
{
    public class HotsPlayer : NonContextQueriesBase<ReplayAllHotsPlayer>, IRawDataQueries<ReplayAllHotsPlayer>
    {
        public List<ReplayAllHotsPlayer> ReadAllRecords()
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.ToList();
            }
        }

        public List<ReplayAllHotsPlayer> ReadLastRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.OrderByDescending(x => x.PlayerId).Take(amount).ToList();
            }
        }

        public List<ReplayAllHotsPlayer> ReadRecordsCustomTop(int amount, string columnName, string orderBy)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                return new List<ReplayAllHotsPlayer>();

            if (amount == 0)
                amount = 1;

            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM ReplayAllHotsPlayers ORDER BY {columnName} {orderBy} LIMIT {amount}").ToList();
            }
        }

        public List<ReplayAllHotsPlayer> ReadRecordsWhere(string columnName, string operand, string input)
        {
            if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                return new List<ReplayAllHotsPlayer>();

            if (LikeOperatorInputCheck(operand, input))
                input = $"%{input}%";
            else if (input == null)
                input = string.Empty;

            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM ReplayAllHotsPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
            }
        }

        public List<ReplayAllHotsPlayer> ReadTopRecords(int amount)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.OrderByDescending(x => x.PlayerId).Take(amount).ToList();
            }
        }

        public ReplayAllHotsPlayer ReadRecordFromPlayerId(long playerId)
        {
            using (var db = new ReplaysContext())
            {
                return db.ReplayAllHotsPlayers.Where(x => x.PlayerId == playerId).FirstOrDefault();
            }
        }

        public long ReadPlayerIdFromBattleTagName(string battleTagName, int battleNetRegionId)
        {
            using (var db = new ReplaysContext())
            {
                // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
                var result = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == battleTagName && x.BattleNetRegionId == battleNetRegionId);
                if (result != null)
                    return result.PlayerId;
                else
                    return 0;
            }
        }

        internal override long CreateRecord(ReplaysContext db, ReplayAllHotsPlayer model)
        {
            db.ReplayAllHotsPlayers.Add(model);
            db.SaveChanges();

            return model.PlayerId;
        }

        internal override long UpdateRecord(ReplaysContext db, ReplayAllHotsPlayer model)
        {
            ReplayAllHotsPlayer record;

            if (model.BattleNetId > 0)
            {
                record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleNetId == model.BattleNetId &&
                                                                      x.BattleNetRegionId == model.BattleNetRegionId &&
                                                                      x.BattleNetSubId == model.BattleNetSubId);
            }
            else // if its an observer with no battlenetid
            {
                if (!string.IsNullOrEmpty(model.BattleNetTId))
                    record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleNetTId == model.BattleNetTId);
                else
                    record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == model.BattleTagName && x.BattleNetRegionId == model.BattleNetRegionId);
            }

            if (record != null)
            {
                // existing observer record, update the info
                if (record.BattleNetId < 1)
                {
                    record.BattleNetId = model.BattleNetId;
                    record.BattleNetRegionId = model.BattleNetRegionId;
                    record.BattleNetSubId = model.BattleNetSubId;
                    record.BattleNetTId = model.BattleNetTId;
                    record.LastSeen = model.LastSeen;
                }

                if (model.LastSeen > record.LastSeen)
                {
                    if (model.BattleTagName != record.BattleTagName)
                    {
                        ReplayRenamedPlayer samePlayer = new ReplayRenamedPlayer
                        {
                            PlayerId = record.PlayerId,
                            BattleNetId = record.BattleNetId,
                            BattleTagName = record.BattleTagName,
                            BattleNetRegionId = record.BattleNetRegionId,
                            BattleNetSubId = record.BattleNetSubId,
                            BattleNetTId = record.BattleNetTId,
                            DateAdded = model.LastSeen,
                        };

                        new RenamedPlayer().CreateRecord(db, samePlayer);
                    }

                    record.BattleTagName = model.BattleTagName; // update the player's battletag, it may have changed
                    record.BattleNetTId = model.BattleNetTId;
                    record.LastSeen = model.LastSeen;
                }

                record.Seen += 1;

                db.SaveChanges();
            }

            return record.PlayerId;
        }

        internal override bool IsExistingRecord(ReplaysContext db, ReplayAllHotsPlayer model)
        {
            if (model.BattleNetId > 0)
            {
                // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
                return db.ReplayAllHotsPlayers.Any(x => x.BattleNetId == model.BattleNetId &&
                                                        x.BattleNetRegionId == model.BattleNetRegionId &&
                                                        x.BattleNetSubId == model.BattleNetSubId);
            }
            else if (!string.IsNullOrEmpty(model.BattleNetTId))
            {
                // has tag but no BattleNetId - observer
                return db.ReplayAllHotsPlayers.Any(x => x.BattleNetTId == model.BattleNetTId);
            }
            else
            {
                // only choice left is to search by BattleTagName
                return db.ReplayAllHotsPlayers.Any(x => x.BattleTagName == model.BattleTagName && x.BattleNetRegionId == model.BattleNetRegionId);
            }
        }

        internal long ReadPlayerIdFromBattleNetId(ReplaysContext db, string battleTagName, int battleNetId)
        {
            // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
            return db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == battleTagName && x.BattleNetId == battleNetId).PlayerId;
        }
    }
}
