using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement_April_.Model;
using TaskManagement_April_.Service;
using System.Linq;


namespace TaskManagement_April_.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
   
    public class DocumentsController : Controller
    {
        #region Variables
        private readonly IDocumentService _documentService;
        private Response? obResponse;
        const string Document = "Document";
        #endregion
        #region Constructor
        public DocumentsController(IDocumentService documentService) 
        {
            _documentService = documentService;
        }
        #endregion
        #region Get/Post
        [HttpPost("UploadDoc")]
        public async Task<IActionResult> UploadAsync(IFormCollection formdata,int userId)
        {
            try
            {
                
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        if (!Directory.Exists(Document))
                        {

                            Directory.CreateDirectory(Document);
                        }

                        string subDirPath = Path.Combine(Document, "Document_" + userId.ToString());

                        if (!Directory.Exists(subDirPath))
                        {
                            Directory.CreateDirectory(subDirPath);
                        }

                        string FileName = file.FileName;
                        string ext = Path.GetExtension(FileName);
                        var newFilePath = Path.Combine(subDirPath,FileName);

                        FileStream fileStream;
                        using (fileStream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        fileStream.Close();
                        var (result,msg,status) = await _documentService.SaveDocument(userId, newFilePath, FileName);
                

                        //if (result !=true)
                        //{
                            obResponse = new Response
                            {
                                Message = msg,
                                IsSuccess = result
                            };
                        return StatusCode(status, obResponse);


                    }
                }
                return Ok();
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

        [HttpGet("GetDocumentByUserId")]
        public async Task<ActionResult> GetDocumentById(int id)
        {
            try
            {
                var result = await _documentService.GetDocumentByUserId(id);
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
        [HttpDelete("DeleteDocument")]
        public async Task<ActionResult> DeleteDoc(int docId)
        {
            try
            {
                var result = await _documentService.DeleteDocument(docId);
                if (result)
                {
                    obResponse = new Response
                    {
                        Message = "Document Deleted",
                        IsSuccess = true
                    };
                    return Ok(obResponse);
                }
                else
                {
                    obResponse = new Response
                    {
                        Message = "Cannot delete Document",
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

        [HttpGet("DownloadDocumentById")]
        public async Task<ActionResult> Download(int id)
        {
            try
            {
                var documents = await _documentService.GetDocument(id);
                if (documents != null)
                {
                    var testDoc = documents.FirstOrDefault();
                    var ff = testDoc.FilePath;
                    string fileName = testDoc.FileName;
                    if (ff != null)
                    {
                        var path = Path.Combine(
                            Directory.GetCurrentDirectory(), ff);

                        var memory = new MemoryStream();
                        using (var stream = new FileStream(path, FileMode.Open))
                        {
                            await stream.CopyToAsync(memory);
                        }

                        memory.Position = 0;
                        string fileExtension = Path.GetExtension(ff);
                        string contentType;
                         contentType = "application/pdf";
                        switch (fileExtension.ToLower())
                        {
                            case ".pdf":
                                contentType = "application/pdf";
                                break;
                            case ".xlsx":
                                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                break;
                            case ".csv":
                                contentType = "text/csv";
                                break;
                            case ".txt":
                                contentType = "text/plain";
                                break;
                            //default:
                            //    contentType = "application/octet-stream"; // Default to octet-stream for unknown types
                            //    break;
                        }
                        return PhysicalFile(path, contentType, fileName);

                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound);
                    }

                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }

            }
            catch (Exception ex)
            {
                obResponse = new Response
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return Ok(obResponse);
            }
        }

        #endregion
    }
}