using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement_April_.Model;
using TaskManagement_April_.Service;
using TaskManagement_April_.Service.Implementation;

namespace TaskManagement_April_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : Controller
    {
        #region Variable
        private readonly ICommentService _commentService;
        private Response? obResponse;
        #endregion
        #region Contrustor
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        #endregion
        #region Method
        [HttpPost("SaveComment")]
        public async Task<IActionResult> SaveComment([FromBody] Comment comment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var (value,msg) = await _commentService.SaveComment(comment);
                    //if (value)
                    //{

                    obResponse = new Response
                    {
                        Message = msg,
                        IsSuccess = value
                    };

                    if (value)
                    {
                        return Ok(obResponse);
                    }
                    else
                    {
                        return BadRequest(obResponse);
                    }
                    //}
                    //else
                    //{
                    //    obResponse = new Response
                    //    {
                    //        Message = "Comment cannot be added",
                    //        IsSuccess = false
                    //    };
                    //    return Ok(obResponse);
                    //}

                }
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

        [HttpGet("GetCommentforTask")]
        public async Task<IActionResult> GetCommentByTaskId(int taskId)
        {
            try
            {
                if (taskId != 0)
                {
                    var value = await _commentService.GetCommentsForTask(taskId);
                    return Ok(value);
                }
                return BadRequest();
            }catch (Exception ex)
            {

                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return Ok(obResponse);
            }

        }

        [HttpPost("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody] Comment comment, int id)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var (result,msg) = await _commentService.UpdateComment(comment, id);
                    //if (result)
                    //{

                    obResponse = new Response
                    {
                        Message = msg,
                        IsSuccess = result
                    };
                       // return Ok(obResponse);
                    if (result)
                    {
                        return Ok(obResponse);
                    }
                    else
                    {
                        return BadRequest(obResponse);
                    }
                    //}
                    //else
                    //{
                    //    obResponse = new Response
                    //    {
                    //        Message = "Comment cannot be updated.",
                    //        IsSuccess = false
                    //    };
                    //    return Ok(obResponse);

                    // }
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

        [HttpDelete("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var value = await _commentService.DelComment(id);
                if (value)
                {

                    obResponse = new Response
                    {
                        Message = "Comment deleted successfully.",
                        IsSuccess = true
                    };
                    return Ok(obResponse);
                }
                else
                {
                    obResponse = new Response
                    {
                        Message = "Comment cannot be delete.",
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
