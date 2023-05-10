using MakaleEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakaleDAL
{
    public class VeriTabanıOlusturucu : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            Kullanici admin = new Kullanici()
            {
                Adi = "Eylül",
                Soyad = "Ayçicek",
                Email = "seylul@gmail.com",
                Admin = true,
                Aktif = true,
                KullaniciAdi = "lüley",
                Sifre = "123",
                AktifGuid = Guid.NewGuid(),
                KayitTarihi = DateTime.Now,
                DegistirmeTarihi = DateTime.Now.AddMinutes(5),
                DegistirenKullanici = "eylül",
                ProfilResimDosyaAdi = "user_1.jpg"
            };
            context.Kullanicilar.Add(admin);

            for (int i = 0; i < 5; i++)
            {
                Kullanici k = new Kullanici()
                {
                    Adi = FakeData.NameData.GetFirstName(),
                    Soyad = FakeData.NameData.GetSurname(),
                    Admin = false,
                    Aktif = true,
                    Email = FakeData.NetworkData.GetEmail(),
                    KullaniciAdi = $"user{i}",
                    Sifre = "123",
                    KayitTarihi = DateTime.Now.AddDays(-1),
                    DegistirmeTarihi = DateTime.Now,
                    DegistirenKullanici = $"user{i}",
                    ProfilResimDosyaAdi = "user_1.jpg"
                };

                context.Kullanicilar.Add(k);
            }
            context.SaveChanges();

            List<Kullanici> kullanicilar = context.Kullanicilar.ToList();
            //Kategori ekle
            for (int i = 0; i < 5; i++)
            {
                // Kategori

                Kategori kat = new Kategori()
                {
                    Baslik = FakeData.PlaceData.GetStreetName(),
                    Aciklama = FakeData.PlaceData.GetAddress(),
                    KayitTarihi = DateTime.Now,
                    DegistirmeTarihi = DateTime.Now,
                    DegistirenKullanici = admin.KullaniciAdi
                };

                context.Kategoriler.Add(kat);

                for (int j = 0; j < 6; j++)
                {
                    Kullanici kullanici = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                    // Makale ekle
                    Makale makale = new Makale()
                    {
                        Baslik = FakeData.TextData.GetAlphabetical(10),
                        Icerik = FakeData.TextData.GetSentences(2),
                        Taslak = false,
                        BegeniSayisi = FakeData.NumberData.GetNumber(2, 5),
                        KayitTarihi = DateTime.Now.AddDays(-2),
                        DegistirmeTarihi = DateTime.Now,
                        DegistirenKullanici = admin.KullaniciAdi,
                        Kullanici = kullanici
                    };

                    kat.Makaleler.Add(makale);
                    //yorum ekle

                    for (int k = 0; k < 3; k++)
                    {
                        Kullanici yorum_kullanici = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                        Yorum yorum = new Yorum()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            KayitTarihi = DateTime.Now,
                            DegistirmeTarihi = DateTime.Now,
                            DegistirenKullanici = admin.KullaniciAdi,
                            Kullanici = yorum_kullanici
                        };

                        makale.Yorumlar.Add(yorum);
                    }
                    

                    // begeni ekle
                    for (int x = 0;x<makale.BegeniSayisi; x++)
                    {
                        Kullanici begenen_kullanici = kullanicilar[FakeData.NumberData.GetNumber(0, 5)];
                        Begeni begen = new Begeni()
                        {
                            Kullanici = begenen_kullanici
                        };
                        makale.Begeniler.Add(begen);
                    } //for begeni
                }//for makale
            }//for kategori

            context.SaveChanges();
        }
    }
}



