using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MariageApp.API.Data;
using AutoMapper;
using MariageApp.API.Dtos;
using System.Security.Claims;
using MariageApp.API.Helpers;

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
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);

        }
    [HttpGet("{id}",Name ="GetUser")]
       
        public async Task<IActionResult> Getuser(int id)
        {
            var user = await _repo.GetUser(id);
             var userToReturn = _mapper.Map<UserForDetailsDto>(user);
            return Ok(userToReturn);

           
        }
            [HttpPut("{id}")]
         public async Task<IActionResult> UpdateUser(int id,UserForUpdateDto userForUpdateDto){
             if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
             return Unauthorized();
             var userFromRepo = await _repo.GetUser(id);
             _mapper.Map(userForUpdateDto,userFromRepo);
             if(await _repo.SaveAll())
                 return NoContent();
             

             throw new Exception($"حدثت مشكلة في تعديل بيانات المشترك رقم {id}");
             
             
         }
    }
}