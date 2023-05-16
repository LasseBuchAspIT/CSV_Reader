using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class TestClass
    {
        string Make;
        int Year;
        string Model;
        string? Destroyed;


        public TestClass(string make, int year, string model, string? destroyed = null)
        {
            Make = make;
            Year = year;
            Model = model;
            Destroyed = destroyed;
        }

        public override string ToString()
        {
            return $"Make: {Make}\nModel: {Model}\nYear: {Year}\nDestroyed?: {!(string.IsNullOrEmpty(Destroyed))}";
        }
    }
}
