using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MariageApp.API.Data;
using AutoMapper;
using MariageApp.API.Dtos;
using System.Security.Claims;
using MariageApp.API.Helpers;
using MariageApp.API.Models;
using Microsoft.Extensions.Options;
using Stripe;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace MariageApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]

    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMariageRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<StripeSettings> _stripeSettings;
        private readonly IConverter _converter;
        public UsersController(IMariageRepository repo, IMapper mapper, IOptions<StripeSettings> stripeSettings, IConverter converter)
        {
            _converter = converter;
            _stripeSettings = stripeSettings;
            _mapper = mapper;
            _repo = repo;

        }


        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId, true);
            userParams.UserId = currentUserId;
            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "رجل" ? "إمرأة" : "رجل";
            }


            var users = await _repo.GetUsers(userParams);
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);

        }
        [HttpGet("{id}", Name = "GetUser")]

        public async Task<IActionResult> Getuser(int id)
        {
            var isCurrentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == id;

            var user = await _repo.GetUser(id, isCurrentUser);
            var userToReturn = _mapper.Map<UserForDetailsDto>(user);
            return Ok(userToReturn);


        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var userFromRepo = await _repo.GetUser(id, true);
            _mapper.Map(userForUpdateDto, userFromRepo);
            if (await _repo.SaveAll())
                return NoContent();


            throw new Exception($"حدثت مشكلة في تعديل بيانات المشترك رقم {id}");


        }
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var like = await _repo.GetLike(id, recipientId);
            if (like != null)
                return BadRequest("لقد قمت بالإعجاب بهذا المشترك من قبل");
            if (await _repo.GetUser(recipientId, false) == null)
                return NotFound();
            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };
            _repo.Add<Like>(like);
            if (await _repo.SaveAll())
                return Ok();
            return BadRequest("فشل في الإعجاب");
        }

        [HttpPost("{userId}/charge/{stripeToken}")]
        public async Task<IActionResult> Charge(int userId, string stripeToken)

        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var customers = new CustomerService();
            var charges = new ChargeService();

            // var options = new TokenCreateOptions
            // {
            // Card = new CreditCardOptions
            //     {
            //         // Number = "4242424242424242",
            //         // ExpYear = 2020,
            //         // ExpMonth = 3,
            //         // Cvc = "123"
            //     }
            // };

            // var service = new TokenService();
            // Token stripeToken = service.Create(options);

            var customer = customers.Create(new CustomerCreateOptions
            {
                Source = stripeToken
            });

            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = 5000,
                Description = "إشتراك مدى الحياة",
                Currency = "usd",
                Customer = customer.Id
            });

            var payment = new Payment
            {
                PaymentDate = DateTime.Now,
                Amount = charge.Amount / 100,
                UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value),
                ReceiptUrl = charge.ReceiptUrl,
                Description = charge.Description,
                Currency = charge.Currency,
                IsPaid = charge.Paid
            };
            _repo.Add<Payment>(payment);
            if (await _repo.SaveAll())
            {
                return Ok(new { IsPaid = charge.Paid });
            }

            return BadRequest("فشل في السداد");

        }
        [HttpGet("{userId}/payment")]
        public async Task<IActionResult> GetPaymentForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var payment = await _repo.GetPaymentForUser(userId);
            return Ok(payment);
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("UserReport/{userId}")]
        public IActionResult CreatePdfForUser(int userId)
        {
            var templateGenerator = new TemplateGenerator(_repo, _mapper);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 15, Bottom = 20 },
                DocumentTitle = "بطاقة مشترك"

            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateGenerator.GetHTMLStringForUser(userId),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Impact", FontSize = 12, Spacing = 5, Line = false },
                FooterSettings = { FontName = "Geneva", FontSize = 15, Spacing = 7, Line = true, Center = "MariageApp par Adnane El ghazi", Right = "[page]" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);
            return File(file, "application/pdf");
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("UserReport1/{userId}")]
        public IActionResult CreatePdfForUser1(int userId)
        {
            var templateGenerator1 = new TemplateGenerator1(_repo, _mapper);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 15, Bottom = 20 },
                DocumentTitle = "بطاقة مشترك"

            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = templateGenerator1.GetHTMLStringForUser1(userId),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Impact", FontSize = 12, Spacing = 5, Line = false },
                FooterSettings = { FontName = "Geneva", FontSize = 15, Spacing = 7, Line = true, Center = "MariageApp par Adnane El ghazi", Left = "[page]" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);
            return File(file, "application/pdf");
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetAllUsersExceptAdmin")]
        public async Task<IActionResult> GetAllUsersExceptAdmin()
        {
            var users = await _repo.GetAllUsersExceptAdmin();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }



    }
}