using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityDemo.DAL;
using IdentityDemo.DTOs;
using Microsoft.AspNetCore.Mvc;
using zAppDev.DotNet.Framework.Data;
using zAppDev.DotNet.Framework.Identity.Model;
using zAppDev.DotNet.Framework.Utilities;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public ServiceLocator ServiceLocator { get; }

        public UsersController(IServiceProvider serviceProvider)
        {
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetApplicationUsers()
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);

            var users = repo.GetAll<ApplicationUser>();
            var results = new List<ApplicationUserDTO>();
            foreach(var user in users)
            {
                var userDto = new ApplicationUserDTO
                {
                    email = user.Email,
                    name = user.Name,
                    username = user.UserName,
                    phoneNumber = user.PhoneNumber
                    
                };
                results.Add(userDto);
            }
            
            return Ok(new 
                { 
                    value = results
                }
            );
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetApplicationUser(string username)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            ApplicationUser applicationUser;
            try
            {
                 applicationUser = repo.GetById<ApplicationUser>(username);
            }
            catch 
            {
                return new NotFoundResult();
            }
            var userDto = new ApplicationUserDTO
            { 
                accessFailedCount = applicationUser.AccessFailedCount,
                email = applicationUser.Email,
                lockoutEnabled = applicationUser.LockoutEnabled,
                lockoutEndDate = applicationUser.LockoutEndDate,
                name = applicationUser.Name,
                phoneNumber = applicationUser.PhoneNumber,
                username = applicationUser.UserName
            };
            
            return Ok(new
                {
                    value = userDto
                }
            );

        }

        [HttpPut("{username}")]
        public ActionResult PutUser(ApplicationUserDTO userDTO,string username)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var applicationUser = repo.GetById<ApplicationUser>(username);
            if(applicationUser == null)
            {
                return new NotFoundResult();
            }
            applicationUser.Name = userDTO.name;
            applicationUser.Email = userDTO.email;
            applicationUser.PhoneNumber = userDTO.phoneNumber;
            repo.Save<ApplicationUser>(applicationUser);
            manager.Session.Flush();
            return NoContent();
        }
        
        [HttpPost]
        public ActionResult<ApplicationUserDTO> PostTeam(ApplicationUser user)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            repo.Save<ApplicationUser>(user);
            return CreatedAtAction("PostUser", new { id = user.UserName}, user);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(string username)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var user = repo.GetById<ApplicationUser>(username);
            if (user == null)
            {
                return NotFound();
            }
            repo.DeleteApplicationUser(user);
            return Ok();
        }
    }
}
