using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Lib
{
    public static class Reader<T> where T : class
    {
        public static (List<T> list, bool delete) ConvertStreamToObjects(Stream stream)
        {
            bool delete = false;
            List<T> list = new List<T>();
            string line;

            //create instance of creator


            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
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

                reader.Read();

                //Converts to Objects
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("WebKitFormBoundary")) 
                    {
                        return (list, delete);
                    }
                    try
                    {
                        //split line into seperate values
                        string[] args = line.Split(";");

                        //pass valuesi into creator to convert to object and add to list
                        list.Add(Creator<T>.CreateT(args.ToList()));
                        Console.WriteLine("Object added to list Succesfully");
                    }
                    catch 
                    {
                        Console.WriteLine("Line formatted incorrectly, skipping and moving on...");
                    }
                }
                
            }
            
            return (list, delete);
        }

        //done to remove header
        public static (bool asd, Stream str) GetBoolAndCutStreamToSize(Stream stream)
        {
            string line;
            Stream ReturnStr = null;
            bool delete = false;
            using (StreamReader reader = new StreamReader(stream))
            {

                //read only first part of stream which contains headers and the deleteExisting bool
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
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
                stream.CopyTo(ReturnStr);
            }
            return (delete, ReturnStr);
        }
    }
}