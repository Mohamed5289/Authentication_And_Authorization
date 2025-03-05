using Authentication.Entities;
using Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace Authentication.Services
{
    public class ValidationService : IValidationService
    {
        private readonly UserManager<AppUser> userManager;
        public ValidationService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<AuthenticationModel> LoginValidity(LoginModel model, ModelStateDictionary modelState)
        {
            if (model is null)
                return new AuthenticationModel { Message = "Model is null" };

            if (!modelState.IsValid)
            {
                var errors = modelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return new AuthenticationModel { Message = "Validation failed", Errors = errors };
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                return new AuthenticationModel
                {
                    Message = "Email or password is incorrect !",
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Error", new List<string> { "Email or password is incorrect !" } }
                    }
                };
            }

            return new AuthenticationModel { IsAuthenticated = true };
        }

        public async Task<AuthenticationModel> RegistrationValidity(RegisterModel model, ModelStateDictionary modelState)
        {
            if (model is null)
                return new AuthenticationModel { Message = "Model is null" };

            if (!modelState.IsValid)
            {
                var errors = modelState.ToDictionary(
                   kvp => kvp.Key,
                   kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToList()
                );

                return new AuthenticationModel { Message = "Validation failed", Errors = errors };
            }

            if (await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthenticationModel
                {
                    Message = "Email is already registered!",
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Email", new List<string> { "Email is already registered!" } }
                    }
                };

            if (await userManager.FindByNameAsync(model.Username) is not null)
                return new AuthenticationModel
                {
                    Message = "Username is already registered!",
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Username", new List<string> { "Username is already registered!" } }
                    }
                };

            return new AuthenticationModel { IsAuthenticated = true };
        }
    }
}
