using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public static class SettingsReader
    {
        public static string GetConnectionString()
        {
            string[] connectionString = new string[2] { "", "" };
            try
            {
                using (StreamReader streamReader = new("Settings.txt"))
                {
                    string settings = streamReader.ReadToEnd();
                    string[] args = settings.Split(',');

                    if (args.Any(a => a.Contains("ConnectionString")))
                    {
                        connectionString = args.Where(a => a.Contains("ConnectionString")).FirstOrDefault().Split('=', 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Settings file could not be read\n{ex.Message}");
                return "ConnectionString not found";
            }
            return connectionString[1];
        }

        public static ushort GetPort()
        {
            ushort port = 8080;

            string[] words = new string[2];

            using (StreamReader streamReader = new("Settings.txt"))
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
