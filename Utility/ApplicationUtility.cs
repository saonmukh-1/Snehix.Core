using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Utility
{
    public class ApplicationUtility
    {
        public static string GetTokenAttribute(string rawtoken,string type)
        {            
            var value = rawtoken.Remove(0, 7);
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadJwtToken(value);
            var usernameClaim = token.Claims.Where(a => a.Type == type).FirstOrDefault();
            return usernameClaim.Value;
        }
    }
}
