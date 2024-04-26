using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagement_April_.Model;
using TaskManagement_April_.Service;
using TaskManagement_April_.Service.Implementation;

namespace TaskManagement_April_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubTaskController : Controller
    {
        #region Variables
        private readonly ISubTaskService _subTaskService;
        private Response? obResponse;
        #endregion
        #region Constructor
        public SubTaskController(ISubTaskService subTaskService)
        {
            _subTaskService = subTaskService;
        }
        #endregion

        #region Methods
        [HttpPost("SaveSubTask")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> SaveSubTask([FromBody] SubTask model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if(model.SubTaskName.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "SubTask Name is required.",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                       var (request,msg,status) = await _subTaskService.SaveSubTask(model);
                        //if (request)
                        //{

                            obResponse = new Response
                            {
                                Message =msg,
                                IsSuccess = request
                            };
                    return StatusCode(status, obResponse);
                    //}
                    //else
                    //{
                    //    obResponse = new Response
                    //    {
                    //        Message = "SubTask cammot be added.",
                    //        IsSuccess = false
                    //    };
                    //    return Ok(obResponse);
                    //}


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

        [HttpPost("UpdateSubTask/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateSubTask([FromBody] SubTask model,int id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.SubTaskName.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "SubTask name is required.",
                            IsSuccess = true
                        };
                        return Ok(obResponse);
                    }
                    var (request, msg, status) = await _subTaskService.UpdateSubTask(model,id);
                    //if (request)
                    //{

                        obResponse = new Response
                        {
                            Message = msg,
                            IsSuccess = request
                        };
                
                   return StatusCode(status, obResponse);     //}
                    //else
                    //{
                    //    obResponse = new Response
                    //    {
                    //        Message = "SubTask cannot be updted.",
                    //        IsSuccess = false
                    //    };
                    //    return Ok(obResponse);
                    //}

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


        [HttpGet("GetSubTaskByProjectId")]
        public async Task<IActionResult> GetSubTaskByProjectId(int projectId)
        {
            try
            {
                var result = await _subTaskService.GetAllSubTaskOfProject(projectId);
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

        [HttpPost("SearchSubtask")]
        public async Task<IActionResult> SearchSubtask(SearchSubTask model)
        {
            try
            {
                var result = await _subTaskService.SearchSubtask(model);
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

        [HttpGet("GetSubTaskBySubtaskId")]
        public async Task<IActionResult> GetSubTaskById(int subtaskId)
        {
            try
            {
                var result = await _subTaskService.GetSubTaskbyId(subtaskId);
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

        [HttpDelete("DeleteSubTask")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteSubTask(int id)
        {
            try
            {
                var value = await _subTaskService.DelSubTask(id);
                if (value)
                {
                    obResponse = new Response
                    {
                        Message = "SubTask deleted successfully.",
                        IsSuccess = true
                    };
                }
                else
                {
                    obResponse = new Response
                    {
                        Message = "SubTask cannot be deleted.",
                        IsSuccess = false
                    };
                }

                return BadRequest(obResponse);
                
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
