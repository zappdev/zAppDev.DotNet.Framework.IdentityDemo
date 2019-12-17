using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityDemo.DAL;
using IdentityDemo.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zAppDev.DotNet.Framework.Data;
using zAppDev.DotNet.Framework.Identity;
using zAppDev.DotNet.Framework.Identity.Model;
using zAppDev.DotNet.Framework.Utilities;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
        [OperationAuthorize("ManageUser", "GeneralOperation", ClaimTypes.GenericAction)]
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
        [OperationAuthorize("ManageUser", "EditUser", ClaimTypes.ControllerAction)]
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
            foreach(var role in applicationUser.Roles)
            {
                var roleDTO = new ApplicationRoleDTO
                { 
                    Description = role.Description,
                    Id = role.Id,
                    IsCustom = role.IsCustom,
                    Name = role.Name
                };
                foreach(var permission in role.Permissions)
                {
                    var permissionDTO = new ApplicationPermissionDTO
                    {
                        Description = permission.Description,
                        Id = permission.Id,
                        IsCustom = permission.IsCustom,
                        Name = permission.Name
                    };
                    roleDTO.Permissions.Add(permissionDTO);
                }
                userDto.roles.Add(roleDTO);
            }
            return Ok(new
                {
                    value = userDto
                }
            );

        }

        [HttpPut("{username}")]
        [OperationAuthorize("ManageUser", "SaveUser", ClaimTypes.ControllerAction)]
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
            applicationUser.ClearRoles();
            foreach(var roleDTO in userDTO.roles)
            {
                var applicationRole = repo.GetById<ApplicationRole>(roleDTO.Id);
                applicationUser.AddRoles(applicationRole);
            }
            repo.Save<ApplicationUser>(applicationUser);
            manager.Session.Flush();
            return NoContent();
        }
        
        [HttpPost]
        [OperationAuthorize("ManageUser", "SaveUser", ClaimTypes.ControllerAction)]
        public ActionResult<ApplicationUserDTO> PostUser(ApplicationUserDTO userDTO)
        {
            if (userDTO.password?.Trim() != userDTO.passwordRepeat?.Trim())
            {
                throw new Exception("Passwords do not match!");
            }
            if (userDTO.username == null || userDTO.username?.Trim() == "" )
            {
                throw new Exception("No username provided!");
            }
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var applicationUser = new ApplicationUser();
            applicationUser.UserName = userDTO.username;
            applicationUser.Name = userDTO.name;
            applicationUser.Email = userDTO.email;
            applicationUser.PhoneNumber = userDTO.phoneNumber;
            string possibleError = zAppDev.DotNet.Framework.Identity.IdentityHelper.CreateUser(applicationUser, (userDTO.password?.Trim() ?? ""));
            if ((((possibleError == null || possibleError == "")) == false))
            {
                throw new Exception("Something Went Wrong");
            }
            foreach(var roleDTO in userDTO.roles)
            {
                var role = repo.GetById<ApplicationRole>(roleDTO.Id);
                applicationUser.AddRoles(role);
            }
            repo.Save<ApplicationUser>(applicationUser);
            manager.Session.Flush();
            return CreatedAtAction("PostUser", new { id = applicationUser.UserName});
        }

        [HttpDelete("{id}")]
        [OperationAuthorize("ManageUser", "DeleteUser", ClaimTypes.ControllerAction)]
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
