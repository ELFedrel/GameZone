using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;
using static GameZone.Constants.ModelConstants;

namespace GameZone.Data
{
    public class Game
    {
        //        Game
        //•	Has Id – a unique integer, Primary Key
        //•	Has Title – a string with min length 2 and max length 50 (required)
        //•	Has Description – string with min length 10 and max length 500 (required)
        //•	Has ImageUrl – nullable string
        //•	Has PublisherId – string (required)
        //•	Has Publisher – IdentityUser(required)
        //•	Has ReleasedOn– DateTime with format " yyyy-MM-dd" (required) (the DateTime format is recommended, if you are having troubles with this one, you are free to use another one)
        //•	Has GenreId – integer, foreign key (required)
        //•	Has Genre – Genre(required)
        //•	Has GamersGames – a collection of type GamerGame

        [Key]
        [Comment("A unique integer, Primary Key")]
        public int Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        [Comment("Game Title")]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [Comment("Game Description")]
        public string Description { get; set; } = null!;


        [Comment("Game Image URL")]
        public string? ImageUrl { get; set; }


        [Required]
        
        [Comment("Publisher Id")]
        public string PublisherId { get; set; } = null!;

        [ForeignKey(nameof(PublisherId))]
        public IdentityUser Publisher { get; set; } = null!;

        [Required]
        [Column("Date")]
        public DateTime ReleasedOn { get; set; }

        [Required]
        [Comment("Genre Id")]
        public int GenreId { get; set; }

        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; } =null!;

        public IList<GamerGame> GamersGames { get; set; }=new List<GamerGame>();

        [Comment("Is Deleated")]
        public bool IsDeleated { get; set; }


    }
}
