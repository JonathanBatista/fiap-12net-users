using GeekBurger.Users.Contract;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Users.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        [HttpPost]
        [ProducesResponseType(typeof(UserProcess), 201)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] User request)
        {
            return Ok();
        }

        [HttpPost("foodRestrictions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] UserFoodRestriction request)
        {
            return Ok();
        }

    }
}