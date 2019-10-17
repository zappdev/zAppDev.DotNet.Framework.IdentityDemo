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
    public class PermissionsController : ControllerBase
    {
        public ServiceLocator ServiceLocator { get; }

        public PermissionsController(IServiceProvider serviceProvider)
        {
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetApplicationPermissions()
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);

            var permissions = repo.GetAll<ApplicationPermission>();
            var results = new List<ApplicationPermissionDTO>();
            foreach (var permission in permissions)
            {
                var permissionDTO = new ApplicationPermissionDTO
                {
                    Description = permission.Description,
                    Id = permission.Id,
                    IsCustom = permission.IsCustom,
                    Name = permission.Name
                };
                results.Add(permissionDTO);
            }

            return Ok(new
            {
                value = results
            }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationPermissions(int? id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            ApplicationPermission applicationPermission;
            try
            {
                applicationPermission = repo.GetById<ApplicationPermission>(id);
            }
            catch
            {
                return new NotFoundResult();
            }
            var applicationPermissionDTO = new ApplicationPermissionDTO
            { 
                Description = applicationPermission.Description,
                Id = applicationPermission.Id,
                IsCustom = applicationPermission.IsCustom,
                Name = applicationPermission.Name
            };

            return Ok(new
            {
                value = applicationPermissionDTO
            }
            );

        }

        [HttpPut("{id}")]
        public ActionResult PutApplicationPermission(ApplicationPermissionDTO applicationPermissionsDTO, int? id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var applicationPermission = repo.GetById<ApplicationPermission>(id);
            if (applicationPermission == null)
            {
                return new NotFoundResult();
            }
            applicationPermission.Name = applicationPermissionsDTO.Name;
            applicationPermission.IsCustom = applicationPermissionsDTO.IsCustom;
            applicationPermission.Description = applicationPermissionsDTO.Description;
            repo.Save<ApplicationPermission>(applicationPermission);
            manager.Session.Flush();
            return NoContent();
        }

        [HttpPost]
        public ActionResult PostPermission(ApplicationPermissionDTO applicationPermissionDTO)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);

            var applicationPermission = new ApplicationPermission();
            applicationPermission.Name = applicationPermissionDTO.Name;
            applicationPermission.Description = applicationPermission.Description;
            applicationPermission.IsCustom = applicationPermission.IsCustom;

            repo.Save<ApplicationPermission>(applicationPermission);
            manager.Session.Flush();
            return CreatedAtAction("PostPermission", new { id = applicationPermission.Id}, applicationPermission);
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePermission(int id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var permission = repo.GetById<ApplicationPermission>(id);
            if (permission == null)
            {
                return NotFound();
            }
            repo.DeleteApplicationPermission(permission);
            return Ok();
        }
    }
}
