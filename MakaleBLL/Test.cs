using MakaleDAL;
using MakaleEntities;
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
        Repository<Kullanici> rep_kul = new Repository<Kullanici>();


        public Test()
        {
            //DatabaseContext db=new DatabaseContext();
            //db.Kullanicilar.ToList();

            // db.Database.CreateIfNotExists();
            List<Kullanici> sonuc = rep_kul.Liste();
            List<Kullanici> liste = rep_kul.Liste(x => x.Admin == true); //admin olan kullanıcıyı bulmasını istedik test etmek için

        }

        public void EkleTest()
        {
            rep_kul.Insert(new Kullanici() { Adi = "test", Soyad = "test", KullaniciAdi = "test", Email = "test", Sifre = "test", Admin = false, Aktif = false, AktifGuid = Guid.NewGuid(), KayitTarihi = DateTime.Now, DegistirenKullanici = "eylül", DegistirmeTarihi = DateTime.Now });
        }

        public void UpdateTest()
        {
            Kullanici k = rep_kul.Find(x => x.KullaniciAdi == "test"); // adı test olan kullanıcıyı k değişkenine aldık
            if (k != null) //kullanici bulunduysa update edeceğiz
            {
                k.Adi = "deneme";
                k.Soyad = "deneme";
                rep_kul.Update(k); //adını soyadını değiştirdikten sonra update etmemiz gerekli
            }
        }

        public void DeleteTest()
        {
            Kullanici k = rep_kul.Find(x => x.KullaniciAdi == "test");
            if (k != null) //kullanici bulunduysa update edeceğiz
            {
                rep_kul.Delete(k);
            }


        }

        public void YorumTest()
        {
            Repository<Makale> rep_makale = new Repository<Makale>(); // find metodunu kullanmak için makale tipinden repository çağırıyoruz
            Repository<Yorum> rep_yorum = new Repository<Yorum>();

            Kullanici k = rep_kul.Find(x => x.Id == 1);
            Makale m = rep_makale.Find(x => x.Id == 1)
;
            Yorum yorum = new Yorum()
            {
                Text = "deneme yorumu",
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi=DateTime.Now,
                DegistirenKullanici = "eylül",
                Kullanici = k,
                Makale = m
            };
            rep_yorum.Insert(yorum);
        }

    }
}
