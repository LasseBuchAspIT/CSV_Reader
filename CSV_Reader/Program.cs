using GenHTTP.Api.Content.Authentication;
using GenHTTP.Engine;
using GenHTTP.Modules.Authentication;
using GenHTTP.Modules.Authentication.ApiKey;
using GenHTTP.Modules.Basics;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Practices;
using GenHTTP.Modules.Security;
using GenHTTP.Modules.Webservices;
using Lib;
using System.Reflection;

namespace CSV_Reader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var PageLayout = Layout.Create()
                 .AddService<Service>("Service")
                 .Add(CorsPolicy.Permissive())
                 .Fallback(Content.From(Resource.FromAssembly(assembly.GetManifestResourceNames()[0])))
                 .Index(Content.From(Resource.FromAssembly(assembly.GetManifestResourceNames()[0])));


            Host.Create()
                .Console()
                .Port(SettingsReader.GetPort())
                .Defaults()
                .Handler(PageLayout)
                .Run();
            
        }
    }
}