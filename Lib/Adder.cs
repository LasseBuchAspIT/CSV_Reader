using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib
{
    public class Adder<T1, T2> where T1 : class where T2 : DbContext
    {
        Reader<T1> reader;
        DbContext dbContext;
        DbSet<T1> dbSet;
        CustomDbContextFactory<T2> customDb = new();

        public Adder(string ConnectionString)
        {
            try
            {
                this.reader = new Reader<T1>();
                Console.WriteLine("creting customDbContext");
                this.dbContext = customDb.CreateDbContext(ConnectionString);
                dbContext.Database.SetConnectionString(ConnectionString);
                Console.WriteLine("Connecting to: " + dbContext.Database.GetConnectionString());
                dbContext.Database.OpenConnection();
                Console.WriteLine("Succesfully connected");
                Console.WriteLine("Creating DbSet");
                this.dbSet = dbContext.Set<T1>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid ConnectionString... Error: " + ex.Message);
            }

        }

        private void AddListToDb(List<T1> list, bool deleteExisitng)
        {
            //delete all entries in database
            if (deleteExisitng)
            {
                foreach (T1 item in dbContext.Set<T1>())
                {
                    dbContext.Set<T1>().Remove(item);
                }
            }

            //add all the items from list to dbx
            foreach (T1 item in list) 
            {
                if (dbSet.Any(a => a.Equals(item)))
                {
                    dbSet.Remove(dbSet.Where(a => a.Equals(item)).FirstOrDefault());
                }
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
            (List<T1> list, bool delete) values = reader.ConvertStreamToObjects(stream);

            //add values to db
            AddListToDb(values.list, values.delete);
        }

        public interface ICustomDbContextFactory<out T> where T : DbContext
        {
            T CreateDbContext(string connectionString);
        }

        public class CustomDbContextFactory<T> : ICustomDbContextFactory<T> where T : DbContext
        {
            public T CreateDbContext(string connectionString)
            {
                var optionsBuilder = new DbContextOptionsBuilder<T>();
                optionsBuilder.UseSqlServer(connectionString);
                return System.Activator.CreateInstance(typeof(T), optionsBuilder.Options) as T;
            }
        }

    }
}
