using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AspNetCore.Http
{
    [AllowAnonymous]
    public class TestHeaders  : Controller
    {
        [HttpGet]
        public async Task<ActionResult> GetTest()
        {
            if (!Request.Headers.TryGetValue(HeaderNames.ContentRange, out var contentRangeHeader))
            {
                return BadRequest("Missing Content-Range header");
            }
            var hcr = new ContentRangeHeaderValue(3, 144, 9999);
            hcr.Unit = "Items";
            Response.Headers[HeaderNames.ContentRange] = hcr.ToString();
            Response.Headers["X-SOME-XD"] = "AAAA";
            Response.Headers[key: "Access-Control-Expose-Headers"] = $"X-SOME-XD, {HeaderNames.ContentRange}";
            var p = ContentRangeHeaderValue.Parse(Request.Headers.ContentRange.First());
            return Ok(new { p.Unit.Value, p.From, p.To });
        }
    }
}
