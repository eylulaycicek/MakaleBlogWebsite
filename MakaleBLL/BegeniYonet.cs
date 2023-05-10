using MakaleDAL;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleBLL
{
    public class BegeniYonet
    {
        Repository<Begeni> rep_begen=new Repository<Begeni>();

        public IQueryable<Begeni> ListQueryable()
        {
            return rep_begen.ListQueryable();
        }
    }
}
