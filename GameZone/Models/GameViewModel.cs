using GameZone.Data;
using System.ComponentModel.DataAnnotations;
using static GameZone.Constants.ModelConstants;
namespace GameZone.Models
{
    public class GameViewModel
    {



        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = string.Empty;


        public string? ImageUrl { get; set; }


        [Required]
        public string ReleasedOn { get; set; } = DateTime.Today.ToString(DateTimeFormat);

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int GenreId { get; set; } 

       public List<Genre> Genres { get; set; } = new List<Genre>();
    }
}
