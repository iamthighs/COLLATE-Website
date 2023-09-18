using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COLLATEFINAL.ViewModels
{
    public class RoleClaimsViewModel
    {
        public RoleClaimsViewModel()
        {
            Cliams = new List<RoleClaim>();
        }

        public string RoleId { get; set; }
        public List<RoleClaim> Cliams { get; set; }
    }
}
