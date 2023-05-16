using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models 
{
    public class Login
    {
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        //public int Language { get; set; }

        public Login() 
        {

        }

        public Login(string companyDB, string userName, string password) 
        {
            CompanyDB = companyDB;
            UserName = userName;
            Password = password;
            //Language = 19;
        }
    }
}