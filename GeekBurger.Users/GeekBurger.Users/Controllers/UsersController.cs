using AutoMapper;
using GeekBurger.Users.Application.AzureServices;
using GeekBurger.Users.Contract;
using GeekBurger.Users.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GeekBurger.Users.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        private readonly IFaceService _faceService;

        public UsersController(IFaceService faceService)
        {
            _faceService = faceService;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(UserProcess), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] UserFace request)
        {
            try
            {
                var user = await _faceService.DetectFaceAsync(request.Face);

                var response = Mapper.Map<UserProcess>(user);

                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }   
        }

        [HttpPost("foodRestrictions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] UserFoodRestriction request)
        {
            UserRepository repo = new UserRepository();

            var restriction = new Core.Domains.UserRestriction();
            restriction.User.AzureGuid = request.UserId;
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