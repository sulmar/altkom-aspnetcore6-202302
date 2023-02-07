using Altkom.Shopper.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Altkom.Shopper.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> logger;

    public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
    {
        _userRepository = userRepository;
        this.logger = logger;
    }

    // GET api/users    
    [AllowAnonymous]
    [HttpGet]    
    public ActionResult<IEnumerable<User>> Get([FromServices] ICurrencyService currencyService)
    {
        if (!this.User.Identity.IsAuthenticated)
        {
            return Forbid();
        }

        // var roleClaims = this.User.Claims.Where(c => c.Type == ClaimTypes.Role);


        logger.LogInformation("Get all users");

        var users = _userRepository.GetAll();

        return Ok(users);
    }

    // GET api/users/{id}
    [HttpGet("/api/users/{id:int}")]
    public ActionResult<User> Get(int id)
    {
        // zła praktyka
        // logger.LogInformation($"Get user id = {id}");

        // dobra praktyka
        logger.LogInformation("Get user id = {id}", id);

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
