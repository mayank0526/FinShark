using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> UM;
        private readonly ITokenService TS;
        private readonly SignInManager<AppUser> SM;
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            UM = userManager;
            TS = tokenService;
            SM = signInManager;
   
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login (LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            return BadRequest(ModelState);

            var user = await UM.Users.FirstOrDefaultAsync(x=> x.UserName == loginDto.UserName.ToLower());

            if (user == null) 
            return Unauthorized("Invalid Username");

            var result = await SM.CheckPasswordSignInAsync(user, loginDto.Password,false);
            
            if(!result.Succeeded)
            return Unauthorized ("Username or Password coud not be found");

            return Ok(
                new NewUserDto 
                {
                UserName = user.UserName,
                Email = user.Email,
                Token = TS.CreateToken(user)

                }
            );

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                {
                    var appUser = new AppUser
                    {
                        UserName = registerDto.Username,
                        Email = registerDto.Email
                    };

                    var createUser = await UM.CreateAsync(appUser, registerDto.Password);

                    if (createUser.Succeeded)
                    {
                        var roleResult = await UM.AddToRoleAsync(appUser, "User");
                        if (roleResult.Succeeded)
                        {
                            return Ok(
                                new NewUserDto {
                                    UserName = appUser.UserName,
                                    Email = appUser.Email,
                                    Token = TS.CreateToken(appUser)
                                }
                            );
                        }
                        else
                        {
                            return StatusCode(500, roleResult.Errors);
                        }
                    }

                    else
                    {
                        return StatusCode(500, createUser.Errors);
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

    }
}