using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.DTOs
{
    public class ApplicationUserDTO
    {
        public string username { get; set; }
        //public string passwordHash;
        //public string securityStamp;
        public bool emailConfirmed { get; set; }
        public bool lockoutEnabled { get; set; }
        public bool phoneNumberConfirmed { get; set; }
        public bool twoFactorEnabled { get; set; }
        public int? accessFailedCount { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public DateTime? lockoutEndDate { get; set; }
        public byte[] versionTimestamp { get; set; }
        public string password { get; set; }
        public string passwordRepeat { get; set; }
        public List<ApplicationRoleDTO> roles { get; set; }
        public ApplicationUserDTO()
        {
            this.roles = new List<ApplicationRoleDTO>();
        }
}
}
