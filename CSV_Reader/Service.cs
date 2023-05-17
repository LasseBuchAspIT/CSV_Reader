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
        CsvProgramTestContext context = new CsvProgramTestContext();
        Adder<Account> adder = new(new CsvProgramTestContext());
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
    }
}
