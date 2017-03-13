﻿using HeroesMatchData.Data;
using HeroesMatchData.Data.Models.ReleaseNotes;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HeroesMatchData.Core.ReleaseNotes
{
    public class ReleaseNoteHandler
    {
        private IReadOnlyList<Release> Releases;
        private IDatabaseService Database;

        public ReleaseNoteHandler(IDatabaseService database)
        {
            Database = database;
        }

        public async Task InitializeClient()
        {
            var client = new GitHubClient(new ProductHeaderValue("HeroesMatchData", AssemblyVersions.HeroesMatchDataVersion().ToString()));
            Releases = await client.Repository.Release.GetAll("koliva8245", "HeroesMatchData");
        }

        /// <summary>
        /// Adds the newest release notes to the database and also updates the latest release note
        /// </summary>
        public void AddApplyReleasesReleaseNotes()
        {
            var latestVersion = new Version(Database.ReleaseNotesDb().ReleaseNotes.ReadLastReleaseNote().Version);

            foreach (var release in Releases)
            {
                if (new Version(release.TagName.TrimStart('v')) >= latestVersion)
                {
                    AddReleaseNote(release);
                }
                else
                {
                    // add one more so its updated
                    AddReleaseNote(release);
                    break;
                }
            }
        }

        /// <summary>
        /// Adds all the Releases to the database
        /// </summary>
        public void AddAllReleaseNotes()
        {
            foreach (var releaseNote in Releases)
                AddReleaseNote(releaseNote);
        }

        /// <summary>
        /// Adds all the Releases up to the current version of the application to the database
        /// </summary>
        public void AddAllReleasesUpToCurrentVersion()
        {
            Version versionTemp = AssemblyVersions.HeroesMatchDataVersion().Version;
            Version currentVersion = new Version(versionTemp.Major, versionTemp.Minor, versionTemp.Build);

            foreach (var releaseNote in Releases)
            {
                if (new Version(releaseNote.TagName.TrimStart('v')) <= currentVersion)
                    AddReleaseNote(releaseNote);
            }
        }

        // adds the release to the database
        private void AddReleaseNote(Release release)
        {
            string version = release.TagName.TrimStart('v');

            ReleaseNote releaseNote = new ReleaseNote
            {
                DateReleased = release.PublishedAt.Value.DateTime,
                PatchNote = release.Body.Replace("\r\n", Environment.NewLine),
                PreRelease = release.Prerelease,
                Version = version,
            };

            if (Database.ReleaseNotesDb().ReleaseNotes.IsExistingReleaseNote(releaseNote))
                Database.ReleaseNotesDb().ReleaseNotes.UpdateReleaseNote(releaseNote);
            else
                Database.ReleaseNotesDb().ReleaseNotes.CreateRecord(releaseNote);
        }
    }
}