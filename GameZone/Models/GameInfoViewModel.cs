namespace GameZone.Models
{
    public class GameInfoViewModel
    {

        public int Id { get; set; }

        public required string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public  string ReleasedOn { get; set; } = null!;
      

        public required string Publisher { get; set; } = null!;

        public required string Genre { get; set; } = null!;

       

       
    }
}
