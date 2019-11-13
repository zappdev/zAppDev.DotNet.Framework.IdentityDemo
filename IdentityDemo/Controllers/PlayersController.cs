using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using IdentityDemo.Domain;
using IdentityDemo.DTOs;
using zAppDev.DotNet.Framework.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        public ServiceLocator ServiceLocator { get; }
        private ISession _session { get; set; }
        public PlayersController(ISession session, IServiceProvider serviceProvider)
        {
            _session = session;
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }
        
        [HttpGet("list")]
        public async Task<IActionResult> GetPlayers()
        {
            var players = _session.CreateCriteria<Player>().List<Player>();
            var results = new List<PlayerDTO>();
            foreach (var player in players)
            {
                var dto = new PlayerDTO
                {
                    DateOfBirth = player.DateOfBirth,
                    Id = player.Id,
                    FirstName = player.FirstName,
                    LastName = player.LastName,
                    Team = player.Team?.Name
                };
                results.Add(dto);
            }

            return Ok(new
            {
                value = results
            });
        }

        [HttpGet("{id}")]
        public ActionResult<Player> GetPlayer(long id)
        {
            var player = _session.Get<Player>(id);
            if (player == null)
            {
                return NotFound();
            }


            return player;
        }

        [HttpPut("{id}")]
        public ActionResult PutPlayer(long id, Player player)
        {
            var savePlayer = _session.Get<Player>(id);

            savePlayer.DateOfBirth = player.DateOfBirth;
            savePlayer.FirstName = player.FirstName;
            savePlayer.LastName = player.LastName;
            savePlayer.Team = player.Team;

            _session.Update(savePlayer);
            _session.Flush();

            return NoContent();
        }

        [HttpPost]
        public ActionResult<Player> PostPlayer(Player player)
        {
            _session.Save(player);
            _session.Flush();

            return CreatedAtAction("PostPlayer", new { id = player.Id }, player);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePlayer(long id)
        {
            var player = _session.Get<Player>(id);
            if (player == null)
            {
                return NotFound();
            }
            _session.Delete(player);
            _session.Flush();
            return Ok();
        }
    }
}
