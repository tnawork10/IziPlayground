using Microsoft.AspNetCore.Mvc;

namespace FileUploading.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UploadController : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> UploadFoormsCollection(IFormCollection forms)
        {
            var keys = forms.Keys;
            await Task.CompletedTask;
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UploadFilesCollection(IFormFileCollection files)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}
