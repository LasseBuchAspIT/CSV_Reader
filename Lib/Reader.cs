using System.Runtime.InteropServices;

namespace Lib
{
    public class Reader<T> where T : class
    {
        public (List<T>, bool DeleteExisting) DataList(Stream stream)
        {
            List<T> list = new List<T>();

            

            return (list, false);
        }
    }
}