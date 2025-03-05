using Authentication.Data;
using Authentication.Entities;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly Jwt _jwt;
        private readonly AppIdentityContext _identityContext;

        public AuthenticationService(UserManager<AppUser> userManager, IOptions<Jwt> jwt, AppIdentityContext identityContext)
        {
            this.userManager = userManager;
            _jwt = jwt.Value;
            _identityContext = identityContext;
        }
        public async Task<string> CreateToken(AppUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            claims.AddRange(roles.Select(role => new Claim("role", role)));
            claims.AddRange(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            double expires = 1;
            Console.WriteLine($"{_jwt.ExpireOfDay}");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(expires),
                SigningCredentials = creds,
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);

        }

        public async Task<AuthenticationModel> Login(LoginModel model)
        {

            var user = await userManager.FindByEmailAsync(model.Email);

            var token = await CreateToken(user!);

            return new AuthenticationModel
            {
                IsAuthenticated = true,
                Token = token,
                Email = user.Email!,
                Username = user.UserName!,
                ExpiresOn = DateTime.UtcNow.AddDays(_jwt.ExpireOfDay),
                Roles = (await userManager.GetRolesAsync(user)).ToList()
            };
        }

        public async Task<AuthenticationModel> Registering(RegisterModel model)
        {
            using var transaction = _identityContext.Database.BeginTransaction();
            try
            {
                var user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var passwordValidator = new PasswordValidator<AppUser>();
                var validationResult = await passwordValidator.ValidateAsync(userManager, user, model.Password);

                if (!validationResult.Succeeded)
                {
                    var errors = validationResult.Errors.Select(e => e.Description);
                    return new AuthenticationModel { Message = string.Join(" ", errors) };
                }

                var result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return new AuthenticationModel { Message = string.Join(" ", errors) };
                }

                await userManager.AddToRoleAsync(user, "User");

                transaction.Commit();

                return new AuthenticationModel
                {
                    IsAuthenticated = true,
                    Token = await CreateToken(user),
                    Email = user.Email,
                    Username = user.UserName,
                    Roles = (await userManager.GetRolesAsync(user)).ToList(),
                    ExpiresOn = DateTime.UtcNow.AddDays(_jwt.ExpireOfDay)
                };
            }
            catch(Exception ex) 
            {
                transaction.Rollback();
                return new AuthenticationModel { Message=ex.Message };
            }
        }
    } 
}

