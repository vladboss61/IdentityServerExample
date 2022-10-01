using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace WebApplication1.Testing.Features.Services
{
    public sealed class User
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class UserPasswordValidator : IResourceOwnerPasswordValidator
    {
        //Your, can be hardcoded or read from database users.
        private readonly User[] _users =
        {
            new() { UserName = "admin.admin@google.com", Password = "admin_1" },
            new() { UserName = "user.user@google.com", Password = "user_1" }
        };

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var username = context.UserName;
            var password = context.Password;

            //byte[] hash = SHA256.Create().ComputeHash(Encoding.Unicode.GetBytes(password)); // for the future.
            if (_users.Any(x => x.UserName == username && x.Password == password))
            {
                context.Result = new GrantValidationResult(username, "pwd");
                return Task.CompletedTask;
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "Invalid Credentials");
            return Task.CompletedTask;
        }
    }
}
