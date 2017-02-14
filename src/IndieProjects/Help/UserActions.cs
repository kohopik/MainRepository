using System;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using IndieProjects.Model;

namespace IndieProjects.Help
{
    public static class UserActions
    {
        public static void SendMessage(User Sender, User Getter, IndieContext context)
        {
           // context.Messages.Add(new Message() {Author = Sender, Getter = Getter.Id,Content });
        }
    }
}
