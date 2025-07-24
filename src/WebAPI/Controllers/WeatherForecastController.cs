using System.Net.Mime;
using Common.Diagrams;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> VsdxToSvg(IFormFile file)
        {
            var converter = new VsdxToSvgConverter();
            using var s = file.OpenReadStream();
            var bytes = converter.GetSvgs(s);
            var result = bytes[0];
            return File(result, MediaTypeNames.Image.Svg);
        }
    }
}
