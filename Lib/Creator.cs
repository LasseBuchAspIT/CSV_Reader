using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Creator<T> where T : class
    {
        public T CreateT(List<string> para)
        {
            try
            {
                List<object> paraValidated = ValidateParams(para);
                Type t = typeof(T);
                var constructors = t.GetConstructors();
                while (paraValidated.Count < constructors[0].GetParameters().Length)
                {
                    //add null values until para length matches correct constructor length, done to avoid errors from lacking null values in constructor
                    paraValidated.Add(null);
                }
                return (T)Activator.CreateInstance(typeof(T), paraValidated.ToArray());
            }
            catch(Exception e)
            {
                throw new Exception($"{e.Message}");
            }
        }

        //Converts all values to the correct type for constructor
        private List<object> ValidateParams(List<string> para)
        {
            List<object> returnList = new List<object>();
            Type t = typeof(T);
            var constructors = t.GetConstructors();
            for (int i = 0; i < para.Count; i++)
            {
                //find datatype to convert to
                if (constructors[0].GetParameters()[i].ParameterType == typeof(string))
                {
                    returnList.Add(para[i]);    
                }
                else if(constructors[0].GetParameters()[i].ParameterType == typeof(int))
                {
                    bool parsed = Int32.TryParse(para[i].ToString(), out int result);
                    if (parsed)
                    {
                        returnList.Add(result);
                    }
                    else
                    {
                        throw new ArgumentException($"Value {i} could not be converted to correct type. Check formatting of csv file...");
                    }
                }
                else if(constructors[0].GetParameters()[i].ParameterType == typeof(DateTime))
                {
                    //convert to datetime
                }
                else if(constructors[0].GetParameters()[i].ParameterType == typeof(bool))
                {
                    //convert to bool
                }
                
            }
            return returnList;
        }
    }
}
