using System.Text;
using AutoMapper;
using MariageApp.API.Data;
using MariageApp.API.Dtos;

namespace MariageApp.API.Helpers
{
    public class TemplateGenerator1
    {
        private readonly IMapper _mapper;
        private readonly IMariageRepository _repo;

        public TemplateGenerator1(IMariageRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }



        public string GetHTMLStringForUser1(int userId)
        {
			//exception of Global Query Filter we use false
            var user = _repo.GetUser(userId, false).Result;
            var userToReturn = _mapper.Map<UserForDetailsDto>(user);

            var likers = _repo.GetLikersOrLikees(userId, "likers").Result;
            var likees = _repo.GetLikersOrLikees(userId, "likees").Result;
            var likersCount=likers.Count;
            var likeesCount=likees.Count;


            var sb = new StringBuilder();

            sb.Append(@"
                        <html dir='ltr'>
                            <head>
                            </head>
                            <body>
                                <div class='page-header'><h2 class='header-container'>Carte De " + userToReturn.KnownAs + @"</h2></div>
                                                             
                                <div class='card-data'>
                                 <img src='" + userToReturn.PhotoURL + @"'>
                                <table style='display:inline;width: 50%;height: 300px;'>
                                <div>
                                <tr>
                                <td>Nom</td>
                                    <td>" + userToReturn.UserName + @"</td>
                                </tr>
                                <tr>
                                    <td>Age</td>
                                    <td>" + userToReturn.Age + @"</td>
                                </tr>    
                                <tr>
                                    <td>Pays</td>
                                    <td>" + userToReturn.Country + @"</td>
                                </tr>    
                                <tr>
                                    <td>Date D'inscription </td>
                                    <td>" + userToReturn.Created.ToShortDateString() + @"</td>
                                </tr> 
                                </div>   
                              </table>
                                </div>
                                <div class='page-header'><h2 class='header-container'>Aimants &nbsp;&nbsp;["+likersCount+@"]</h2></div>
                                <table align='center'>
                                    <tr>
                                        <th>Nom</th>
                                        <th> Date D'inscription</th>
                                        <th>Age</th>
                                        <th>Pays</th>
                                    </tr>");

            foreach (var liker in likers)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", liker.KnownAs, liker.Created.ToShortDateString(), liker.DateOfBirth.CalculateAge(), liker.Country);
            }

            sb.Append(@"
                                </table>
                                <div class='page-header'><h2 class='header-container'> Aimés  &nbsp;&nbsp;["+likeesCount+@"] </h2></div>
                                <table align='center'>
                                <tr>
                                 <th>Nom</th>
                                        <th> Date D'inscription</th>
                                        <th>Age</th>
                                        <th>Pays</th>
                                </tr>");
            foreach (var likee in likees)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", likee.KnownAs, likee.Created.ToShortDateString(), likee.DateOfBirth.CalculateAge(), likee.Country);
            }

            sb.Append(@"     </table>                   
                            </body>
                        </html>");

            return sb.ToString();
        }
		
    }
}