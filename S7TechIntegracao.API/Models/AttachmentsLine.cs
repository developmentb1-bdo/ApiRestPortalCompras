using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class AttachmentsLine
    {
        public string SourcePath { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string AttachmentDate { get; set; }
        public string UserID { get; set; }
        public string Override { get; set; }
        public string FreeText { get; set; }
    }
}