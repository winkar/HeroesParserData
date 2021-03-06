﻿using HeroesParserData.Models.DbModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;

namespace HeroesParserData.DataQueries
{
    public static partial class Query
    {
        internal static class HotsPlayer
        {
            public static long CreateRecord(ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                using (var db = new HeroesParserDataContext())
                {
                    db.ReplayAllHotsPlayers.Add(replayAllHotsPlayer);
                    db.SaveChanges();
                }

                return replayAllHotsPlayer.PlayerId;
            }

            public static long CreateRecord(HeroesParserDataContext db, ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                db.ReplayAllHotsPlayers.Add(replayAllHotsPlayer);
                db.SaveChanges();

                return replayAllHotsPlayer.PlayerId;
            }

            public static void DeleteRecord(HeroesParserDataContext db, long playerId)
            {
                db.ReplayAllHotsPlayers.Remove(new ReplayAllHotsPlayer
                {
                    PlayerId = playerId
                });

                db.SaveChanges();
            }

            public static List<ReplayAllHotsPlayer> ReadAllRecords()
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.ToList();
                }
            }

            public static List<ReplayAllHotsPlayer> ReadTopRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.Take(num).ToList();
                }
            }

            public static List<ReplayAllHotsPlayer> ReadLastRecords(int num)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.OrderByDescending(x => x.PlayerId).Take(num).ToList();
                }
            }

            public static List<ReplayAllHotsPlayer> ReadRecordsCustomTop(int count, string columnName, string orderBy)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(orderBy))
                    return new List<ReplayAllHotsPlayer>();

                if (count == 0)
                    count = 1;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM ReplayAllHotsPlayers ORDER BY {columnName} {orderBy} LIMIT {count}").ToList();
                }
            }

            public static List<ReplayAllHotsPlayer> ReadRecordsWhere(string columnName, string operand, string input)
            {
                if (string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(operand))
                    return new List<ReplayAllHotsPlayer>();

                if (Utilities.LikeOperatorInputCheck(operand, input))
                    input = $"%{input}%";
                else if (input == null)
                    input = string.Empty;

                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.SqlQuery($"SELECT * FROM ReplayAllHotsPlayers WHERE {columnName} {operand} @Input", new SQLiteParameter("@Input", input)).ToList();
                }
            }

            public static bool IsExistingHotsPlayer(ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.Any(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName);
                }
            }

            public static bool IsExistingHotsPlayer(HeroesParserDataContext db, ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                if (replayAllHotsPlayer.BattleNetId > 0)
                {
                    // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
                    return db.ReplayAllHotsPlayers.Any(x => x.BattleNetId == replayAllHotsPlayer.BattleNetId &&
                                                            x.BattleNetRegionId == replayAllHotsPlayer.BattleNetRegionId &&
                                                            x.BattleNetSubId == replayAllHotsPlayer.BattleNetSubId);
                }
                else if (!string.IsNullOrEmpty(replayAllHotsPlayer.BattleNetTId))
                {
                    // has tag but no BattleNetId - observer
                    return db.ReplayAllHotsPlayers.Any(x => x.BattleNetTId == replayAllHotsPlayer.BattleNetTId);
                }
                else 
                {
                    // only choice left is to search by BattleTagName
                    return db.ReplayAllHotsPlayers.Any(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName && x.BattleNetRegionId == replayAllHotsPlayer.BattleNetRegionId);
                }
            }

            public static long UpdateRecord(HeroesParserDataContext db, ReplayAllHotsPlayer replayAllHotsPlayer)
            {
                ReplayAllHotsPlayer record;

                if (replayAllHotsPlayer.BattleNetId > 0)
                {
                    record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleNetId == replayAllHotsPlayer.BattleNetId &&
                                                                          x.BattleNetRegionId == replayAllHotsPlayer.BattleNetRegionId &&
                                                                          x.BattleNetSubId == replayAllHotsPlayer.BattleNetSubId);
                }       
                else // if its an observer with no battlenetid
                {
                    if (!string.IsNullOrEmpty(replayAllHotsPlayer.BattleNetTId))
                        record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleNetTId == replayAllHotsPlayer.BattleNetTId);
                    else
                        record = db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == replayAllHotsPlayer.BattleTagName && x.BattleNetRegionId == replayAllHotsPlayer.BattleNetRegionId);
                }

                if (record != null)
                {
                    // existing observer record, update the info
                    if (record.BattleNetId < 1)
                    {
                        record.BattleNetId = replayAllHotsPlayer.BattleNetId;
                        record.BattleNetRegionId = replayAllHotsPlayer.BattleNetRegionId;
                        record.BattleNetSubId = replayAllHotsPlayer.BattleNetSubId;
                        record.BattleNetTId = replayAllHotsPlayer.BattleNetTId;
                        record.LastSeen = replayAllHotsPlayer.LastSeen;
                    }

                    if (replayAllHotsPlayer.LastSeen > record.LastSeen)
                    {
                        if (replayAllHotsPlayer.BattleTagName != record.BattleTagName)
                        {
                            ReplayRenamedPlayer samePlayer = new ReplayRenamedPlayer
                            {
                                PlayerId = record.PlayerId,
                                BattleNetId = record.BattleNetId,
                                BattleTagName = record.BattleTagName,
                                BattleNetRegionId = record.BattleNetRegionId,
                                BattleNetSubId = record.BattleNetSubId,
                                BattleNetTId = record.BattleNetTId,
                                DateAdded = replayAllHotsPlayer.LastSeen
                            };

                            RenamedPlayer.CreateRecord(db, samePlayer);
                        }
                        record.BattleTagName = replayAllHotsPlayer.BattleTagName; // update the player's battletag, it may have changed
                        record.BattleNetTId = replayAllHotsPlayer.BattleNetTId;
                        record.LastSeen = replayAllHotsPlayer.LastSeen;
                    }

                    record.Seen += 1;

                    db.SaveChanges();

                }

                return record.PlayerId;
            }

            public static long ReadPlayerIdFromBattleNetId(HeroesParserDataContext db, string battleTagName, int battleNetId)
            {
                // battleNetId is not unique, player can change their battletag and their battleNetId stays the same
                return db.ReplayAllHotsPlayers.SingleOrDefault(x => x.BattleTagName == battleTagName && x.BattleNetId == battleNetId).PlayerId;
            }

            public static long ReadPlayerIdFromBattleNetTag(string battleTagName)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.FirstOrDefault(x => x.BattleTagName == battleTagName).PlayerId;
                }
            }

            public static ReplayAllHotsPlayer ReadRecordFromPlayerId(long playerId)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.Where(x => x.PlayerId == playerId).FirstOrDefault();
                }
            }

            public static bool IsValidBattleNetTagName(string battleTagName)
            {
                using (var db = new HeroesParserDataContext())
                {
                    return db.ReplayAllHotsPlayers.Any(x => x.BattleTagName == battleTagName);
                }
            }
        }
    }
}
