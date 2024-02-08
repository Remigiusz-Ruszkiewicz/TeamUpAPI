using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TeamUpAPI.Helpers
{
    public static class TokenHelper
    {
        //public static string? GetUserIdFromToken(string token)
        //{
        //    new JwtSecurityTokenHandler().ValidateToken(token, new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Key"])),
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        ClockSkew = TimeSpan.Zero
        //    }, out SecurityToken validatedToken);

        //    return ((JwtSecurityToken)token).Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        //}
    }
}
