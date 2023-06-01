using CSV_Reader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GenHTTP.Engine;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Authentication;
using GenHTTP.Modules.Authentication.ApiKey;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Practices;
using GenHTTP.Modules.Security;
using GenHTTP.Modules.Webservices;
using Lib;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CSV_Reader.DAL;
using Microsoft.Identity.Client;

namespace CSV_Service
{
    public class CSV_Service
    {
        public void Run()
        {
            string dir = Assembly.GetExecutingAssembly().Location;
            //get variables from settings
            var assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine("Getting connectionString");
            CsvProgramTestContext context = new(SettingsReader.GetConnectionString(dir + "Settings.txt"));
            Console.WriteLine("Getting Port");
            ushort port = SettingsReader.GetPort(dir + "Settings.txt");

            //add users to auth
            Console.WriteLine("Adding users");
            var auth = BasicAuthentication.Create();
            foreach(User user in context.Users)
            {
                auth.Add(user.Name, user.Password);
            }

            //Create layout
            var PageLayout = Layout.Create()
            .AddService<Service>("Service")
            .Add(CorsPolicy.Permissive())
            .Fallback(Content.From(Resource.FromAssembly(assembly.GetManifestResourceNames()[0])))
            .Authentication(auth)
            .Index(Content.From(Resource.FromAssembly(assembly.GetManifestResourceNames()[0])));

            //start site
            Console.WriteLine("GenHttp Starting on port " + port);
            GenHTTP.Engine.Host.Create()
                    .Console()
                    .Port(port)
                    .Defaults()
                    .Handler(PageLayout)
                    .Run();
        }

    }
}
