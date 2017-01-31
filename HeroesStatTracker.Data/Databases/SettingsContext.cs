namespace HeroesStatTracker.Data.Databases
{
    using Models.Settings;
    using SQLite.CodeFirst;
    using System.Data.Entity;

    internal class SettingsContext : StatTrackerDbContext
    {
        public SettingsContext()
            : base($"name={Properties.Settings.Default.SettingsConnNameDb}") { }

        public virtual DbSet<UserSetting> UserSettings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<SettingsContext>(modelBuilder));
        }
    }
}