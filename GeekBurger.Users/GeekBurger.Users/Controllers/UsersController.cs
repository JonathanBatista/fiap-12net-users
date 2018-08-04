using GeekBurger.Users.Contract;
using GeekBurger.Users.Data;
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
            UserRepository repo = new UserRepository();
            var guid = repo.InsertFace(request.Face);
            return Ok(new { Guid = guid });
        }

        [HttpPost("foodRestrictions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] UserFoodRestriction request)
        {
            UserRepository repo = new UserRepository();

            var restriction = new Core.Domains.UserRestriction();
            restriction.UserId = request.UserId;
            restriction.Ingredient = request.Restrictions;
            try
            {
                repo.InserFoodRestriction(restriction);
                return Ok();
            }
            catch
            {
                return StatusCode(400);
            }
            
        }

    }
}