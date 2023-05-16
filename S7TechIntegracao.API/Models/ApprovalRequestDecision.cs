using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class ApprovalRequestDecision
    {
        public string Status { get; set; }
        public string Remarks { get; set; } 
        public string ApproverUserName { get; set; }
        public string ApproverPassword { get; set; }

        public ApprovalRequestDecision()
        {

        }
    }
}