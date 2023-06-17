using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MariageApp.API.Data;
using AutoMapper;
using MariageApp.API.Dtos;

namespace MariageApp.API.Controllers
{
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Getuser(int id)
        {
            var user = await _repo.GetUser(id);
             var userToReturn = _mapper.Map<UserForDetailsDto>(user);
            return Ok(userToReturn);

           
        }
    }
}