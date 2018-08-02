using GeekBurger.Users.Contract.Dtos.RequestDto;
using GeekBurger.Users.Contract.Dtos.ResponseDto;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.Users.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        [HttpPost]
        [ProducesResponseType(typeof(UserProcessDto), 201)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] UserRequestDto request)
        {
            return Ok();
        }

        [HttpPost("foodRestrictions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] UserFoodRestrictionRequestDto request)
        {
            return Ok();
        }

    }
}