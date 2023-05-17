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

namespace CSV_Reader
{
    public class Service
    {
        CsvProgramTestContext context = new();
        Reader<Account> reader = new();
        [ResourceMethod(RequestMethod.PUT)]
        public ValueTask<Task> AddFile(bool deleteExisting, Stream input, IRequest request)
        {


            List<Account> accounts = reader.ConvertStreamToObjects(input).list;

            foreach (Account account in accounts) 
            {
                Console.WriteLine(account.CostumerName);
            }

            return new ValueTask<Task>();
        }
    }
}
