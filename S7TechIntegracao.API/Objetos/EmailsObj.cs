using RestSharp;
using S7TechIntegracao.API.Interfaces;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class EmailsObj
    {
        private static readonly EmailsObj _instancia = new EmailsObj();

        public static EmailsObj GetInstance()
        {
            return _instancia;
        }

        public void EnviarEmailAprovador(EmailsAprovadores infoEmail)
        {
            try
            {
                var aprovador = EmployeesInfoObj.GetInstance().Consultar(infoEmail.Aprovador.U_empID);
                var emailTo = new List<string> { aprovador.eMail };

                var email = (Emails)infoEmail;

                EnviarEmail(email, emailTo);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[EmailsObj] [EnviarEmailAprovador] {ex.Message}");

                throw ex;
            }
        }

        public void EnviarEmail(Emails infoEmail, List<string> emailTo)
        {
            try
            {
                DefaultSettingsModel defaultSettings = DefaultSettingsModel.GetInstance();

                var address = defaultSettings.Address;
                var host = defaultSettings.Host;
                var userName = defaultSettings.UserName; 
                var password = defaultSettings.Password;
                var enableSsl = defaultSettings.EnableSsl;
                var port = defaultSettings.Port;

                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress(address);

                    //destinatarios do e-mail, para incluir mais de um basta separar por ponto e virgula 
                    emailTo.ForEach(x => {
                        mailMessage.To.Add(x);
                    });

                    var _subject = infoEmail.Assunto; //$"Aprovação de solicitação - {esboco.DocumentLines.FirstOrDefault().CostingCode2}";

                    mailMessage.Subject = _subject;
                    mailMessage.IsBodyHtml = true;

                    //conteudo do corpo do e-mail 
                    mailMessage.Body = infoEmail.CorpoEmail;

                    mailMessage.Priority = MailPriority.High;

                    //anexos
                    if (infoEmail.Anexos != null) {
                        infoEmail.Anexos.ForEach(att => { mailMessage.Attachments.Add(new System.Net.Mail.Attachment(att)); });
                    }

                    //smtp do e-mail que irá enviar 
                    SmtpClient smtpClient = new SmtpClient(host);
                    smtpClient.EnableSsl = enableSsl;
                    smtpClient.Port = port;

                    //credenciais da conta que utilizará para enviar o e-mail 
                    smtpClient.Credentials = new NetworkCredential(userName, password);
                    smtpClient.Send(mailMessage);
                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                Log4Net.Log.Error($"[EmailsObj] [EnviarEmail] {ex.Message}");

                throw ex;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[EmailsObj] [EnviarEmail] {ex.Message}");

                throw ex;
            }

        }

        public string GetLayoutEmail(int draftEntry, BoObjectTypes objectTypes)
        {
            try
            {
                Documents docEsboco = null;
                string layoutName = string.Empty;

                switch (objectTypes)
                {
                    case BoObjectTypes.oPurchaseOrders:
                        {
                            docEsboco = PurchaseOrdersObj.GetInstance().Consultar(draftEntry);
                            layoutName = DefaultSettingsModel.GetInstance().PODefMailLayout;
                        }
                        break;
                    case BoObjectTypes.oPurchaseRequest:
                        {
                            docEsboco = PurchaseRequestsObj.GetInstance().Consultar(draftEntry);
                            layoutName = DefaultSettingsModel.GetInstance().RQDefMailLayout;
                        }
                        break;
                }

                var employee = EmployeesInfoObj.GetInstance().Consultar(docEsboco.DocumentsOwner.Value);
                var transform = Path.Combine(HttpRuntime.AppDomainAppPath, layoutName);

                string[] requesterName = { "RequesterName", "", $"{employee.EmployeeID} - {employee.FirstName} {employee.LastName}" };
                string[] requesterEmail = { "RequesterEmail", "", $"{employee.eMail}" };
                string[] paymentTerms = { "PaymentTermsGroupName", "", "" };
                string[] approvalLink = { "ApprovalLink", "", $"{ParamsModel.GetInstance().Url}AprovarSolicitacao?draftEntry={docEsboco.DocEntry}" };
                string[] repprovalLink = { "RepprovalLink", "", $"{ParamsModel.GetInstance().Url}ReprovarSolicitacao?draftEntry={docEsboco.DocEntry}" };

                List<string[]> parameters = new List<string[]>
                {
                    requesterName,
                    requesterEmail,
                    paymentTerms,
                    approvalLink,
                    repprovalLink
                };

                var stringTransformedXml = docEsboco.GetAsXml().Transform(transform, parameters);

                return stringTransformedXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}