using EFTest.DbContext;
using System;
using System.Linq;

namespace EFTest
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        void InitUsers()
        {
            using (var db = new SqlContext())
            {
                Console.WriteLine($"Looking for administrative user-accounts.");
                var manager = db.Users.FirstOrDefault(a => a.UsersRole == UsersRoles.Manager);
                if (manager == null)
                {
                    Console.WriteLine($"Manager is not present. Creating.");
                }
                else
                {

                }
            }
        }
    }
}
