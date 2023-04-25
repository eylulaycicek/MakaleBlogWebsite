using MakaleDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class Test
    {
        public Test()
        {
            DatabaseContext db=new DatabaseContext();
            db.Kullanicilar.ToList();

           // db.Database.CreateIfNotExists();
        }
    }
}
