﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;
using TaskManagement_April_.Service;
using TaskManagement_April_.Service.Implementation;

namespace TaskManagement_April_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class TaskController : Controller
    {
        #region Variables
        private readonly ITaskService _taskService;
        private Response? obResponse;
        private TaskManagementContext _dbcontext;
        #endregion
        #region Constructor
        public TaskController(ITaskService taskService,TaskManagementContext dbcontext)
        {
            _taskService = taskService;
            _dbcontext = dbcontext;
        }
        #endregion

        #region Methods
        [HttpPost("SaveTask")]
        public async Task<IActionResult> SaveTask([FromBody] TaskL task)
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
                    var (request,msg,status,generatedcode) = await _taskService.SaveTask(task);
                    //if (request)
                    //{
                       
                            obResponse = new Response
                            {
                                Message = msg,
                                IsSuccess = request
                            };

                    return StatusCode(status,new { response = obResponse, code = generatedcode});
                    //}
                    //else
                    //{
                    //    obResponse = new Response
                    //    {
                    //        Message = "Task cannot be added",
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

        [HttpPost("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask([FromBody] TaskL task,int id)
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
                    var (request, msg, status) = await _taskService.UpdateTask(task,id);
                    obResponse = new Response
                    {
                        Message = msg,
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
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
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

                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
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
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
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
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
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
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
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

                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
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
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return BadRequest(obResponse);
            }
        }

        [HttpPost("UpdateTaskStatus")]
        public async Task<IActionResult> UpdateTaskStatus(int id)
        {
            try
            {
                var (result, msg) = await _taskService.UpdateTaskStatus(id);
                obResponse = new Response
                {
                    Message = msg,
                    IsSuccess = result
                };
                return Ok(obResponse);

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

        [HttpPost("UpdateTaskPriority")]
        public async Task<IActionResult> UpdateTaskPriority(int id)
        {
            try
            {
                var (result, msg) = await _taskService.UpdateTaskPriority(id);
                obResponse = new Response
                {
                    Message = msg,
                    IsSuccess = result
                };
                return Ok(obResponse);

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

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            { 
                var task= await _taskService.DelTask(id);
                if (task)
                {
                    obResponse = new Response
                    {
                        Message ="Task Deleted",
                        IsSuccess = true
                    };
                    return Ok(obResponse);
                }
                else
                {
                    obResponse = new Response
                    {
                        Message = "Not found",
                        IsSuccess = false
                    };
                    return BadRequest(obResponse);
                }

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
