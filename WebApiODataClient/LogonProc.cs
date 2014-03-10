using System;

using RF.Common.Security;

namespace WebApiODataClient
{
    public class LogonProc : ILogonPage
    {
        public LogonCreds GetLogin(Exception showReason)
        {
            Console.WriteLine("Process default login");
            var login = new LogonCreds();
            Console.WriteLine("Enter Login:");
            login.Name = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            login.Psw = Console.ReadLine();
            
            login.IsSuccessful = true;
            login.IsCanceled = false;
            return login;
        }
    }
}
