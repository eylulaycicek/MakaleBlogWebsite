using MakaleBLL;
using MakaleCommon;
using MakaleEntities;
using MakaleEntities.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor;
using System.Web.WebSockets;
using MakaleWeb_MVC.Models;
using System.Data.Entity;

namespace MakaleWeb_MVC.Controllers
{
    public class HomeController : Controller
    {
        MakaleYonet my = new MakaleYonet();
        KategoriYonet ky = new KategoriYonet();
        KullaniciYonet kuly = new KullaniciYonet();
        BegeniYonet by=new BegeniYonet();

      
        public ActionResult Index()
        {
            Test test = new Test();
            //test.EkleTest();
            //test.UpdateTest();
            //test.YorumTest();       
            return View(my.Listele());
        }

        public ActionResult Kategori(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); //hata gönderiyoruz
            }

            Kategori kat = ky.KategoriBul(id.Value);

            return View("Index", kat.Makaleler);
        }


        //farklı bir örnek olarak bu şekilde de yazabiliriz diğerini kullanacağım
        public PartialViewResult KategoriPartial() // partialpage içine yazmak yerine bu şekilde de yazıp indexe actionlink ile gönderebiliriz diğer türlü html.partial ile gönderiyoruz
        {
            KategoriYonet ky = new MakaleBLL.KategoriYonet();
            List<Kategori> liste = ky.Listele();
            return PartialView("_PartialPageKat2", liste);
        }

        public ActionResult EnBegenilenler()
        {
            return View("Index",my.Listele().OrderByDescending(x=>x.BegeniSayisi).ToList());

        }

        public ActionResult SonYazilanlar()
        {
            return View("Index", my.Listele().OrderByDescending(x => x.DegistirmeTarihi).ToList());
        }

        public ActionResult Hakkimizda()
        {
            return View();
        }

        public ActionResult Giris()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Giris(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                MakaleBLLSonuc<Kullanici> sonuc = kuly.LoginKontrol(model);

                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }            
                SessionUser.Login= sonuc.nesne;
                Uygulama.login = sonuc.nesne.KullaniciAdi;
                return RedirectToAction("Index");

            }

            return View(model);           
        }

        public ActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]

        public ActionResult KayitOl(RegisterModel model)
        { 
            
            //kullaniciadı ve mail kontrolü
            //kayıt işlemi yapılacak
            //aktivasyon maili gönderilecek maildeki linke tıklayınca
            if (ModelState.IsValid) //tüm datalar geçerlidir doldurulmuştur stringlengthleri uygundur ise çalışacak
            {
               MakaleBLLSonuc<Kullanici> sonuc= kuly.KullaniciKaydet(model);
                if (sonuc.hatalar.Count>0) //hataların countı sıfırdan büyükse hata vardır.
                {
                    //ModelState.AddModelError("", "Bu kullanıcı adı ya da email kayıtlı"); 
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);
                }
                else
                {
                    //database'e kaydet deri
                    return RedirectToAction("KayitBasarili");

                }

            }
            return View();
        }


        public ActionResult KayitBasarili()
        {
            return View();
        }

        public ActionResult HesapAktiflestir(Guid id)
        {
            MakaleBLLSonuc<Kullanici> sonuc = kuly.ActivateUser(id);
            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }

            return View();
        }

        public ActionResult Cikis()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Error()
        {
            List<string> errors = new List<string>();

            if (TempData["hatalar"] != null)
            {
                ViewBag.hatalar = TempData["hatalar"];
            }
            else
            { ViewBag.hatalar = errors; }

            return View();
        }

        public ActionResult ProfilGoster()
        {
          

            MakaleBLLSonuc<Kullanici> sonuc = kuly.KullaniciBul(SessionUser.Login.Id);

            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }

            return View(sonuc.nesne);

        }

        public ActionResult ProfilDegistir()
        {
           

            MakaleBLLSonuc<Kullanici> sonuc = kuly.KullaniciBul(SessionUser.Login.Id);

            if (sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }


            return View(sonuc.nesne);
        }

        [HttpPost]
        public ActionResult ProfilDegistir(Kullanici model, HttpPostedFileBase profilresim)
        {
            ModelState.Remove("DegistirenKullanici");

            if (ModelState.IsValid)
            {
                if (profilresim != null && (profilresim.ContentType == "image/jpg" || profilresim.ContentType == "image/jpeg" || profilresim.ContentType == "image/png"))
                {
                    string dosya = $"user_{model.Id}.{profilresim.ContentType.Split('/')[1]}";

                    profilresim.SaveAs(Server.MapPath($"~/resim/{dosya}"));

                    model.ProfilResimDosyaAdi = dosya;
                }
                Uygulama.login = model.KullaniciAdi;
                MakaleBLLSonuc<Kullanici> sonuc = kuly.KullaniciUpdate(model);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(model);

                }
                SessionUser.Login = sonuc.nesne;
                return RedirectToAction("ProfilGoster");
            }
            else
            {
                return View(model);
            }
           
        }
         public ActionResult ProfilSil()
        {
            
            MakaleBLLSonuc<Kullanici> sonuc=kuly.KullaniciSil(SessionUser.Login.Id);
            if(sonuc.hatalar.Count > 0)
            {
                TempData["hatalar"] = sonuc.hatalar;
                return RedirectToAction("Error");
            }
            Session.Clear();
            return RedirectToAction("Index");
        }



        public ActionResult Begendiklerim()
        {
            var query = by.ListQueryable().Include("Kullanici").Include("Makale").Where(x => x.Kullanici.Id == SessionUser.Login.Id).Select(x => x.Makale).Include("Kategori").Include("Kullanici").OrderByDescending(x => x.DegistirmeTarihi);
            return View("Index",query.ToList());
        }



    }
}