using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class RetornoListaGenerica<T>
    {
        public T value { get; set; }
    }
}