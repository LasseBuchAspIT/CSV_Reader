﻿using GenHTTP.Modules.Webservices;
using GenHTTP.Api.Protocol;
using CSV_Reader.DAL;
using Lib;
using Microsoft.IdentityModel.Tokens;
using GenHTTP.Modules.Authentication;
using GenHTTP.Api.Content.Authentication;

namespace CSV_Reader
{
    public class Service
    {
        private string dir;
        private readonly string connectionString;
        CsvProgramTestContext context;
        Adder<Account, CsvProgramTestContext> adder;


        //Constructor initializing all needed variables/objects
        //Considering moving to seperate method
        public Service()
        {
            dir = System.Reflection.Assembly.GetExecutingAssembly().Location;
            connectionString = SettingsReader.GetConnectionString(dir + "Settings.txt");
            context = new(connectionString);
            adder = new Adder<Account, CsvProgramTestContext>(connectionString);
        }



        [ResourceMethod(RequestMethod.PUT, "AddFile")]
        public ValueTask<Task> AddFile(bool deleteExisting, Stream input, IRequest request)
        {
            int user = context.Users.Where(a => a.Name == request.GetUser<IUser>().DisplayName).FirstOrDefault().Id;
            (List<Account> l, bool b) values = Reader<Account>.ConvertStreamToObjects(input);

            foreach(Account a in values.l)
            {
                a.UserId = user;
            }

            adder.AddListToDb(values.l, values.b);
            //recreate context to get changes
            context = new(SettingsReader.GetConnectionString(dir + "/Settings.txt"));
            return new ValueTask<Task>();
        }

        [ResourceMethod("GetAll")]
        public List<Account> GetAll()
        {
            //quick makeshift solution, need to fix
            List<Account> list = context.Accounts.ToList();
            foreach(Account a in list)
            {
                if (a.Fsa.IsNullOrEmpty())
                {
                   a.Fsa = "";
                }
                if (a.Vip.IsNullOrEmpty())
                {
                    a.Vip = "";
                }
                if (a.UserId == null)
                {
                    a.UserId = 0;
                }
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
