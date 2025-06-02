using Microsoft.AspNetCore.Identity;
using PhotoContests.Entities;
using PhotoContests.Models;
using System.Text.RegularExpressions;

namespace PhotoContests.Manager
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ITokenManager tokenManager;
        public AuthenticationManager(UserManager<User> userManager, SignInManager<User> signInManager, ITokenManager tokenManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenManager = tokenManager;
        }

        public async Task<IList<string>> Role(LogeInUserModel model)
        {
            var user = await userManager.FindByEmailAsync(model.email);
            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);
                return role;
            }
            return null;
        }

        public async Task<TokenModel> SignIn(LogeInUserModel model)
        {
            var user = await userManager.FindByEmailAsync(model.email);
            if (user != null) 
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, model.password, false);
                if (result.Succeeded)
                {
                    var token = await tokenManager.CreateToken(user);
                    return new TokenModel
                    {
                        token = token
                    };
                }
            }
            return null;
        }

        public async Task Register(RegisterUserModel model)
        {

            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase);
            if (emailRegex.IsMatch(model.email) == true)
            {

                if (model.idRole == "PhotographerUser")
                {
                    var user = new Photographer 
                    {
                        Email = model.email,
                        UserName = model.email,
                        firstName = model.firstName,
                        lastName = model.lastName,
                        profilePicture = model.profilePicture,
                        biography = model.biography,
                        dateOfBirth = (DateTime)model.dateOfBirth,
                        idNationality = (int)model.idNationality,
                        newsSubscription = model.newsSubscription //new
                    };

                    var result = await userManager.CreateAsync(user, model.password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, model.idRole);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else if (model.idRole == "JurorUser")
                {
                    var user = new Juror
                    {
                        Email = model.email,
                        UserName = model.email,
                        firstName = model.firstName,
                        lastName = model.lastName,
                        profilePicture = model.profilePicture,
                        biography = model.biography,
                        yearsOfExperience = (int)model.yearsOfExperience,
                        newsSubscription = model.newsSubscription //new
                    };

                    var result = await userManager.CreateAsync(user, model.password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, model.idRole);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else if (model.idRole == "AdminUser")
                {
                    var user = new User 
                    {
                        Email = model.email,
                        UserName = model.email,
                        firstName = model.firstName,
                        lastName = model.lastName,
                        profilePicture = model.profilePicture,
                        biography = model.biography,
                        newsSubscription = true //new
                    };

                    var result = await userManager.CreateAsync(user, model.password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, model.idRole);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

            }
            else
            {
                throw new Exception();
            }
        }
    }
}
