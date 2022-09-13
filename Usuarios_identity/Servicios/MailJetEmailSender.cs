using Mailjet;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace Usuarios_identity.Servicios
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        public OpcionesMailJet _opcionesMailJet;


        public MailJetEmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _opcionesMailJet = configuration.GetSection("MailJet").Get<OpcionesMailJet>();

            MailjetClient client = new MailjetClient(_opcionesMailJet.ApiKey, _opcionesMailJet.SecretKey)
            {
                Version = ApiVersion.V3_1
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
             new JObject {
              {
               "From",
               new JObject {
                {"Email", "user_pruebas@protonmail.com"},
                {"Name", "Users Identity"}
               }
              }, {
               "To",
               new JArray {
                new JObject {
                 {
                  "Email",
                  email
                 }, {
                  "Name",
                  "Users Identity"
                 }
                }
               }
              }, {
               "Subject",
               subject
              }, {
               "HTMLPart",
               htmlMessage
              }, {
               "CustomID",
               "AppGettingStartedTest"
              }
             }
                     });
            await client.PostAsync(request);

        }
    }
}


