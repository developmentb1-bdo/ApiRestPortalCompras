using S7TechIntegracao.API.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class Emails : IEmails
    {
        public string Assunto { get; set; }
        public string CorpoEmail { get; set; }
        public List<string> Anexos { get; set; }

        public Emails()
        {

        }
    }

    public class EmailsAprovadores : Emails
    {
        public S7T_WDD1 Aprovador { get; set; }

        public EmailsAprovadores()
        {

        }
    }
}