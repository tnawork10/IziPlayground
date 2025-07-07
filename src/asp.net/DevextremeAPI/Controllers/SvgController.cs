using IziHardGames.SVG;
using Microsoft.AspNetCore.Mvc;
using F = System.IO.File;

namespace DevextremeAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SvgController(ILogger<SvgController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetImage()
        {
            var xml = await F.ReadAllTextAsync(@"C:\Users\adyco\Downloads\test.svg");
            var result = SvgSplitter.Split(xml);
            var first = result.First();
            var stream = new MemoryStream();
            first.Save(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, System.Net.Mime.MediaTypeNames.Image.Svg);
        }
    }
}