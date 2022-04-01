using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml;

namespace GameServer.API.Helper
{
    public class CustomJwtTokenValidator : ISecurityTokenValidator
    {

        public bool CanValidateToken => true;

        private int _maximumTokenSizeInBytes = int.MaxValue;
        private byte[] _key;

        public CustomJwtTokenValidator()
        {
            var doc = new XmlDocument();
            doc.Load("secret.xml");
            _key = Encoding.ASCII.GetBytes(doc.DocumentElement?.SelectSingleNode("/secret/key")?.InnerText);

        }

        public int MaximumTokenSizeInBytes { get => _maximumTokenSizeInBytes; set => _maximumTokenSizeInBytes = value; }

        public bool CanReadToken(string securityToken)
        {
            return CanValidateToken;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            ClaimsPrincipal claim;
            if (securityToken == null)
                return new ClaimsPrincipal();

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                claim = tokenHandler.ValidateToken(securityToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken _validatedToken);
                validatedToken = _validatedToken;

                // return user id from JWT token if validation successful
                return claim;
            }
            catch
            {
                // return null if validation fails
                return null;
            }

        }
    }
}
