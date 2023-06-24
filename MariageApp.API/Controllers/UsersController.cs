using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MariageApp.API.Data;
using AutoMapper;
using MariageApp.API.Dtos;
using System.Security.Claims;
using MariageApp.API.Helpers;
using MariageApp.API.Models;

namespace MariageApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMariageRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IMariageRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }


        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId=currentUserId;
              if(string.IsNullOrEmpty(userParams.Gender)){
                userParams.Gender=userFromRepo.Gender=="رجل"?"إمرأة":"رجل";
            }


            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);

        }
        [HttpGet("{id}", Name = "GetUser")]

        public async Task<IActionResult> Getuser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailsDto>(user);
            return Ok(userToReturn);


        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo = await _repo.GetUser(id);
            _mapper.Map(userForUpdateDto, userFromRepo);
            if (await _repo.SaveAll())
                return NoContent();


            throw new Exception($"حدثت مشكلة في تعديل بيانات المشترك رقم {id}");


        }
              [HttpPost("{id}/like/{recipientId}")]
         public async Task<IActionResult> LikeUser(int id , int recipientId )
         {
              if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             return Unauthorized();
             var like = await _repo.GetLike(id,recipientId);
             if(like!=null)
             return BadRequest("لقد قمت بالإعجاب بهذا المشترك من قبل");
             if(await _repo.GetUser(recipientId)==null)
             return NotFound();
             like = new Like{
                 LikerId = id,
                 LikeeId = recipientId
             };
             _repo.Add<Like>(like);
             if(await _repo.SaveAll())
             return Ok();
             return BadRequest("فشل في الإعجاب");
         }
    }
}