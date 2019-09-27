using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using IdentityDemo.Domain;
using IdentityDemo.DTOs;
using zAppDev.DotNet.Framework.Utilities;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        public ServiceLocator ServiceLocator { get; }

        private ISession _session { get; set; }
        public TeamsController(ISession session, IServiceProvider serviceProvider)
        {
            _session = session;
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        [HttpGet("list")]
        public IActionResult GetTeams()
        {
            var teams = _session.CreateCriteria<Team>().List<Team>();
            var results = new List<TeamDTO>();
            foreach (var team in teams)
            {
                var dto = new TeamDTO
                {
                    Founded = team.Founded,
                    Id = team.Id,
                    Name = team.Name,
                    City = team.City
                };
                results.Add(dto);
            }
            return Ok(new
            {
                value = results
            });
        }

        [HttpGet("{id}")]
        public ActionResult<TeamDTO> GetTeam(long id)
        {
            var team = _session.Get<Team>(id);

            if (team == null)
            {
                return NotFound();
            }

            var teamDto = new TeamDTO
            {
                Founded = team.Founded,
                Name = team.Name,
                City = team.City,
                Id = team.Id
            };

            return teamDto;
        }

        [HttpPut("{id}")]
        public ActionResult PutTeam(Team team)
        {
            _session.Update(team);
            _session.Flush();
            return NoContent();
        }

        [HttpPost]
        public ActionResult<Team> PostTeam(Team team)
        {
            _session.Save(team);
            _session.Flush();

            return CreatedAtAction("PostTeam", new { id = team.Id }, team);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTeam(long id)
        {
            var team = _session.Get<Team>(id);
            if (team == null)
            {
                return NotFound();
            }
            _session.Delete(team);
            _session.Flush();
            return Ok();
        }
    }
}
