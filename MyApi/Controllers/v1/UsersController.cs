using Common.Exceptions;
using Data.Repositories;
using ElmahCore;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApi.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Entities.Roles;

namespace MyApi.Controllers.v1
{
    [AllowAnonymous]
    [ApiVersion("1")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UsersController> logger;
        private readonly IJwtService jwtService;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger, IJwtService jwtService,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            //var userName = HttpContext.User.Identity.GetUserName();
            //userName = HttpContext.User.Identity.Name;
            //var userId = HttpContext.User.Identity.GetUserId();
            //var userIdInt = HttpContext.User.Identity.GetUserId<int>();
            //var phone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            //var role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);

            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public virtual async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user2 = await userManager.FindByIdAsync(id.ToString());
            var role = await roleManager.FindByNameAsync("Admin");

            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();

            await userManager.UpdateSecurityStampAsync(user);
            //await userRepository.UpdateSecurityStampAsync(user, cancellationToken);

            return user;
        }

        /// <summary>
        /// This method generate JWT Token
        /// </summary>
        /// <param name="tokenRequest">The information of token request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public virtual async Task<ActionResult> Token([FromForm] TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            if (!tokenRequest.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                throw new Exception("OAuth flow is not password.");

            //var user = await userRepository.GetByUserAndPass(username, password, cancellationToken);
            var user = await userManager.FindByNameAsync(tokenRequest.username);
            if (user == null)
                throw new BadRequestException("There is no user.");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!isPasswordValid)
                throw new BadRequestException("Password is uncorrect.");


            var jwt = await jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            //await HttpContext.RaiseError(new Exception("Create Failed"));

            var exists = await userRepository.TableNoTracking.AnyAsync(p => p.UserName == userDto.UserName);
            if (exists)
                return BadRequest("There is already user.");

            var user = mapper.Map<User>(userDto);

            await userManager.CreateAsync(user, userDto.Password);
            await userManager.AddToRoleAsync(user, UserRoles.Admin);

            //await userRepository.AddAsync(user, userDto.Password, cancellationToken);
            return user;
        }

        [HttpPut]
        public virtual async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;

            await userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        public virtual async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }

    }
}
