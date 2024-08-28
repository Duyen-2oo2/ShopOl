using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShopOnline.Model;
using ShopOnline.Models;
using ShopOnline.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static ShopOnline.Model.TokenModels;

namespace ShopOnline.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ShopContext _context;
        private readonly IConfiguration _configuration;

        public UserController(ShopContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        


        [HttpPost("Login")]
        public async Task<IActionResult> Validate(string user, string pass)
        {


            var acc = _context.Account
                              .SingleOrDefault(a => a.UserName == user && a.Password == pass);
            if (acc == null) //không đúng
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password"
                });
            }

            //cấp token
            var token = await GenerateToken(acc);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = token
            });
        }

        private async Task<TokenModel> GenerateToken(Account acc)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["AppSetting:SecretKey"]);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, acc.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", acc.UserName),
                    new Claim("Id", acc.AccountId.ToString()),

                    //roles
                }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //Lưu database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = acc.AccountId,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };

            await _context.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["AppSetting:SecretKey"]);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _context.Update(storedToken);
                await _context.SaveChangesAsync();

                //create new token
                var acc = await _context.Account.SingleOrDefaultAsync(nd => nd.AccountId == storedToken.UserId);
                var token = await GenerateToken(acc);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }
    }
}
