using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class Project
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Active { get; set; }
        public string U_S7Regiao { get; set; }
        public string U_S7Tipo { get; set; }
        public string U_S7ClasseSocial { get; set; }
        public string U_S7LinhaNegocio { get; set; }
        public string U_S7Segmento { get; set; }

        public Project()
        {

        }
    }
}