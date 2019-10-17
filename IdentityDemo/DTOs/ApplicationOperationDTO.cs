using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.DTOs
{
    public class ApplicationOperationDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string ParentControllerName { get; set; }
        public string Type { get; set; }
        public bool IsAvailableToAnonymous { get; set; }
        public bool IsAvailableToAllAuthorizedUsers { get; set; }
        public List<ApplicationPermissionDTO> Permissions { get; set; }

        public ApplicationOperationDTO()
        {
            this.Permissions = new List<ApplicationPermissionDTO>();
        }
    }
}
