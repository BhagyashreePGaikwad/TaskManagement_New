using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TaskManagement_April_.Model;
using TaskManagement_April_.Service;

namespace TaskManagement_April_.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : Controller
    {
        #region Variables
        private readonly IProjectService _projectService;
        private Response? obResponse;
        #endregion
        #region Constructor
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        #endregion
        #region Methods
        [HttpPost("SaveProject")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SaveProject([FromBody] ProjectF model)
        {
            try
            {
              //  var userId = claims.FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
                if (ModelState.IsValid)
                {
                    if(model.ProjectName.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "Project name is required.",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                    if (model.Description.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "Description is required.",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                    var (request, msg, status, generatedcode) = await _projectService.SaveProject(model);
                       

                            obResponse = new Response
                            {
                                Message = msg,
                                IsSuccess = request
                            };

                    return StatusCode(status, new { response = obResponse, code = generatedcode });


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
        [HttpPost("UpdateProject/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateProject([FromBody] Project model,int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.ProjectName.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "Project name is required.",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                    if (model.Description.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "Description is required.",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                       var (request,msg,status) = await _projectService.UpdateProject(model, id);

                            obResponse = new Response
                            {
                                Message =msg,
                                IsSuccess = request
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

        [HttpGet("ProjectList")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetProject()
        {
            try
            {
                var result = await _projectService.GetAllProject();
                return Ok(result);

            }
            catch (Exception ex) {

                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);

            }
        }

        [HttpGet("GetProjectById")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var result = await _projectService.GetProjectById(id);
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

        [HttpDelete("DeleteProject")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteProjectById(int id)
        {
            try
            {
                var value = await _projectService.DelProject(id);
                var result = await _projectService.GetAllProject();
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


        [HttpPost("SearchProject")]
        public async Task<IActionResult> SearchProject(SearchProject model)
        {
            try
            {
                var value = await _projectService.SearchProject(model);
        
                    return Ok(value);
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
