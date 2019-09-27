using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.DTOs
{
    public class TeamDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Founded { get; set; }
        public string City { get; set; }
    }
}
