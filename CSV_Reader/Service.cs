using GenHTTP.Modules.Webservices;
using GenHTTP.Api.Protocol;
using GenHTTP.Api.Content.Authentication;
using GenHTTP.Modules.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSV_Reader.DAL;
using Lib;
using System.Reflection.Metadata;

namespace CSV_Reader
{
    public class Service
    {
        private readonly string connectionString;
        CsvProgramTestContext context;
        Adder<Account, CsvProgramTestContext> adder;


        public Service()
        {
            connectionString = GetConnectionStringFromSettings();
            context = new("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CSV_Program_Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            adder = new Adder<Account, CsvProgramTestContext>(connectionString);
        }



        [ResourceMethod(RequestMethod.PUT, "AddFile")]
        public ValueTask<Task> AddFile(bool deleteExisting, Stream input, IRequest request)
        {

            adder.AddStreamToDb(input);

            

            return new ValueTask<Task>();
        }

        [ResourceMethod("GetAll")]
        public List<Account> GetAll()
        {
            return context.Accounts.ToList();
        }

        [ResourceMethod("GetById")]
        public Account GetAccountById(int id) 
        {
            return context.Accounts.Where(a => a.CostumerNumber == id).FirstOrDefault();
        }

        private string GetConnectionStringFromSettings()
        {
            string[] connectionString = new string[2] {"", ""};
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
            }
            return connectionString[1];
        }
    }
}
