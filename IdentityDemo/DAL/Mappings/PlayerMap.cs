using IdentityDemo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace IdentityDemo.DAL.Mappings
{
    public class PlayerMap : ClassMap<Player>
    {
        public PlayerMap()
        {
            Id(x => x.Id);
            Map(x => x.DateOfBirth);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            References(x => x.Team);
        }
    }
}
