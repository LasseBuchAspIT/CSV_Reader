using System.IO;
using System.Runtime.InteropServices;

namespace Lib
{
    public class Reader<T> where T : class
    {
        public List<T> ConvertStreamToObjects(Stream stream)
        {
            List<T> list = new List<T>();
            string line;
            Creator<T> creator = new();

            using (StreamReader reader = new StreamReader(stream))
            {
                //Converts to Objects
                while((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        string[] args = line.Split(";");
                        list.Add(creator.CreateT(args.ToList()));
                        Console.WriteLine("Object added to list Succesfully");
                    }
                    catch 
                    {
                        Console.WriteLine("Line formatted incorrectly, skipping and moving on...");
                    }
                }
            }

            return list;
        }

        public bool GetBoolAndCutStreamToSize(Stream stream)
        {
            string line;
            bool delete = false;
            using (StreamReader reader = new StreamReader(stream))
            {

                //read only first part of stream which contains headers and the deleteExisting bool
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "true")
                    {
                        delete = true;
                    }
                    if (line == "Content-Type: text/csv")
                    {
                        //consume next line to avoid trying to create object from empty line
                        reader.ReadLine();
                        break;
                    }
                }
            }
            return delete;
        }
    }
}