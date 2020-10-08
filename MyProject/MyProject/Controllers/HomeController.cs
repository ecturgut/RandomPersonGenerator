using MyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using LinqToDB;
using System.Data.Entity;

namespace MyProject.Controllers
{
  
    public class HomeController : Controller
    {
        KullaniciDBEntities db = new KullaniciDBEntities();
       

        public ActionResult Olusturma()
        {
            var _entities = new KullaniciDBEntities();
            var model = _entities.Kisi;
            
            //KullaniciDBEntities db = new KullaniciDBEntities();

            //List<Kisi> kL = db.Kisi.ToList();
            //ViewBag.kisiList = new SelectList(kL, "AdSoyad", "Email", "Yas", "Konum", "Telefon", "Parola");

            return View();
        }

        [HttpPost]
        public ActionResult Olusturma(KullaniciDBEntities model)
        {
            try
            {
                KullaniciDBEntities db = new KullaniciDBEntities();
                List<Kisi> kL = db.Kisi.ToList();
                ViewBag.kisiList = new SelectList(kL, "AdSoyad", "Email", "Yas", "Konum", "Telefon", "Parola");

                Kisi k = new Kisi();

                k.ID = model.ID;
                k.AdSoyad = model.AdSoyad;
                k.Email = model.Email;
                k.Yas = model.Yas;
                k.Konum = model.Konum;
                k.Telefon = model.Telefon;
                k.Parola = model.Parola;
                

                db.Kisi.Add(k);
                db.SaveChanges();

                int SonId = k.ID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           return RedirectToAction("Listeleme");
        }
        public ActionResult SoruSil(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KisiSoruları s = db.KisiSoruları.Find(id);

            if (s == null)
            {
                return HttpNotFound();
            }
            return View(s);

        }

        [HttpPost, ActionName("SoruSil")]
        [ValidateAntiForgeryToken]
        public ActionResult SoruSil(int id)
        {
            KisiSoruları s = db.KisiSoruları.Find(id);
            db.KisiSoruları.Remove(s);
            db.SaveChanges();
            return RedirectToAction("KullaniciSoruEkran");
        }

        public ActionResult Listeleme()
        {
            Kisi k = new Kisi();
            List<Kisi> kisiListesi = db.Kisi.ToList();
            return View(kisiListesi);
        }
        public ActionResult ListedenKullaniciSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kisi k = db.Kisi.Find(id);

            if (k == null)
            {
                return HttpNotFound();
            }
            return View(k);
        }

        [HttpPost, ActionName("ListedenKullaniciSil")]
        [ValidateAntiForgeryToken]
        public ActionResult ListedenKullaniciSil(int id)
        {
            Kisi k = db.Kisi.Find(id);
            db.Kisi.Remove(k);
            db.SaveChanges();
            return RedirectToAction("Listeleme");
        }
        public ActionResult SoruKullanıcı()
        {
           
            KullaniciDBEntities db = new KullaniciDBEntities();

            List<SelectListItem> kisi = new List<SelectListItem>();

            foreach (var item in db.Kisi.ToList())
            {   
                kisi.Add(new SelectListItem { Text = item.AdSoyad, Value = item.ID.ToString() });
            }

            ViewBag.kisiler = kisi;

            List<SelectListItem> soru = new List<SelectListItem>();
           
            foreach (var item in db.Soru.ToList())
            {   
                soru.Add(new SelectListItem { Text = item.SoruAd, Value = item.ID.ToString() });
            }
           
            ViewBag.sorular = soru;

            return View();
        }
        public ActionResult KullanıcıSoruEkran()
        {
            KisiSoruları ks = new KisiSoruları();
            List<KisiSoruları> kisiSoruları = db.KisiSoruları.ToList();
            return View(kisiSoruları);
        }
    }
}