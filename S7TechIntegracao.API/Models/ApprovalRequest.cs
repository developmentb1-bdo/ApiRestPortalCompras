using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class ApprovalRequest
    {
        public string Status { get; set; }
        public List<ApprovalRequestDecision> ApprovalRequestDecisions { get; set; }
        public ApprovalRequest()
        {
            ApprovalRequestDecisions = new List<ApprovalRequestDecision>();
        }
    }
}