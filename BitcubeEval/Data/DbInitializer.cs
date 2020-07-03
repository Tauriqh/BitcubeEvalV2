using BitcubeEval.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitcubeEval.Data
{
    public class DbInitializer
    {
        public static void Initialize(BitvalEvalContext context)
        {
            context.Database.EnsureCreated();

            // look for any users
            if (context.ApplicationUser.Any())
            {
                return;
            }

            // password is all the same Admin@1, the hashed is saved
            var appusers = new ApplicationUser[]
            {
                new ApplicationUser{EmailAddress="admin@gmail.com", FirstName="Admin", LastName="User", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="tauriq@gmail.com", FirstName="Tauriq", LastName="Hendricks", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="susan@gmail.com", FirstName="Susan", LastName="White", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="mike@gmail.com", FirstName="Michael", LastName="Johnson", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="kyle@gmail.com", FirstName="Kyle", LastName="Alexander", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="john@gmail.com", FirstName="John", LastName="Black", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="deigo@gmail.com", FirstName="Deigo", LastName="Abrahams", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="tasneem@gmail.com", FirstName="Tasneem", LastName="Jones", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="maria@gmail.com", FirstName="Maria", LastName="Brown", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="alex@gmail.com", FirstName="Alex", LastName="salie", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="griffin@gmail.com", FirstName="Griffin", LastName="Parker", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="mikhail@gmail.com", FirstName="Mikhail", LastName="Williams", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="kim@gmail.com", FirstName="Kim", LastName="Osman", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="johnB@gmail.com", FirstName="John", LastName="Brown", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"},
                new ApplicationUser{EmailAddress="kyleB@gmail.com", FirstName="Kyle", LastName="Brown", Password="de5090839402b1ddabc6c6246ea0ac6d2cdcceb296c1dac9eb5e604972bfa1ce"}
            };

            foreach (ApplicationUser a in appusers)
            {
                context.ApplicationUser.Add(a);
            }
            context.SaveChanges();

            var friends = new Friend[]
            {
                new Friend{UserID1=1, UserID2=2, Confirmed=true},
                new Friend{UserID1=1, UserID2=4, Confirmed=false},
                new Friend{UserID1=1, UserID2=5, Confirmed=false},

                new Friend{UserID1=2, UserID2=3, Confirmed=false},
                new Friend{UserID1=2, UserID2=5, Confirmed=false},

                new Friend{UserID1=3, UserID2=1, Confirmed=true},
                new Friend{UserID1=3, UserID2=5, Confirmed=false}
            };

            foreach (Friend f in friends)
            {
                context.Friend.Add(f);
            }
            context.SaveChanges();
        }
    }
}
