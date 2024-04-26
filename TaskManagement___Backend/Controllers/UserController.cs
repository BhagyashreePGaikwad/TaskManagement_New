using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement_April_.AuthAttribute;
using TaskManagement_April_.Model;
using TaskManagement_April_.Service;
using TaskManagement_April_.Service.Implementation;

namespace TaskManagement_April_.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        #region Variables
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private IConfiguration _config;
        private Response? obResponse;
        #endregion
        #region Constructor
        public UserController(IUserService userService, IConfiguration config,IRoleService roleService)
        {
            _userService = userService;
            _config = config;
            _roleService = roleService;
        }
        #endregion

        #region Methods
        [HttpGet("GetUsers")]
        [CustomAuthorize("Admin", "Manager")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
            }
        }

        [HttpGet("GetUserById")]
        [CustomAuthorize("Admin", "Manager")]
       // [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                if (id != null)
                {
                    var result = await _userService.GetUserById(id);
                    return Ok(result);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {

                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            try
            {
                if (!model.Email.IsNullOrEmpty() && !model.Password.IsNullOrEmpty())
                {
                    var result = await _userService.CheckValidUser(model);
                    var value = await _userService.LoginUser(model);
                    if (value!=null && result)
                    {

                        
                        var claims = new List<Claim>
                        {

                            new Claim(ClaimTypes.Name, value.Name),
                            new Claim(ClaimTypes.NameIdentifier, value.Id.ToString()),
                            new Claim(ClaimTypes.NameIdentifier,value.ToString())

                    };

                        var roleName = await _roleService.GetRoleNameById(value.RoleId);
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            claims.Add(new Claim(ClaimTypes.Role, roleName));
                        }

                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                          _config["Jwt:Issuer"],
                          claims,
                          expires: DateTime.Now.AddDays(1),
                          signingCredentials: credentials);
                        var userId=claims.FirstOrDefault(s=>s.Type==ClaimTypes.NameIdentifier)?.Value;
                        var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);
                        
                        return Ok(new { Token = token, UserId = userId });
                    }
                    else if(result && value==null)
                    {
                        obResponse = new Response
                        {
                            Message ="Please check Password",
                            IsSuccess = false
                        };
                    }
                    else if(!result && value==null)
                    {
                        obResponse = new Response
                        {
                            Message = "Please check Email",
                            IsSuccess = false
                        };
                    }
                    return BadRequest(obResponse);
                }
                else if (model.Email.IsNullOrEmpty())
                {
                    obResponse = new Response
                    {
                        Message = "Please enter Email",
                        IsSuccess = false
                    };
                    return Ok(obResponse);
                }
                else if (!model.Password.IsNullOrEmpty())
                {
                    obResponse = new Response
                    {
                        Message = "Please enter Password",
                        IsSuccess = false
                    };

                    return Ok(obResponse);

                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
            }
        }

        [HttpPost("SaveUser")]
        // [Authorize(Roles = "Admin")]
        [CustomAuthorize("Admin")]
       // [CustomAdminAuthorize]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (user.Name.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "User Name is required.",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                    if (user.Email.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "Email is required.",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                    if (user.Password.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "Password is required.",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                    var (result,message,status)= await _userService.SaveUser(user);

                            obResponse = new Response
                            {
                                Message =message,
                                IsSuccess = result
                            };
                     return StatusCode(status,obResponse);
                    
                }
                return BadRequest("Some properties are not valid.");

            }
            catch (Exception ex)
            {
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
            }
        }

        [HttpPost("UpdateUser/{id}")]
        //[Authorize(Roles = "Admin")]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> UpdateUser([FromBody] User user,int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (result,msg,status) = await _userService.UpdateUser(user,id);
                        
                        obResponse = new Response
                        {
                            Message = msg,
                            IsSuccess = result
                        };
                    return StatusCode(status, obResponse);
                }
                return BadRequest("Some properties are not valid.");

            }
            catch (Exception ex)
            {

                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
            }
        }

        [HttpDelete("DeleteUser")]
        // [Authorize(Roles = "Admin")]
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var value = await _userService.DelUser(id);
                var result = await _userService.GetUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
            }
        }
        #endregion
    }
}
