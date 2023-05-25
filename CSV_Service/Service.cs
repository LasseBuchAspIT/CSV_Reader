using GenHTTP.Modules.Webservices;
using GenHTTP.Api.Protocol;
using CSV_Reader.DAL;
using Lib;

namespace CSV_Reader
{
    public class Service
    {
        private readonly string connectionString;
        CsvProgramTestContext context;
        Adder<Account, CsvProgramTestContext> adder;


        //Constructor initializing all needed variables/objects
        //Considering moving to seperate method
        public Service()
        {
            connectionString = SettingsReader.GetConnectionString("Settings.txt");
            context = new(connectionString);
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

       
    }
}
