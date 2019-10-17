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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        public ServiceLocator ServiceLocator { get; }

        public OperationsController(IServiceProvider serviceProvider)
        {
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetApplicationOperations()
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);

            var operations = repo.GetAll<ApplicationOperation>();
            var results = new List<ApplicationOperationDTO>();
            foreach (var operation in operations)
            {
                var operationDTO = new ApplicationOperationDTO
                {
                    Id = operation.Id,
                    IsAvailableToAllAuthorizedUsers = operation.IsAvailableToAllAuthorizedUsers,
                    IsAvailableToAnonymous = operation.IsAvailableToAnonymous,
                    Name = operation.Name,
                    ParentControllerName = operation.ParentControllerName,
                    Type = operation.Type                    
                };
                foreach(var permission in operation.Permissions)
                {
                    var permissionDTO = new ApplicationPermissionDTO
                    {
                        Id = permission.Id,
                        Description = permission.Description,
                        IsCustom = permission.IsCustom,
                        Name = permission.Name
                    };
                    operationDTO.Permissions.Add(permissionDTO);
                }
                results.Add(operationDTO);
            }

            return Ok(new
            {
                value = results
            }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationOperation(int? id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            ApplicationOperation applicationOperation;
            try
            {
                applicationOperation = repo.GetById<ApplicationOperation>(id);
            }
            catch
            {
                return new NotFoundResult();
            }
            var applicationOperationDTO = new ApplicationOperationDTO
            {
                Id = applicationOperation.Id,
                IsAvailableToAllAuthorizedUsers = applicationOperation.IsAvailableToAllAuthorizedUsers,
                IsAvailableToAnonymous = applicationOperation.IsAvailableToAnonymous,
                Name = applicationOperation.Name,
                ParentControllerName = applicationOperation.ParentControllerName,
                Type = applicationOperation.Type
            };
            foreach(var permission in applicationOperation.Permissions)
            {
                var permissionDTO = new ApplicationPermissionDTO
                {
                    Description = permission.Description,
                    Id = permission.Id,
                    IsCustom = permission.IsCustom,
                    Name = permission.Name
                };
                applicationOperationDTO.Permissions.Add(permissionDTO);
            }
            return Ok(new
            {
                value = applicationOperationDTO
            }
            );

        }

        [HttpPut("{id}")]
        public ActionResult PutApplicationOperation(ApplicationOperationDTO applicationOperationDTO, int? id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var applicationOperation = repo.GetById<ApplicationOperation>(id);
            if (applicationOperation == null)
            {
                return new NotFoundResult();
            }
            applicationOperation.IsAvailableToAllAuthorizedUsers = applicationOperationDTO.IsAvailableToAllAuthorizedUsers;
            applicationOperation.IsAvailableToAnonymous = applicationOperationDTO.IsAvailableToAnonymous;
            applicationOperation.ClearPermissions();
            foreach(var permissionDTO in applicationOperationDTO.Permissions)
            {
                var permission = repo.GetById<ApplicationPermission>(permissionDTO.Id);
                applicationOperation.Permissions.Add(permission);
            }
            manager.Session.Flush();
            return NoContent();
        }

        [HttpPost]
        public ActionResult<ApplicationOperationDTO> PostOperation(ApplicationOperationDTO applicationOperationDTO)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var applicationOperation = new ApplicationOperation();
            applicationOperation.IsAvailableToAllAuthorizedUsers = applicationOperationDTO.IsAvailableToAllAuthorizedUsers;
            applicationOperation.IsAvailableToAnonymous = applicationOperationDTO.IsAvailableToAnonymous;
            applicationOperation.Name = applicationOperationDTO.Name;
            applicationOperation.ParentControllerName = applicationOperationDTO.ParentControllerName;
            applicationOperation.Type = applicationOperationDTO.Type;
            foreach (var permissionDTO in applicationOperationDTO.Permissions)
            {
                var permission = repo.GetById<ApplicationPermission>(permissionDTO.Id);
                applicationOperation.Permissions.Add(permission);
            }
            repo.Save<ApplicationOperation>(applicationOperation);
            manager.Session.Flush();
            return CreatedAtAction("PostOperation", new { id = applicationOperation.Id }, applicationOperation);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteOperation(int id)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            var repo = new Repository(manager);
            var operation = repo.GetById<ApplicationOperation>(id);
            if (operation == null)
            {
                return NotFound();
            }
            repo.DeleteApplicationOperation(operation);
            return Ok();
        }
    }
}
