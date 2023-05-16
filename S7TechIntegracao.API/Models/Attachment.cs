using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class Attachment
    {
        public int AbsoluteEntry { get; set; }
        public List<AttachmentsLine> Attachments2_Lines { get; set; }
    }
}