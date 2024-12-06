using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using DAL.Interfaces;

namespace Backend
{
    public class SessionVariables 
    {
       public const string SessionKeyUsername = "SessionKeyUsername";
       public const string SessionKeySessionId = "SessionKeySessionId";
    }

    public enum SessionKeyEnum
    {
        SessionKeyUsername = 0,
        SessionKeySessionId = 1
    }
}
