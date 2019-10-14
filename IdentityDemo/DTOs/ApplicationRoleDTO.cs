using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.DTOs
{
    public class ApplicationRoleDTO
    {
        public int? Id {get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCustom { get; set; }
        public List<ApplicationPermissionDTO> Permissions { get; set; }

        public ApplicationRoleDTO()
        {
            this.Permissions = new List<ApplicationPermissionDTO>();
        }
    }
}
