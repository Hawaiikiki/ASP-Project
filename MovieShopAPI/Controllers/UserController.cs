using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // need to be authorized
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("Purchases")]
        public async Task<IActionResult> GetMoviesPurchasedByUser()
        {
            // need to get the userId from the Token, using HttpContext
            return Ok();
        }
    }
}
