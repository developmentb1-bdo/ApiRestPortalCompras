using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class RetornoPost<T, S>
    {
        public T error { get; set; }

        public S value { get; set; }
    }
}