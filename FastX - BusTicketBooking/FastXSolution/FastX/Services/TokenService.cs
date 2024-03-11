using FastX.Interfaces;
using FastX.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FastX.Services
{
    public class TokenService : ITokenService
    {
        private readonly string _keyString;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            _keyString = configuration["SecretKey"].ToString();
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyString));
        }
        //public async Task<string> GenerateToken(LoginUserInputDTO user)
        //{
        //    string token = string.Empty;
        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.NameId,user.Username),
        //        new Claim(ClaimTypes.Role,user.Role)
        //    };
        //    //Algorithm Signature with secret key
        //    var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

        //    //Giving the token decription
        //    var tokenDescription = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(claims),
        //        Expires = DateTime.Now.AddDays(1),
        //        SigningCredentials = cred
        //    };

        //    //Putting the token together
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var myToken = tokenHandler.CreateToken(tokenDescription);
        //    token = tokenHandler.WriteToken(myToken);
        //    return token;
        //}
        public async Task<string> GenerateToken(string username, string role)
        {
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.NameId, username),
        new Claim(ClaimTypes.Role, role)
    };

            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            return tokenHandler.WriteToken(token);
        }
    }
}
