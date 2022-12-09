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

    static void Main()
    {
    Role memberRole = new Role(0, "Member");
    roles.Add(memberRole);
    }

    [HttpGet(Name = "users")]
    public List<User> Get()
    {
        return users;
    }

    [HttpGet]
    [Route("roles")] 
    public List<Role> getRoles()
    {
        return roles;
    }

    [HttpPost]
    [Route("login")] 
    public Boolean Login(User user)
    {
        if (users.Find(i => (i.Username == user.Username && i.Password == user.Password )) != null) {
            return true;
        } 
        return false;
    }

    [HttpPost]
    [Route("register")] 
    public Boolean Register(User user)
    {
        user.Id = users.Count + 1;
        users.Add(user);
        return true;
    }

    [HttpGet]
    [Route("deleteUser")] 
    public Boolean deleteUser(long id)
    {
        var found = users.Find(i => i.Id == id);
        if (found != null) {
            users.Remove(found);
            return true;
        } 
        return false;
    }

    [HttpPost]
    [Route("updateUser")] 
    public Boolean updateUser(User user)
    {
        var found = users.Find(i => i.Id == user.Id);
        if (found != null) {
            users.Remove(found);
            users.Add(user);
            return true;
        } 
        return false;
    }

    [HttpGet]
    [Route("getUserTeams")]
    public List<Team> GetUserTeams(long id)
    {
        List<UserRoleInTeam> results = userRolesTeams.FindAll(
        delegate(UserRoleInTeam item)
        {
            return item.UserId == id;
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

    [HttpGet]
    [Route("getRole")] 
    public Role getRole(long id)
    {
        var found = roles.Find(i => i.Id == id);
        if (found != null) {
        return found;
        }
        return new Role(0, "No role");
    }
   
}
