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
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Practices;
using GenHTTP.Modules.Security;
using GenHTTP.Modules.Webservices;
using Lib;

namespace CSV_Service
{
    public class CSV_Service
    {
        public void Run()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var PageLayout = Layout.Create()
            .AddService<Service>("Service")
            .Add(CorsPolicy.Permissive())
                 .Fallback(Content.From(Resource.FromAssembly(assembly.GetManifestResourceNames()[0])))
                 .Index(Content.From(Resource.FromAssembly(assembly.GetManifestResourceNames()[0])));


            GenHTTP.Engine.Host.Create()
                    .Console()
                    .Port(SettingsReader.GetPort())
                    .Defaults()
                    .Handler(PageLayout)
                    .Run();
        }

    }
}
