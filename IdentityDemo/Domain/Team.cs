using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Domain
{
    public class Team
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Founded { get; set; }
        public virtual string City { get; set; }
        public virtual IList<Player> Players { get; protected set; }

        public Team()
        {
            Players = new List<Player>();
        }
        public virtual void AddPlayer(Player player)
        {
            player.Team = this;
            Players.Add(player);
        }
    }
}
