using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NHibernate;
using IdentityDemo.DTOs;
using IdentityDemo.DAL;
using zAppDev.DotNet.Framework.Utilities;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAdminController : ControllerBase
    {
        private ISession _session { get; set; }
        private ServiceLocator ServiceLocator { get; set; }
        public CreateAdminController(ISession session,IServiceProvider serviceProvider)
        {
            _session = session;
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        [HttpPost]
        public ActionResult<zAppDev.DotNet.Framework.Identity.Model.ApplicationUser> CreateAdmin(ApplicationUserDTO userDTO)
        {
            if (userDTO.password?.Trim() != userDTO.passwordRepeat?.Trim())
            {
                throw new Exception("Passwords do not match!");
            }
            if (userDTO.username?.Trim() == "")
            {
                throw new Exception("No username provided!");
            }
            zAppDev.DotNet.Framework.Identity.Model.ApplicationRole adminRole = new Repository(_session).GetAsQueryable<zAppDev.DotNet.Framework.Identity.Model.ApplicationRole>((r) => r.Name == "Administrator")?.FirstOrDefault();
            if ((adminRole == null))
            {
                throw new Exception("No Administrator role found in Database!");
            }
            zAppDev.DotNet.Framework.Identity.Model.ApplicationUser adminUser = new zAppDev.DotNet.Framework.Identity.Model.ApplicationUser();
            adminUser.UserName = (userDTO.username?.Trim() ?? "");
            adminUser?.AddRoles(adminRole);
            string possibleError = zAppDev.DotNet.Framework.Identity.IdentityHelper.CreateUser(adminUser, (userDTO.password?.Trim() ?? ""));
            if ((((possibleError == null || possibleError == "")) == false))
            {
                throw new Exception("Something Went Wrong");
            }
            return CreatedAtAction("CreateAdmin", new { id = adminUser.UserName }, adminUser);
        }
    }
}
