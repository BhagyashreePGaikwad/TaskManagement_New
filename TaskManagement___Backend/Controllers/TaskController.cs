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
    public class TaskController : Controller
    {
        #region Variables
        private readonly ITaskService _taskService;
        private Response? obResponse;
        #endregion
        #region Constructor
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        #endregion

        #region Methods
        [HttpPost("SaveTask")]
        public async Task<IActionResult> SaveTask([FromBody] Tasks task)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (task.Name.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "Task cannot be empty",
                            IsSuccess = true
                        };

                        return Ok(obResponse);
                    }
                    if (task.Description.IsNullOrEmpty())
                    {
                        obResponse = new Response
                        {
                            Message = "Task Description cannot be empty",
                            IsSuccess = true
                        };

                        return Ok(obResponse);
                    }
                    var request = await _taskService.SaveTask(task);
                    if (request)
                    {
                       
                            obResponse = new Response
                            {
                                Message = "Task added successfully.",
                                IsSuccess = true
                            };
                      
                        return Ok(obResponse);
                    }
                    else
                    {
                        obResponse = new Response
                        {
                            Message = "Task cannot be added",
                            IsSuccess = false
                        };

                        return Ok(obResponse);
                    }
                }
                return BadRequest("Some properties are not valid.");

            }
            catch (Exception ex)
            {

                return BadRequest("Some properties are not valid.");
            }
        }

        [HttpPost("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask([FromBody] Tasks task,int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var request = await _taskService.UpdateTask(task,id);
                    if (request)
                    {
                        
                        obResponse = new Response
                        {
                            Message = "Task update successfully.",
                            IsSuccess = true
                        };
                        return Ok(obResponse);
                    }
                    else
                    {
                        obResponse = new Response
                        {
                            Message = "Task cannot be updated",
                            IsSuccess = false
                        };
                        return Ok(obResponse);
                    }
                }
                return BadRequest("Some properties are not valid.");

            }
            catch (Exception ex)
            {

                return BadRequest("Some properties are not valid.");
            }
        }
        [HttpGet("GetTaskByProject")]
        public async Task<IActionResult> TaskByProject(int projectId)
        {
            try
            {
                var result = await _taskService.GetAllTaskofProject(projectId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet("GetTaskBySubTask")]
        public async Task<IActionResult> TaskBySubtaskId(int subTaskid)
        {
            try
            {
                var result = await _taskService.GetAllTaskofSubTask(subTaskid);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet("GetTaskByAssignTo")]
        public async Task<IActionResult> TaskByAssignToId(int assignTo)
        {
            try
            {
                var result = await _taskService.GetTaskByAssignTo(assignTo);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("GetYourTaskByFilter")]
        public async Task<IActionResult> YourTaskByFilter(SearchSortTask model)
        {
            try
            {
                var result = await _taskService.GetYourTaskSortByDueDateorPriority(model);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("GetYourAssignedTaskByFilter")]
        public async Task<IActionResult> AssignedTaskByFilter(SearchSortTask model)
        {
            try
            {
                var result = await _taskService.GetYourTaskAssignedSortByDueDateorPriority(model);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("GetTaskByFilter")]
        public async Task<IActionResult> TaskByFilter(SearchSortTask1 model)
        {
            try
            {
                var result = await _taskService.GetTaskSortByDueDateorPriority(model);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost("SearchTask")]
        public async Task<IActionResult> SearchTask(SearchTasks model)
        {
            try
            {
                var result = await _taskService.SearchTask(model);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}
