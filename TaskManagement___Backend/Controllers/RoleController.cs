using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement_April_.Model;
using TaskManagement_April_.Service;

namespace TaskManagement_April_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : Controller
    {
        #region Variables

        private readonly IRoleService _roleService;
        private Response? obResponse;
        #endregion
        #region Constructor
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        #endregion
        #region Methods
        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole()
        {
            try
            {
                var result = await _roleService.GetRole();
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
        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetRoleById(int id)
        {

            try
            {
                var result = await _roleService.GetRoleNameById(id);
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
