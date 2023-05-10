using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakaleBLL;
using MakaleEntities;
using MakaleWeb_MVC.Models;

namespace MakaleWeb_MVC.Controllers
{
    public class MakaleController : Controller
    {
       MakaleYonet my=new MakaleYonet();
       KategoriYonet ky=new KategoriYonet();

        // GET: Makale
        public ActionResult Index()
        {
            return View(my.Listele().Where(x=>x.Kullanici.Id==SessionUser.Login.Id));
        }

        // GET: Makale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }
        public ActionResult Create()
        {
            ViewBag.Kategori = new SelectList(ky.Listele(),"Id","Baslik");
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Makale makale)
        {
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ModelState.Remove("Kategori.Aciklama");
            ViewBag.Kategori = new SelectList(ky.Listele(), "Id", "Baslik", makale.Kategori.Id);
            if (ModelState.IsValid)
            {
                makale.Kullanici = SessionUser.Login;
                makale.Kategori = ky.KategoriBul(makale.Kategori.Id);
                MakaleBLLSonuc<Makale> sonuc = my.MakaleEkle(makale);
                if (sonuc.hatalar.Count > 1)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Kategori = new SelectList(ky.Listele(), "Id", "Baslik",makale.Kategori.Id);
            return View(makale);
        }

        public ActionResult Edit(int? id)
        {
            Makale makale = my.MakaleBul(id.Value);
            ViewBag.Kategori = new SelectList(ky.Listele(), "Id", "Baslik", makale.Kategori.Id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (makale == null)
            {
                return HttpNotFound();
            }
            
            return View(makale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Makale makale)
        {
            ViewBag.Kategori = new SelectList(ky.Listele(), "Id", "Baslik", makale.Kategori.Id);
            ModelState.Remove("DegistirenKullanici");
            ModelState.Remove("Kategori.Baslik");
            ModelState.Remove("Kategori.DegistirenKullanici");
            ModelState.Remove("Kategori.Aciklama");        
            if (ModelState.IsValid)
            {
                makale.Kategori = ky.KategoriBul(makale.Kategori.Id);
                MakaleBLLSonuc<Makale> sonuc = my.MakaleUpdate(makale);
                if (sonuc.hatalar.Count > 0)
                {
                    sonuc.hatalar.ForEach(x => ModelState.AddModelError("", x));
                    return View(makale);
                }
                my.MakaleUpdate(makale);
                return RedirectToAction("Index");
            }
            
            return View(makale);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale =my.MakaleBul(id.Value);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }
      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            my.MakaleSil(id);
            return RedirectToAction("Index");
        }

       
    }
}
