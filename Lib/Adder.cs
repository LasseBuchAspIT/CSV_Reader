using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Adder<T> where T : class
    {
        DbSet<T> dbSet;
        Reader<T> reader;

        public Adder(DbSet<T> dbSet)
        {
            this.dbSet = dbSet;
            reader = new Reader<T>();
        }

        private void AddListToDb(List<T> list)
        {
            foreach (T item in list) 
            {
                dbSet.Add(item);
            }
        }

        public void AddStreamToDb(Stream stream)
        {
            foreach(T item in reader.ConvertStreamToObjects(stream).Item1)
            {
                dbSet.Add(item);
            }
        }

    }
}
