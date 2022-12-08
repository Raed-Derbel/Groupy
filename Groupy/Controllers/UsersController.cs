using Microsoft.AspNetCore.Mvc;

namespace Groupy.Controllers;
using Groupy.Models;

[ApiController]
[Route("")] 
public class UsersController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    
    private static List<User> users = new List<User>();
    private static List<Team> teams = new List<Team>();
    private static List<UserRoleInTeam> userRolesTeams = new List<UserRoleInTeam>();
    private static List<Role> roles = new List<Role>();
    private static List<Tasks> tasks = new List<Tasks>();

    [HttpGet(Name = "users")]
    public List<User> Get()
    {
        return users;
    }

    [HttpPost]
    [Route("login")] 
    public User Login(User user)
    {
        if (users.Find(i => i.Username == user.Username) != null) {
            return user;
        } 
        return new User();
    }

    [HttpPost]
    [Route("register")] 
    public User Register(User user)
    {
        users.Add(user);
        return user;
    }

    [HttpGet]
    [Route("getUserTeams")] 
    public List<Team> GetUserTeams(long id)
    {
        List<UserRoleInTeam> results = userRolesTeams.FindAll(
        delegate(UserRoleInTeam item)
        {
            return item.UserId < id;
        }
        );
        List<Team> Response = new List<Team>();
        foreach (var item in results)
        {
            Response.Add(getTeam(item.TeamId));
        }
        return Response;
    }

     private Team getTeam(long id)
    {
        return teams.Find(i => i.Id == id);
        //return teams;
    }

   
}
