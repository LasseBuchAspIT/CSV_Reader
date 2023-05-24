using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class SettingsReader
    {
        //checks parsed location for a connectionString, returns only the connectionString itself
        public static string GetConnectionString(string location)
        {
            string[] connectionString = new string[2] { "", "" };
            try
            {
                using (StreamReader streamReader = new(location))
                {
                    string settings = streamReader.ReadToEnd();
                    string[] args = settings.Split(',');

                    //split into before and after =, ConnectiongString should be in second part
                    if (args.Any(a => a.ToUpper().Contains("CONNECTIONSTRING")))
                    {
                        connectionString = args.Where(a => a.ToUpper().Contains("CONNECTIONSTRING")).FirstOrDefault().Split('=', 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Settings file could not be read\n{ex.Message}");
                throw new ArgumentNullException(ex.Message);
            }
            return connectionString[1];
        }

        //gets port from parsed location
        //same code as connectionstring but for port
        public static ushort GetPort(string location)
        {
            //standard port
            ushort port = 8080;

            string[] words = new string[2];

            using (StreamReader streamReader = new(location))
            {
                string settings = streamReader.ReadToEnd();
                string[] args = settings.Split(',');

                if (args.Any(a => a.ToUpper().Contains("PORT")))
                {
                    words = args.Where(a => a.ToUpper().Contains("PORT")).FirstOrDefault().Split('=', 2);
                    if (ushort.TryParse(words[1], out ushort parsed))
                    {
                        port = parsed;
                    }
                    else
                    {
                        Console.WriteLine("invalid port... port set to 8080");
                    }
                }
                return port;
            }
        }
    }
}
