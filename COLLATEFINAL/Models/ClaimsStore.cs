using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace COLLATEFINAL.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            new Claim("Administrator","Administrator"),
            new Claim("Events","Events"),
            new Claim("Software Projects","Software Projects"),
            new Claim("Instructional Materials","Instructional Materials"),
            new Claim("Research Papers","Research Papers"),
        };

        public static List<Claim> AdminClaim = new List<Claim>()
        {
            new Claim("Administrator","Administrator"),
        };


        public static List<Claim> EventsClaim = new List<Claim>()
        {
            new Claim("Events","Events"),
        };

        public static List<Claim> SoftwareClaim = new List<Claim>()
        {
            new Claim("Software Projects","Software Projects"),
        };

        public static List<Claim> InstrucClaim = new List<Claim>()
        {
            new Claim("Instructional Materials","Instructional Materials"),
        };
        public static List<Claim> ResearchClaim = new List<Claim>()
        {
            new Claim("Research Papers","Research Papers"),
        };






    }
}
