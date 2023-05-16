using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace S7TechIntegracao.API.Interfaces
{
    public interface IEmails
    {
        string Assunto { get; set; }
        string CorpoEmail { get; set; }
    }
}
