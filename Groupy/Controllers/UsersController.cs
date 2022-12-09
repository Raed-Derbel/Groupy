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

  

    [HttpGet]
    [Route("users")] 
    public List<User> Get()
    {
        return users;
    }

    [HttpGet]
    [Route("teams")] 
    public List<Team> getTeams()
    {
        return teams;
    }

    [HttpGet]
    [Route("userRolesTeams")] 
    public List<UserRoleInTeam> getuserRolesTeams()
    {
        return userRolesTeams;
    }

    [HttpGet]
    [Route("roles")] 
    public List<Role> getRoles()
    {
       initRoles();
        return roles;
    }

    [HttpGet]
    [Route("tasks")] 
    public List<Tasks> getTasks()
    {
        return tasks;
    }

    [HttpPost]
    [Route("login")] 
    public long Login(User user)
    {
        var found = users.Find(i => (i.Username == user.Username && i.Password == user.Password));
        if (found != null) {
            return (long)found.Id;
        } 
        return -1;
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
       initRoles();
        var found = roles.Find(i => i.Id == id);
        if (found != null) {
        return found;
        }
        return new Role(0, "No role");
    }
   
    [HttpPost]
    [Route("setUserRoleInTeam")] 
    public Boolean setUserRole(UserRoleInTeam userRoleTeam)
    {
        var found = userRolesTeams.Find(i => (i.UserId == userRoleTeam.UserId));
        if (found != null) {
            userRolesTeams.Remove(found);
        }
        userRolesTeams.Add(userRoleTeam);
        return true;
    }

    [HttpGet]
    [Route("getUserRoleInTeam")] 
    public Role getUserRole(long id, long idTeam)
    {
        UserRoleInTeam found = userRolesTeams.Find(i => (i.UserId == id && i.TeamId == idTeam));
        if (found != null) {
            return findRole(found.RoleId);
        }
        return new Role(-1, "No role found");
    }

    private Role findRole(long id)
    {
        initRoles();
        return roles.Find(i => i.Id == id);
    }

    private void initRoles()
    {
         roles.Clear();
        Role memberRole = new Role(0, "Member");
        roles.Add(memberRole);
        memberRole = new Role(1, "Super-Member");
        roles.Add(memberRole);
        memberRole = new Role(2, "Supervisor");
        roles.Add(memberRole);
    }

    [HttpPost]
    [Route("createTeam")] 
    public Boolean createTeam(Team team)
    {
        team.Id = teams.Count + 1;
        teams.Add(team);
        return true;
    }

    [HttpGet]
    [Route("getTeam")] 
    public Team getTeamPost(long id)
    {
        var found = teams.Find(i => i.Id == id);
        if (found != null) {
        return found;
        }
        return new Team();
    }

    [HttpPost]
    [Route("updateTeam")] 
    public Boolean updateTeam(Team team)
    {
        var found = teams.Find(i => i.Id == team.Id);
        if (found != null) {
            teams.Remove(found);
            teams.Add(team);
            return true;
        } 
        return false;
    }

    [HttpGet]
    [Route("getTeamMembers")]
    public List<User> getTeamMembers(long id)
    {
        List<UserRoleInTeam> results = userRolesTeams.FindAll(
        delegate(UserRoleInTeam item)
        {
            return item.TeamId == id;
        }
        );
        List<User> Response = new List<User>();
        foreach (var item in results)
        {
            Response.Add(users.Find(i => i.Id == id));
        }
        return Response;
    }

    [HttpGet]
    [Route("addTeamMember")]
    public Boolean addTeamMember(long idTeam, long idMember)
    {
        UserRoleInTeam tmp = new UserRoleInTeam();
        tmp.TeamId = idTeam;
        tmp.UserId = idMember;
        userRolesTeams.Add(tmp);
        return true;
    }

    [HttpGet]
    [Route("removeTeamMember")] 
    public Boolean removeTeamMember(long teamId, long userId)
    {
        var found = userRolesTeams.Find(i => (i.TeamId == teamId && i.UserId == userId) );
        if (found != null) {
            userRolesTeams.Remove(found);
            return true;
        } 
        return false;
    }

    [HttpPost]
    [Route("createTask")] 
    public Boolean createTask(Tasks task)
    {
        task.Id = tasks.Count + 1;
        tasks.Add(task);
        return true;
    }

    [HttpPost]
    [Route("updateTask")] 
    public Boolean updateTask(Tasks task)
    {
        var found = tasks.Find(i => i.Id == task.Id);
        if (found != null) {
            tasks.Remove(found);
            tasks.Add(task);
            return true;
        } 
        return false;
    }

    [HttpGet]
    [Route("getTeamTasks")]
    public List<Tasks> getTeamTasks(long id)
    {
        List<Tasks> Response = new List<Tasks>();
        foreach (var item in tasks)
        {
           if (item.TeamId == id) {
            Response.Add(item);
           }
        }
        return Response;
    }

}
