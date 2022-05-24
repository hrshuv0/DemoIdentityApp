using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace DemoIdentityApp.API.Services.EmailService;

public class MailJetEmailSender : IEmailSender
{
    private readonly MailJetOptions _mailJetOptions;

    public MailJetEmailSender(IConfiguration configuration)
    {
        _mailJetOptions = configuration.GetSection("MailJet").Get<MailJetOptions>();
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailjetClient client = new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey) {
            Version = ApiVersion.V3_1,
        };
        MailjetRequest request = new MailjetRequest {
                Resource = Send.Resource,
            }
            .Property(Send.Messages, new JArray {
                new JObject {
                    {
                        "From",
                        new JObject {
                            {"Email", "shahedislamshovon@protonmail.com"}, 
                            {"Name", "TeleDoc BD"}
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
                                    "Shahed Islam"
                                }
                            }
                        }
                    }, {
                        "Subject",
                        subject
                    }, {
                        "TextPart",
                        "My first Mailjet email"
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

