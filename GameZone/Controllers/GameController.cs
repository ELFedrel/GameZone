using GameZone.Data;
using GameZone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static GameZone.Constants.ModelConstants;


namespace GameZone.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameZoneDbContext context;

        public GameController(GameZoneDbContext context)
        {
           this.context = context;
        }


        [HttpGet]
        public async Task<IActionResult> All()
        {
            var model = await context.Games
                .Where(g => g.IsDeleated == false)
                .Select(g => new GameInfoViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    ImageUrl = g.ImageUrl,
                    ReleasedOn = g.ReleasedOn.ToString(DateTimeFormat),
                    Publisher = g.Publisher.UserName ?? string.Empty,
                    Genre = g.Genre.Name
                    
                    
                })
                .AsNoTracking()
                .ToListAsync();
               
            
                

               
            return View(model);

        }


        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new GameViewModel();
            model.Genres = await GetGanres();


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(GameViewModel model)
        {


            if (!ModelState.IsValid)
            {
                model.Genres = await GetGanres();
                return View(model);
            }

            DateTime releasedOn;
            if (!DateTime.TryParse(model.ReleasedOn, out releasedOn))
            {
                ModelState.AddModelError(nameof(model.ReleasedOn), "Invalid date format.");
                model.Genres = await GetGanres();
                return View(model);
            }

            Game game = new Game
            {
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                PublisherId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                ReleasedOn = releasedOn,
                GenreId = model.GenreId,
               
            };

            await this.context.Games.AddAsync(game);
            await this.context.SaveChangesAsync();
            return RedirectToAction(nameof(All));
            


           
        }
     


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await context.Games
                .Where(g => g.Id == id)
                .Where(g => g.IsDeleated == false)
                .AsNoTracking()
                .Select(g => new GameViewModel
                {
                   Title = g.Title,
                   Description = g.Description,
                   ImageUrl = g.ImageUrl,
                   ReleasedOn = g.ReleasedOn.ToString(DateTimeFormat),
                   GenreId = g.GenreId
                   


                })
                .FirstOrDefaultAsync();

            model.Genres = await GetGanres();


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameViewModel model, int id)
        {
           

            if (!ModelState.IsValid)
            {
                model.Genres = await GetGanres();
                return View(model);
            }

            DateTime releasedOn;
            if (!DateTime.TryParse(model.ReleasedOn, out releasedOn))
            {
                ModelState.AddModelError(nameof(model.ReleasedOn), "Invalid date format.");
                model.Genres = await GetGanres();
                return View(model);
            }



            Game? game = await context.Games.FindAsync(id);
           

            if (game == null || game.IsDeleated)
            {
                throw new ArgumentException("Invalid game.");
            }

            string curentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            if (game.PublisherId != curentUserId)
            {
                throw new InvalidOperationException("You are not the publisher of this game.");
            }

            game.Title = model.Title;
            game.Description = model.Description;
            game.ImageUrl = model.ImageUrl;
            game.ReleasedOn = releasedOn;
            game.GenreId = model.GenreId;
            game.PublisherId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;


            
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(All));


        }

        [HttpGet]
        public async Task<IActionResult> MyZone()
        {
            string curentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            var model = await context.Games
                .Where(g => g.IsDeleated == false)
                .Where(g => g.GamersGames.Any(gr => gr.GamerId == curentUserId))
                .Select(g => new GameInfoViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    ImageUrl = g.ImageUrl,
                    ReleasedOn = g.ReleasedOn.ToString(DateTimeFormat),
                    Publisher = g.Publisher.UserName ?? string.Empty,
                    Genre = g.Genre.Name


                })
                .AsNoTracking()
                .ToListAsync();





            return View(model);

          

        }

        [HttpGet]
        public async Task<IActionResult> AddToMyZone(int id)
        {
            Game? game = await context.Games
                .Where(g => g.Id == id)
                .Include(g => g.GamersGames)
                .FirstOrDefaultAsync();


            if (game == null || game.IsDeleated)
            {
                throw new ArgumentException("Invalid game.");
            }

            
            string curentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            if (game.GamersGames.Any(gg => gg.GamerId == curentUserId) )
            {
                
                return RedirectToAction(nameof(MyZone));

                
               
            }
            game.GamersGames.Add(new GamerGame
            {
                GamerId = curentUserId,
                GameId = game.Id
            });

            return RedirectToAction(nameof(MyZone));

        }


        [HttpGet]
        public async Task<IActionResult> StrikeOut(int id)
        {
            Game? game = await context.Games
                 .Where(g => g.Id == id)
                 .Include(g => g.GamersGames)
                 .FirstOrDefaultAsync();


            if (game == null || game.IsDeleated)
            {
                throw new ArgumentException("Invalid game.");
            }


            string curentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

            GamerGame? gamerGame = game.GamersGames
                .FirstOrDefault(gg => gg.GamerId == curentUserId);
            if (gamerGame != null)
            {
                game.GamersGames.Remove(gamerGame);
                

                await context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(MyZone));

        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await context.Games
               .Where(g => g.Id == id)
               .Where(g => g.IsDeleated == false)
               .AsNoTracking()
               .Select(g => new GameDetailsViewModel
               {
                     Id = g.Id,
                     Title = g.Title,
                     Description = g.Description,
                     ImageUrl = g.ImageUrl,
                     ReleasedOn = g.ReleasedOn.ToString(DateTimeFormat),
                     Publisher = g.Publisher.UserName ?? string.Empty,
                     Genre = g.Genre.Name,
                   



               })
               .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Games
              .Where(g => g.Id == id)
              .Where(g => g.IsDeleated == false)
              .AsNoTracking()
              .Select(g => new DeleteViewModel
              {
                  Id = g.Id,
                  Title = g.Title,
                  Publisher = g.Publisher.UserName ?? string.Empty,
                 


              })
              .FirstOrDefaultAsync();

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(DeleteViewModel model)
        {
            Game? game = await context.Games
                .Where(g => g.Id == model.Id)
                .Where(g => g.IsDeleated == false)
                .FirstOrDefaultAsync();

            if (game != null)
            {
                
                
                game.IsDeleated = true;
                context.SaveChangesAsync();

            }

            return RedirectToAction(nameof(All));   

        }



        private async Task<List<Genre>> GetGanres()
        {
            return await context.Genres.ToListAsync();
        }


       


    }
}
