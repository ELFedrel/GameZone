using Humanizer;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static GameZone.Constants.ModelConstants;
namespace GameZone.Data
{
    public class Genre
    {
        //•	Has Id – a unique integer, Primary Key

        //        · Name – a string with min length 3 and max length 25 (required)

        //· Has Games – a collection of type Game

        [Key]
        [Comment("A unique integer, Primary Key")]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        [Comment("Genre Name")]
        public string Name { get; set; } = null!;


        public ICollection<Game> Games { get; set; } = new HashSet<Game>();


    }
}
