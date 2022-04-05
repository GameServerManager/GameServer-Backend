using GameServer.API.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml;

namespace GameServer.API.Services
{
    public class UserService : IUserService
    {
        private IDatabaseService _databaseService;
        private byte[] _key;
        private string _maudiKey;

        public UserService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            var doc = new XmlDocument();
            doc.Load("secret.xml");
            _key = Encoding.ASCII.GetBytes(doc.DocumentElement?.SelectSingleNode("/secret/key")?.InnerText);
            _maudiKey = doc.DocumentElement?.SelectSingleNode("/secret/maudiKey")?.InnerText;
        }

        public async Task<string> Authenticate(string username, string password)
        {   
            if (string.IsNullOrEmpty(username) | string.IsNullOrEmpty(password))
                return null;

            var login = await _databaseService.GetUser(username);
            if (login == null || login.Hash == null || !PasswordHash.VerifyPassword(password, login.Hash, login.Salt))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, login.Username.ToString()),
                    new Claim(ClaimTypes.Role, login.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); ;
        }

        public async Task<bool> Register(string username, string password, string maudiSecret)
        {
            if (string.IsNullOrEmpty(username) | string.IsNullOrEmpty(password))
                return false;

            var salt = Guid.NewGuid().ToString();

            var expectedMaudiSecret = PasswordHash.HashPassword(username, _maudiKey);
            string expectedMaudiSecretString = Convert.ToBase64String(expectedMaudiSecret);

            if (expectedMaudiSecretString != maudiSecret)
            {
                return false;
            }

            await _databaseService.SaveNewUser(new Models.User() { 
                Username = username, 
                Hash = PasswordHash.HashPassword(password, salt), 
                Role = "User", 
                Salt = salt
            });

            return true;
        }
    }
}
