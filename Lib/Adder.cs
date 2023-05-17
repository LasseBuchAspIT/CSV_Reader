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
            //delete all entries in database
            if (deleteExisitng)
            {
                foreach (T item in dbSet)
                {
                    dbSet.Remove(item);
                }
            }
            else
            {
                //replace all items which already exist in db with the updated versions (done by deleting old to avoid conflicts)
                //consider changing this to actually replacing instead of deleting and adding new
                foreach(T item in list)
                {
                    if (dbSet.Any(a => a.Equals(item)))
                    {
                        dbSet.Remove(item);
                    }
                }
            }

            //add all the items from list to db
            foreach (T item in list) 
            {
                dbSet.Add(item);
            }

            //save changes
            dbContext.SaveChanges();
        }


        //Takes a stream and adds it to the database
        public void AddStreamToDb(Stream stream)
        {
            //trupple to only need to convert stream once
            //convert stream to a list of objects<T> and a bool for checking if should delete existing database
            (List<T> list, bool delete) values = reader.ConvertStreamToObjects(stream);

            //add values to db
            AddListToDb(values.list, values.delete);
        }

    }
}
