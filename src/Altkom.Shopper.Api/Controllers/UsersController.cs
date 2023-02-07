using Altkom.Shopper.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Altkom.Shopper.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET api/users    
    [HttpGet]    
    public IEnumerable<User> Get([FromServices] ICurrencyService currencyService)
    {
        return _userRepository.GetAll();
    }

    // GET api/users/{id}
    [HttpGet("/api/users/{id:int}")]
    public ActionResult<User> Get(int id)
    {
        var user = _userRepository.GetById(id); 

        if (user == null)
        {
            // return new NotFoundResult(); // Results.NotFound() // NotFound()

             return NotFound();
        }

        // return new OkObjectResult(user); // Results.Ok(user) // Ok()

        return Ok(user);
    }

    // GET api/users/{email}
    [HttpGet("/api/users/{email}")]
    public User Get(string email)
    {
        return new User();
    }

    // GET api/users?Lng=44.55&Lat=32.32
    //[HttpGet("/api/users")]
    //public User FindNear([FromQuery] Location location)
    //{
    //    throw new NotImplementedException();
    //}


    public class Location
    {
        public double Lng { get; set; }
        public double Lat { get; set; }
    }


}
