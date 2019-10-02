using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityDemo.DTOs;
using Microsoft.AspNetCore.Mvc;
using zAppDev.DotNet.Framework.Utilities;
using zAppDev.DotNet.Framework.Identity;
using zAppDev.DotNet.Framework.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        public ServiceLocator ServiceLocator { get; }

        public SignInController(IServiceProvider serviceProvider)
        {
            ServiceLocator = new ServiceLocator(serviceProvider);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        public IActionResult SignIn(ApplicationUserDTO userdto)
        {
            var manager = ServiceLocator.Current.GetInstance<IMiniSessionService>();
            manager.OpenSession();

            ActionResult _result = new EmptyResult();
            var success = IdentityHelper.SignIn(userdto.username, userdto.password, false);
            
            if(success == false)
            {
                return _result;
            }
            else
            {
                var applicationUser = IdentityHelper.GetApplicationUserByName(userdto.username);
                Claim claim = null;
                if (applicationUser.Claims.Any())
                {
                    var appUserClaim = applicationUser.Claims[0];
                    claim = new Claim(appUserClaim.ClaimType, appUserClaim.ClaimValue);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = EncodingUtilities.StringToByteArray("vMzWOLmHnkNYBLfruoqBvMzWOLmHnkNYBLfruoqBvMzWOLmHnkNYBLfruoqBvMzWOLmHnkNYBLfruoqBvMzWOLmHnkNYBLfruoqB", "ascii");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] { claim }),
                    Expires = DateTime.UtcNow.AddHours(2),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var result = new
                {
                    idToken = token,
                    expiresIn = 120
                };
                
                manager.CloseSession();
                
                return Ok(result);
            }
        }
    }
}
