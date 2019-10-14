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
    public class RolesController : ControllerBase
    {
        public ServiceLocator ServiceLocator { get; }

        public RolesController(IServiceProvider serviceProvider)
        {
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetApplicationRoles()
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);

            var roles = repo.GetAll<ApplicationRole>();
            var results = new List<ApplicationRoleDTO>();
            foreach (var role in roles)
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
                results.Add(roleDTO);
            }

            return Ok(new
            {
                value = results
            }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationRole(int? id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            ApplicationRole applicationRole;
            try
            {
                applicationRole = repo.GetById<ApplicationRole>(id);
            }
            catch
            {
                return new NotFoundResult();
            }
            var applicationRoleDTO = new ApplicationRoleDTO
            {
                Description = applicationRole.Description,
                Id = applicationRole.Id,
                IsCustom = applicationRole.IsCustom,
                Name = applicationRole.Name
            };
            foreach(var permisison in applicationRole.Permissions)
            {
                var pemrissionDTO = new ApplicationPermissionDTO
                {
                    Description = permisison.Description,
                    Id = permisison.Id,
                    IsCustom = permisison.IsCustom,
                    Name = permisison.Name
                };
                applicationRoleDTO.Permissions.Add(pemrissionDTO);
            }

            return Ok(new
            {
                value = applicationRoleDTO
            }
            );

        }

        [HttpPut("{id}")]
        public ActionResult PutApplicationRole(ApplicationRoleDTO applicationRoleDTO, int? id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var applicationRole = repo.GetById<ApplicationRole>(id);
            if (applicationRole == null)
            {
                return new NotFoundResult();
            }
            applicationRole.Description = applicationRoleDTO.Description;
            applicationRole.Name = applicationRoleDTO.Name;
            applicationRole.IsCustom = applicationRoleDTO.IsCustom;
            applicationRole.ClearPermissions();
            foreach(var permissionDTO in applicationRoleDTO.Permissions)
            {
                var applicationPermission = repo.GetById<ApplicationPermission>(permissionDTO.Id);
                applicationRole.AddPermissions(applicationPermission);
            }
            
            repo.Save<ApplicationRole>(applicationRole);
            manager.Session.Flush();
            return NoContent();
        }

        [HttpPost]
        public ActionResult<ApplicationRoleDTO> PostRole(ApplicationRoleDTO applicationRoleDTO)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);

            var applicationRole = new ApplicationRole
            {
                Description = applicationRoleDTO.Description,
                Name = applicationRoleDTO.Name,
                IsCustom = applicationRoleDTO.IsCustom
            };
            foreach (var permissionDTO in applicationRoleDTO.Permissions)
            {
                var applicationPermission = repo.GetById<ApplicationPermission>(permissionDTO.Id);
                applicationRole.AddPermissions(applicationPermission);
            }
            repo.Save<ApplicationRole>(applicationRole);
            manager.Session.Flush();
            return CreatedAtAction("PostRole", new { id = applicationRole.Id }, applicationRole);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRole(int id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var role = repo.GetById<ApplicationRole>(id);
            if (role == null)
            {
                return NotFound();
            }
            repo.DeleteApplicationRole(role);
            return Ok();
        }

    }
}
