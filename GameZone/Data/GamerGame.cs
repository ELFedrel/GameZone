using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace GameZone.Data
{
    [PrimaryKey(nameof(GameId), nameof(GamerId))]
    public class GamerGame
    {
//        •	Has GameId – integer, PrimaryKey, foreign key(required)
//•	Has Game – Game
//•	Has GamerId – string, PrimaryKey, foreign key(required)
//•	Has Gamer – IdentityUser

        public int GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game Game { get; set; }


        public string GamerId { get; set; }

        [ForeignKey(nameof(GamerId))]
        public IdentityUser Gamer { get; set; }

    }
}
