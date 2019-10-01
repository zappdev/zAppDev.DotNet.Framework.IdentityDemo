using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using IdentityDemo.DTOs;
using IdentityDemo.DAL;
using zAppDev.DotNet.Framework.Utilities;
using zAppDev.DotNet.Framework.Data.DAL;
using zAppDev.DotNet.Framework.Data;
using zAppDev.DotNet.Framework.Identity.Model;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAdminController : ControllerBase
    {   
        private ServiceLocator ServiceLocator { get; set; }
        public CreateAdminController(IServiceProvider serviceProvider)
        {
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        [HttpPost]
        public ActionResult<ApplicationUser> CreateAdmin(ApplicationUserDTO userDTO)
        {
            if (userDTO.password?.Trim() != userDTO.passwordRepeat?.Trim())
            {
                throw new Exception("Passwords do not match!");
            }
            if (userDTO.username?.Trim() == "")
            {
                throw new Exception("No username provided!");
            }
            
            var adminUser = new ApplicationUser();
            adminUser.UserName = (userDTO.username?.Trim() ?? "");
            string possibleError = zAppDev.DotNet.Framework.Identity.IdentityHelper.CreateUser(adminUser, (userDTO.password?.Trim() ?? ""));
            if ((((possibleError == null || possibleError == "")) == false))
            {
                throw new Exception("Something Went Wrong");
            }

            var repo = new Repository();
            var adminRole = repo.GetAsQueryable<ApplicationRole>((r) => r.Name == "Administrator")?.FirstOrDefault();
            if ((adminRole == null))
            {
                throw new Exception("No Administrator role found in Database!");
            }
            var appAdminUser = repo.GetAsQueryable<ApplicationUser>(a => a.UserName == userDTO.username).FirstOrDefault();
            appAdminUser.AddRoles(adminRole);
            repo.Save(appAdminUser);

            return CreatedAtAction("CreateAdmin", new { id = adminUser.UserName }, adminUser);
        }
    }
}
