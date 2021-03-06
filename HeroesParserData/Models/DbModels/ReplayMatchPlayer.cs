namespace HeroesParserData.Models.DbModels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class ReplayMatchPlayer : IReplayDataTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MatchPlayerId { get; set; }

        public long ReplayId { get; set; }

        public int? Team { get; set; }

        public int PlayerNumber { get; set; }

        public long PlayerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Character { get; set; }

        public int CharacterLevel { get; set; }

        public long PartyValue { get; set; }

        [StringLength(25)]
        public string Difficulty { get; set; }

        public int? Handicap { get; set; }

        public bool IsAutoSelect { get; set; }

        public bool IsSilenced { get; set; }

        public bool IsWinner { get; set; }

        [StringLength(50)]
        public string MountAndMountTint { get; set; }

        [StringLength(50)]
        public string SkinAndSkinTint { get; set; }

        public virtual Replay Replay { get; set; }

        public virtual ReplayAllHotsPlayer ReplayAllHotsPlayer { get; set; }
    }
}
