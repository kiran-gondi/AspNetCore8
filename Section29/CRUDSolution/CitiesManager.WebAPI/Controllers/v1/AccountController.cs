using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CitiesManager.WebAPI.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AccountController : CustomControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
            _roleManager = roleManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerDTO"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
        {
            //Validation
            if(ModelState.IsValid == false)
            {
                string? errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v=>v.Errors).Select(i=>i.ErrorMessage));
                return Problem(errorMessage);
            }

            //Create User
            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                //Sign-in
                await _signInManager.SignInAsync(user, isPersistent: false);
                AuthenticationResponse authenticationResponse = await UpdateRefreshTokenDetails(user);

                return Ok(authenticationResponse);
            }
            else
            {
                string errorMessage = string.Join(" | ", result.Errors.Select(x=>x.Description)); //error1 | error2 ...etc
                return Problem(errorMessage);
            }
        }

        private async Task<AuthenticationResponse> UpdateRefreshTokenDetails(ApplicationUser user)
        {
            var authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);
            return authenticationResponse;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null) {
                return Ok(true);
            }
            return Ok(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin(LoginDTO loginDTO)
        {
            //Validation
            if (ModelState.IsValid == false)
            {
                string? errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(i => i.ErrorMessage));
                return Problem(errorMessage);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user == null)
                {
                    return NoContent();
                }

                //Sign-in
                await _signInManager.SignInAsync(user, isPersistent: false);

                AuthenticationResponse authenticationResponse = await UpdateRefreshTokenDetails(user);

                return Ok(authenticationResponse);
            }
            else
            {
                return Problem("Invalid email or password");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        { 
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpPost("refresh-jwt-token")]
        public async Task<IActionResult> RefreshJwtToken(JwtTokenModel jwtTokenModel)
        {
            if (jwtTokenModel == null) { 
                return BadRequest("Invalid client request");
            }

            string? jwtToken = jwtTokenModel.Token;
            string? refreshToken = jwtTokenModel.RefreshToken;

           ClaimsPrincipal? claimsPrincipal = _jwtService.GetPrincipalFromJwtToken(jwtToken);
            if (claimsPrincipal == null) {
                return BadRequest("Invalid jwt access token");
            }

            string? email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.RefreshToken != jwtTokenModel.RefreshToken ||
                user.RefreshTokenExpirationDateTime <= DateTime.Now) {
                return BadRequest("Invalid refresh token");
            }

            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);

            return Ok(authenticationResponse);

        }

    }
}
