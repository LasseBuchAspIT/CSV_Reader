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

namespace CSV_Service
{
    public class CSV_Service
    {
        public void Run()
        {
            var assembly = Assembly.GetExecutingAssembly();
            CsvProgramTestContext context = new(SettingsReader.GetConnectionString());


            Console.WriteLine("adding users");
            var auth = BasicAuthentication.Create();
            foreach(User user in context.Users)
            {
                auth.Add(user.Name, user.Password);
            }


            var PageLayout = Layout.Create()
            .AddService<Service>("Service")
            .Add(CorsPolicy.Permissive())
            .Fallback(Content.From(Resource.FromAssembly(assembly.GetManifestResourceNames()[0])))
            .Authentication(auth)
            .Index(Content.From(Resource.FromAssembly(assembly.GetManifestResourceNames()[0])));


            Console.WriteLine("GenHttp Starting on port " + SettingsReader.GetPort());
            GenHTTP.Engine.Host.Create()
                    .Console()
                    .Port(SettingsReader.GetPort())
                    .Defaults()
                    .Handler(PageLayout)
                    .Run();
        }

    }
}
