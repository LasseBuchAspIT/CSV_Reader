using GenHTTP.Modules.Webservices;
using GenHTTP.Api.Protocol;
using CSV_Reader.DAL;
using Lib;
using Microsoft.IdentityModel.Tokens;
using static System.Net.Mime.MediaTypeNames;

namespace CSV_Reader
{
    public class Service
    {
        private readonly string dir;
        private readonly string connectionString;
        CsvProgramTestContext context;
        Adder<Account, CsvProgramTestContext> adder;


        //Constructor initializing all needed variables/objects
        //Considering moving to seperate method
        public Service()
        {
            dir = System.AppDomain.CurrentDomain.BaseDirectory;
            connectionString = SettingsReader.GetConnectionString(dir + "/Settings.txt");
            context = new(connectionString);
            adder = new Adder<Account, CsvProgramTestContext>(connectionString);
        }



        [ResourceMethod(RequestMethod.PUT, "AddFile")]
        public ValueTask<Task> AddFile(bool deleteExisting, Stream input, IRequest request)
        {

            adder.AddStreamToDb(input);

            //recreate context to get changes
            context = new(SettingsReader.GetConnectionString(dir + "/Settings.txt"));
            return new ValueTask<Task>();
        }

        [ResourceMethod("GetAll")]
        public List<Account> GetAll()
        {
            //quick makeshift solution, need to fix
            List<Account> list = context.Accounts.ToList();
            if (list[0].Fsa.IsNullOrEmpty())
            {
                list[0].Fsa = "";
            }
            if (list[0].Vip.IsNullOrEmpty())
            {
                list[0].Vip = "";
            }
            if (list[0].UserId == null)
            {
                list[0].UserId = 0;
            }
            return list;

        }

        [ResourceMethod("GetById")]
        public Account GetAccountById(int id) 
        {
            return context.Accounts.Where(a => a.CostumerNumber == id).FirstOrDefault();
        }

       
    }
}
