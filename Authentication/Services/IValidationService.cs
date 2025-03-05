using Authentication.Entities;
using Authentication.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Authentication.Services
{
    public interface IValidationService
    {
        Task<AuthenticationModel> RegistrationValidity(RegisterModel model, ModelStateDictionary modelState);
        Task<AuthenticationModel> LoginValidity(LoginModel model, ModelStateDictionary modelState);
    }
}
