using Lib;

namespace CSV_Reader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Creator<TestClass> creator = new(); 
            List<string> para = new List<string>();
            para.Add("Test");
            para.Add("2");
            para.Add("Test");


            TestClass result = creator.CreateT(para);
            Console.WriteLine(result.ToString());
        }
    }
}