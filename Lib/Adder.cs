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
        DbContext dbContext;

        public Adder(DbContext context)
        {
            dbSet = context.Set<T>();
            reader = new Reader<T>();
            dbContext = context;
        }

        private void AddListToDb(List<T> list, bool deleteExisitng)
        {
            if (deleteExisitng)
            {
                foreach (T item in dbSet)
                {
                    dbSet.Remove(item);
                }
            }
            foreach (T item in list) 
            {
                dbSet.Add(item);
            }
            dbContext.SaveChanges();
        }

        public void AddStreamToDb(Stream stream)
        {
            (List<T> list, bool delete) values = reader.ConvertStreamToObjects(stream);
            AddListToDb(values.list, values.delete);
        }

    }
}
