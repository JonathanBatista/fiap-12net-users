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
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public UsersController(IFaceService faceService, IMapper mapper, IUserService userService)
        {
            _faceService = faceService;
            _mapper = mapper;
            _userService = userService;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(UserProcess), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] UserFace request)
        {
            try
            {
                var user = await _faceService.DetectFaceAsync(request.Face);
                var response = _mapper.Map<UserProcess>(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }   
        }

        [HttpPost("foodRestrictions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] UserFoodRestriction request)
        {
           
            try
            {
                _userService.SaveUserRestriction(Guid.Parse(request.UserId), request.Restrictions, request.Other);
                return Ok();
            }
            catch
            {
                return StatusCode(400);
            }
            
        }

    }
}