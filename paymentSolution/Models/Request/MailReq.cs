namespace paymentSolution.Models.Request
{
    public class MailReq
    {
        public string from { get; set; }
        public string to { get; set; }
        public string cc { get; set; }
        public string subject { get; set; }
        public string bcc { get; set; }
        public string mailMessage { get; set; } = $"<head>" +
                                $"<meta charset=\"utf-8\" /> <meta name=\"x-apple-disable-message-reformatting\" /> <meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\" /> " +
                                $"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" /> " +
                                $"<meta name=\"format-detection\" content=\"telephone=no, date=no, address=no, email=no\" /> " +
                                $"<title>AFF Registration Notification</title>" +
                                $" <link href=\"https://fonts.googleapis.com/css?family=Montserrat:ital,wght@0,200;0,300;0,400;0,500;0,600;0,700;0,800;0,900;1,200;1,300;1,400;1,500;1,600;1,700\" rel=\"stylesheet\" media=\"screen\" />" +
                                $"<style>.hover-underline:hover </style></head>" +
                                $"<body style=\" margin: 0; padding: 0; width: 100%; word-break: break-word; -webkit-font-smoothing: antialiased; --bg-opacity: 100; background-color: #ffffff; background-color: #ffffff; \">" +
                                $" <div style=\"display: none\"> Thank you for using BreezePay </div> <div role=\"article\" aria-roledescription=\"email\" aria-label=\"Thank you for using BreezePay uD83DuDC4B\" lang=\"en\">" +
                                $"<table style=\" font-family: Montserrat, -apple-system, ''Segoe UI'' , sans-serif; width: 100%; \" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"> <tr>" +
                                $"<td align=\"center\" style=\" --bg-opacity: 1; background-color: #ffffff; background-color: rgba( 236,239, 241, var(--bg-opacity) ); font-family: Montserrat, -apple-system, ''Segoe UI'' , sans-serif; \" bgcolor=\"rgba(236, 239, 241, var(--bg-opacity))\"> " +
                                $"<table class=\"sm-w-full\" style=\" font-family: ''Montserrat'' , Arial, sans-serif; width: 600px; \" width=\"600\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">" +
                                $"<tr><td class=\"sm-py-32 sm-px-24\" style=\" font-family: Montserrat, -apple-system, ''Segoe UI'', sans-serif; padding: 12px; text-align: center; \" align=\"center\"> <a href=\"https://africafintechfoundry.ng\"> " +
                                $"<img src=\"https://res.cloudinary.com/dvqls9yc9/image/upload/v1701436225/bq2dabyqe7vmcaby0fiu.png\" width=\"250\" alt=\"BreezePay\" style=\" border: 0; max-width: 100%;line-height: 100%; vertical-align: middle; \" /> " +
                                $"</a> </td></tr> <tr> <td align=\"center\" class=\"sm-px-24\" style=\" font-family: ''Montserrat'' , Arial,sans-serif; \">" +
                                $"<table style=\" font-family: ''Montserrat'' , Arial, sans-serif; width: 100%; \"width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><tr>" +
                                $"<td class=\"sm-px-24\" style=\" --bg-opacity: 1; background-color: #ffffff; background-color: rgba( 255, 255, 255, var(--bg-opacity) );" +
                                $" border-radius: 4px; font-family: Montserrat, -apple-system, ''Segoe UI'' , sans-serif; font-size: 14px; line-height: 24px; padding: 12px; text-align: left; --text-opacity: 1; color: #626262; " +
                                $"color: rgba( 98, 98, 98,var(--text-opacity) ); \" bgcolor=\"rgba(255, 255, 255, var(--bg-opacity))\"align=\"left\"> <p style=\" font-weight: 600; font-size: 18px; margin-bottom: 0; \"> Hello, </p>" +
                                
                                $"<p>You have successfully registered for the FDA program set to be span from January 1st to january 20th</p>\r\n " +
                                $"<p>A payment of the sum of N50,000 is required for you to be confirmed for this event, please see below registration link:</p>\r\n " +
                                $"<p>https://payment.africafintechfoundry.ng/payment</p>\r\n " +
                                $"<p></p>\r\n " +
                                $"<p>Thank you very much and God bless</p>\r\n " +

                                $"<p style=\"margin: 12px 0\">{DateTime.Now}</p> <p style=\"margin: 12px 0\">Cheers</p> <p style=\"margin: 12px 0\"> <strong>From AFF Team</strong> </p>" +
                                $"<div ><a href=\"https://africafintechfoundry.ng\"><img src=\"https://res.cloudinary.com/dvqls9yc9/image/upload/v1701436225/bq2dabyqe7vmcaby0fiu.png\"width=\"50\"" +
                                $" alt=\"BreezePay\" style=\" border: 0; max-width: 20%;line-height: 20%; vertical-align: right; \" /> </a></div><table style=\"font-family: ''Montserrat'' , Arial, sans-serif; width: 100%;\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\"role=\"presentation\">" +
                                $"<tr><td style=\" font-family: ''Montserrat'' , Arial, sans-serif;padding-top: 32px; padding-bottom: 32px; \"><div style=\" --bg-opacity: 1; background-color: #eceff1;background-color: rgba( 236, 239, 241, var( --bg-opacity )); height: 1px; line-height: 1px; \">" +
                                $" &zwnj; </div></td></tr> </table><p style=\"margin: 0 0 16px\"> If you have questions or suggestions, please call or chat us on" +
                                $"<a href=\"*\" class=\"hover-underline\"style=\" --text-opacity: 1; color: #1f376d; color: rgba( 115, 103,240, var( --text-opacity ) ); text-decoration: none; \"> " +
                                $"09168350086</a> or send an email to<a href=\"africafintechfoundry2017@gmail.com\" class=\"hover-underline\" style=\" --text-opacity: 1; color: #1f376d; color: rgba( 115, 103, 240, var( --text-opacity ) ); text-decoration: none; \"> " +
                                $"support@breezepay.io</a>. </p><p style=\"margin: 0 0 16px\"> Thank you for partnering with AFF, <br />The AFF Team </p></td></tr><tr><td style=\" font-family: ''Montserrat'' , Arial, sans-serif; height: 20px; \" height=\"20\">" +
                                $"</td></tr></table></td></tr></table> </td></tr></table></div></body> </html>";
        public string displayName { get; set; }
    }
}
