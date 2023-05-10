using MakaleCommon;
using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDAL
{
    public class Repository<T> :Singleton, IRepository<T> where T : class // where T :class dememizin sebebi db.set içine gönderdiğimiz T'lerin class olduğunu garanti etmek aksi takdirde dbset izin vermez string vs yazabiliyor olursak
    {
        
        private DbSet<T> dbset;
        public Repository()
        {
            dbset = db.Set<T>();
        }

        public List<T> Liste()
        {
            //return db.Set<T>().ToList(); bunun yerine db.set<T() yazmamak için dbset diye değişken tanımladık ve consructor'da ona değer atadık. değer atamak için consructor kullanmalıyız
            // her seferinde dbsete ulaşmaya çalışmasın bir kere consructorunda ulaşsın ve onu kullansın diye yaptık
            return dbset.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return dbset.AsQueryable(); //burada bir query oluşturuyoruz imzasını da Irepositorye ekliyoruz yine
        }



        public List<T> Liste(Expression<Func<T, bool>> kosul)
        {
            return dbset.Where(kosul).ToList();
        }


        public int Insert(T nesne)
        {
            dbset.Add(nesne);

            if (nesne is BaseClass)
            {
                BaseClass obj = nesne as BaseClass;
                DateTime tarih = DateTime.Now;

                obj.KayitTarihi = tarih;
                obj.DegistirmeTarihi = tarih;
                obj.DegistirenKullanici = Uygulama.login;
            }

            return db.SaveChanges();
        }


        public int Delete(T nesne)
        {
            dbset.Remove(nesne); //burada execute etmiş oluyoruz
            return db.SaveChanges();
        }

        public int Update(T nesne)
        {
            if (nesne is BaseClass)
            {
                BaseClass obj = nesne as BaseClass;

                obj.DegistirmeTarihi = DateTime.Now;
                obj.DegistirenKullanici = Uygulama.login;
            }
            return db.SaveChanges();
        }
        
        public T Find(Expression<Func<T, bool>> kosul)
        {
           return dbset.FirstOrDefault(kosul); // ilk bulduğum kayıt gelsin
        }
    }
}
