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

            return $@"<html>

            <head></head>
            <body>
                <h1>hello world this message form Hairlytics {email} </h1>
            </body
                
            </html


            ";

        }
    }
}
