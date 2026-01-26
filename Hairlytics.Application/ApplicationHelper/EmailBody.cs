using Hairlytics.Application.DTOs.HelperDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Application.ApplicationHelper
{
    public class EmailBody
    {
        public static string EmailStringBody(string email)
        {

                return $@"
                <!DOCTYPE html>
                <html>
                <body>
                    <p>Hello,</p>
                    <p>{email}</p>
                    <p>Thanks,<br/>Hairlytics Team</p>
                </body>
                </html>";            
        }
    }
}
