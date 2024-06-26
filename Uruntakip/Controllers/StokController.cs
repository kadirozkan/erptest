﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages.Html;
using System.Xml;
using Uruntakip.db;
using Uruntakip.Models;

namespace Uruntakip.Controllers
{
    public class StokController : Controller
    {
        // GET: Urun
        uruntakipdbEntities6 db = new uruntakipdbEntities6();
        public ActionResult Index()
        {
            return View();
        }
        public string tarihduzelt_gun_ay_yıl(string m)
        {
            string tarih = Convert.ToDateTime(m).ToShortDateString();  // 2020.08.19 seklınde gelen tarıhı 19.08.2020 formatına cevırdık datetıme yazmak ıcın

            return tarih;
        }
        [Authorize]
        public ActionResult tedarikci()
        {
            List<tblkategori> liste = db.tblkategoris.ToList();
           
            return View(liste);
        }
        // -----------------------------------------------------Tedarikci ekleme-------------------------------
        [Authorize]
        [HttpPost]
        public ActionResult tedarikci(FormCollection frm)
        {
            List<tblkategori> liste = db.tblkategoris.ToList();
         

            try
            {
                tblCustomer t = new tblCustomer();
                t.adres = frm["adres"].ToString();
                t.firmaadi = frm["firmaadı"].ToUpper().ToString();
                t.ad = frm["adı"].ToString().Trim();
                t.soyad = frm["soyadı"].ToString().Trim();
                t.email = frm["email"].ToString();
                t.telefon = frm["telefon"].ToString();
                t.musteritipi = 0;
                db.tblCustomers.Add(t);
                db.SaveChanges();
                tblefirmaurun_kategorisi k = new tblefirmaurun_kategorisi();
                k.firmaid = t.firmaid;
                k.kategoriid = Convert.ToInt32(frm["kategori"]);
                db.tblefirmaurun_kategorisi.Add(k);

                db.SaveChanges();
                ViewBag.mesaj = 1;
            }
            catch (Exception)
            {

                ViewBag.mesaj = 0;
            }
            
            return View(liste);
        }
        [Authorize]
        //-----------------------------------------------------------tedarikci bilgileri jquery ıle alınıyor
        public ActionResult _tedarickibilgileri(int id)
        {
            List<cls_musteriler> liste = (from c in db.tblCustomers
                                          join k in db.tblefirmaurun_kategorisi on c.firmaid equals k.firmaid
                                          join m in db.tblkategoris on k.kategoriid equals m.kategoriID
                                          where c.firmaid == id
                                          select (new cls_musteriler { _musteriid = c.firmaid, _adres = c.adres, _email = c.email, _firmaadi = c.firmaadi, _personelad = c.ad, _personelsoyad = c.soyad, _urunkategori = m.kategoriadi, _telefon = c.telefon, _urunkatid = m.kategoriID })).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        //-----------------------------------------------------------butun tedarıkcıler cekılıyor
        public ActionResult _tedarikcilerigetir()
        {
            List<cls_musteriler> liste = (from c in db.tblCustomers
                                          join k in db.tblefirmaurun_kategorisi on c.firmaid equals k.firmaid
                                          join m in db.tblkategoris on k.kategoriid equals m.kategoriID
                                          select (new cls_musteriler { _musteriid = c.firmaid, _adres = c.adres, _email = c.email, _firmaadi = c.firmaadi, _personelad = c.ad, _personelsoyad = c.soyad, _urunkategori = m.kategoriadi, _telefon = c.telefon })).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }[Authorize]

        //--------------------------------------------------------tedarikci bilgileri jquery ıle guncellenıyor
        public ActionResult _tedarikci_bilgilerini_guncelle(int musteriid, string firmaadi, string personelad, string personelsoyad, string email, string telefon, int kategori, string adres)
        {
            int sonuc = 0;
            try
            {
                tblCustomer c = db.tblCustomers.FirstOrDefault(x => x.firmaid == musteriid);
                c.adres = adres;
                c.ad = personelad;
                c.email = email;
                c.firmaadi = firmaadi;
                c.soyad = personelsoyad;
                c.telefon = telefon;
                tblefirmaurun_kategorisi t = db.tblefirmaurun_kategorisi.FirstOrDefault(x => x.firmaid == musteriid);
                t.kategoriid = kategori;
                db.SaveChanges();
                sonuc = 1;

            }
            catch (Exception)
            {

                sonuc = 0;
            }
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }



        [Authorize]
        public ActionResult tedarikciler()
        {

            return View();
        }
        [Authorize]
        //----------------------------------------------------------- firmanın urun kategorileri bilgileri alınıyor----------------------
        public ActionResult _firmaurunkategorisi(int id)
        {
            List<cls_musteriler> liste = (from k in db.tblefirmaurun_kategorisi
                                          join i in db.tblkategoris on k.kategoriid equals i.kategoriID
                                          join m in db.tblCustomers on k.firmaid equals m.firmaid
                                          where m.firmaid == id
                                          select (new cls_musteriler { _urunkatid = (int)k.kategoriid, _urunkategori = i.kategoriadi })).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);

        }


        [Authorize]
        public ActionResult stokkarti()
        {
           List< tblkategori> urunkategorisi = db.tblkategoris.ToList();
            ViewBag.tedarikci = db.tblCustomers.Where(x => x.musteritipi == 0).ToList();
            
            return View(urunkategorisi);
        }
        [Authorize]
        //------------------------------------------------stok kartı olusturuluyor------------------------
        [HttpPost]
        public ActionResult stokkarti(FormCollection frm)
        {
            List<tblkategori> urunkategorisi = db.tblkategoris.ToList();
            ViewBag.tedarikci = db.tblCustomers.Where(x => x.musteritipi == 0).ToList();

            string urunadi = frm["ad"].ToUpper().ToString().Trim();
            tblurunler urun = db.tblurunlers.FirstOrDefault(x => x.urunadi ==urunadi );
            if(urun==null)
            {
                try
                {
                    tblurunler yeniurun = new tblurunler();
                    yeniurun.tedarikci_id =Convert.ToInt32( frm["tedarikci"].ToString().Trim());
                    yeniurun.urunadi = frm["ad"].ToUpper().Trim();
                    yeniurun.urun_fiyati = Convert.ToDecimal(frm["fiyat"].ToString().Trim());
                    yeniurun.urunkategori = Convert.ToInt32(frm["kategori"]);
                    db.tblurunlers.Add(yeniurun);
                   
                    db.SaveChanges();
                    tblurundetayları t = new tblurundetayları();
                    
                    ViewBag.mesaj = 1;
                }
                catch (Exception)
                {

                    ViewBag.mesaj = 2;
                }
            }
            else
            {
                ViewBag.mesaj = 0;
            }
            return View(urunkategorisi);
        }
        
        [Authorize]
        public ActionResult urunekle()
        {
          var  liste = db.tblurunlers.ToList().Take(10);
            List<tblurunler> t = new List<tblurunler>();
            foreach (var item in liste)
            {
                tblurunler n = new tblurunler();
                n =(tblurunler) item;
                t.Add(n); 
            }
           
            ViewBag.urunler = t;

            return View();

        }
        [Authorize]
        //---------------------------------------------yenı urun eklenıyor--------------------
        [HttpPost]
        public ActionResult urunekle(FormCollection frm)
        {
            int urunid = Convert.ToInt32(frm["urun"].ToString());
            tblurunler p = db.tblurunlers.FirstOrDefault(x => x.uruid == urunid);

            var liste = db.tblurunlers.ToList().Take(10);
            List<tblurunler> t = new List<tblurunler>();
            foreach (var item in liste)
            {
                tblurunler n = new tblurunler();
                n = (tblurunler)item;
                t.Add(n);
            }
            ViewBag.urunler = t;

            if(string.IsNullOrEmpty(frm["serino"].ToString())) //urunde serı numarası yoksa ekleme ıslemı ona yapılacak
            {
                try
                {
                    tblurundetayları yeniurun = db.tblurundetayları.FirstOrDefault(x=>x.urun_id==urunid);
                    if(yeniurun==null)
                    {
                        yeniurun = new tblurundetayları();
                        yeniurun.urun_id = Convert.ToInt32(frm["urun"].ToString());
                        yeniurun.urunserino = frm["serino"].ToString();
                        yeniurun.adet = Convert.ToInt32(frm["adet"].ToString());
                        db.tblurundetayları.Add(yeniurun);
                        db.SaveChanges();

                        tblurun_islem_Gecmisi u = new tblurun_islem_Gecmisi();
                        u.urun_detay_id = yeniurun.detay_id;
                        u.islem_tarihi = DateTime.Now.Date;
                        u.uruneskimusteri = p.tedarikci_id;
                        u.urunyenimusteri = 1007;
                        u.islemtipi = 1;
                        db.tblurun_islem_Gecmisi.Add(u);
                        db.SaveChanges();
                        ViewBag.mesaj = 1;
                    }
                    else
                    {
                        yeniurun.adet=yeniurun.adet+ Convert.ToInt32(frm["adet"].ToString());
                        tblurun_islem_Gecmisi u = new tblurun_islem_Gecmisi();
                        u.urun_detay_id = yeniurun.detay_id;
                        u.islem_tarihi = DateTime.Now.Date;
                        u.uruneskimusteri = p.tedarikci_id;
                        u.urunyenimusteri = 1007;
                        u.islemtipi = 1;
                        db.tblurun_islem_Gecmisi.Add(u);
                        db.SaveChanges();
                        ViewBag.mesaj = 1;
                    }
                    
                }
                catch (Exception)
                {

                    ViewBag.mesaj = 0;
                }
            }
            else
            {
                try
                {
                    string serino = frm["serino"].ToString().Trim();

                    tblurundetayları b = db.tblurundetayları.FirstOrDefault(x => x.urunserino==serino&&x.urun_id==urunid);

                    if(b==null) // girilen seri numarası eger stokta varsa ıkıncıye gırılmesı engellenıyor
                    {
                        tblurundetayları yeniurun = new tblurundetayları();
                        yeniurun.urun_id = Convert.ToInt32(frm["urun"].ToString());
                        yeniurun.urunserino = frm["serino"].ToString().ToUpper();
                        yeniurun.adet = Convert.ToInt32(frm["adet"].ToString());
                        db.tblurundetayları.Add(yeniurun);
                        db.SaveChanges();

                        tblurun_islem_Gecmisi u = new tblurun_islem_Gecmisi();
                        u.urun_detay_id = yeniurun.detay_id;
                        u.islem_tarihi = DateTime.Now.Date;
                        u.uruneskimusteri = p.tedarikci_id;
                        u.urunyenimusteri = 1007;
                        u.islemtipi = 1;
                        db.tblurun_islem_Gecmisi.Add(u);
                        db.SaveChanges();

                        ViewBag.mesaj = 1;
                    }
                    else
                    {   
                        ViewBag.mesaj = 2;
                    }
                   
                }
                catch (Exception)
                {

                    ViewBag.mesaj = 0;
                }
               
            }
            

         
            return View();
        }
        [Authorize]
        //------------------------------------------------------------gelen veri turune arama yapılıyor deger ınt ıse uruıd uzerınden strıng ıse urunadı uzerınden ıslem yapılıyor
        public ActionResult _urunarama(string name)
        {
            name = name.Trim();
            List<tblurunler> liste = new List<tblurunler>();
            try
            {
                int id = Convert.ToInt32(name);
                liste = db.tblurunlers.Where(x => x.uruid == id).ToList();
            }
            catch (Exception)
            {
                if(string.IsNullOrEmpty(name))
                {
                    var urunler = db.tblurunlers.ToList().Take(10);
                    
                    foreach (var item in urunler)
                    {
                        tblurunler n = new tblurunler();
                        n = (tblurunler)item;
                        liste.Add(n);
                    }
                }
                else
                {
                    liste = db.tblurunlers.Where(x => x.urunadi.StartsWith(name.ToUpper()) || x.urunadi.Contains(name.ToUpper())).ToList();
                }
              
            }
          
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
        [Authorize]


        //------------------------------------------------------ depoda bulunan urunlerın kategorı lıstesı----------------------------------
        public ActionResult _urunkategorileri()
        {
            List<tblkategori> liste = db.tblkategoris.ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult urunlistesi()
        {
           List< cls_urunler> urunler = new List<cls_urunler>();
            foreach (var item in db.tblurunlers)
            {
                List<tblurundetayları> liste = db.tblurundetayları.Where(x => x.urun_id == item.uruid).ToList();
                if(liste.Count()>0)
                {
                    int adet = 0;
                    cls_urunler p = new cls_urunler();
                    p._stokkodu = item.uruid;
                    p._urunadi = item.urunadi;
                    foreach (var item2 in liste)
                    {
                        adet +=(int) item2.adet;
                    }
                    p._urunadedi = adet;
                    urunler.Add(p);
                }
                else
                {
                    cls_urunler p = new cls_urunler();
                    p._stokkodu = item.uruid;
                    p._urunadi = item.urunadi;                   
                    p._urunadedi = 0;
                    urunler.Add(p);
                }
            }

            return View(urunler);
        }
       
        [Authorize]
        public ActionResult uretimecikis()
        {
            List<Uruntakip.Models.cls_makinalar> liste = (from m in db.tblmakinas
                                                          join c in db.tblCustomers on m.musteri_id equals c.firmaid
                                                          join t in db.tblmakinatipis on m.makinatip_id equals t.tipid where c.firmaid==1007
                                                          select (new Uruntakip.Models.cls_makinalar 
                                                          { _firmaadi = c.firmaadi, _makinaserino = m.serino, _makinatipi = t.makinatipi, _makinaid = m.makinaid })).ToList();
                         return View(liste);
        }
        [Authorize]
        [HttpPost]
        public ActionResult uretimecikis(FormCollection frm)
        {
            return View();
        }
        [Authorize]

        public double kurcevir(string birim,double tutar)
        {
            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldoc = new XmlDocument();
            xmldoc.Load(bugun);
            string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;
            string eur=  xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/BanknoteSelling").InnerXml;
            usd = usd.Replace('.', ',');
            eur = eur.Replace('.', ',');
            double dolar = Convert.ToDouble(usd);
            double euro = Convert.ToDouble(eur);
            double deger = 0;
            switch (birim)
            {
                case "DOLAR":
                    deger = tutar * (euro / dolar);
                    break;
                case "TL":
                    deger = tutar * euro;
                    break;
            }
            deger = Math.Round(deger, 3);
            return deger;
        }
        [Authorize]

        public ActionResult teklifurunu(string name,string teklifno,string kontrol)
        {
            name = name.ToUpper().Trim();
            string parabirimi = "";
            if (kontrol == "1")
            {
                int id = Convert.ToInt32(teklifno);
                var teklif = from t in db.tblteklifs join p in db.tblparabirimleris on t.parabirimi equals p.paraid where t.teklifid == id select new { p.parabirimi };
                foreach (var item in teklif)
                {
                    parabirimi = item.parabirimi;
                }
            }
            else
            {
               var  teklif = from t in db.tblteklifs join p in db.tblparabirimleris on t.parabirimi equals p.paraid where t.teklifno == teklifno select new { p.parabirimi };

                foreach (var item in teklif)
                {
                    parabirimi = item.parabirimi;
                }


            }       
                    
                    
                    
                    
                 
            List < cls_urunler > liste = new List<cls_urunler>();
            try
            {
                int id = Convert.ToInt32(name);
                //tblurundetayları urunkontrol = db.tblurundetayları.FirstOrDefault(x => x.urun_id == id);
                List<tblurunler> urunler = db.tblurunlers.Where(t => t.uruid == id || t.urunadi.Contains(name)).ToList();

                //if (urunkontrol==null)
                //{
                //    var arananurun = from p in db.tblurunler join k in db.tblkategori on p.urunkategori equals k.kategoriID where p.uruid==id select new { p.uruid, p.urun_fiyati, k.carpan, p.urunadi };
                //    foreach (var item in arananurun)
                //    {
                //        cls_urunler u = new cls_urunler();
                //        u._stokkodu = item.uruid;
                //        u.urunfiyati = Convert.ToDouble(item.urun_fiyati * item.carpan);
                //        u._urunadi = item.urunadi;
                //        u._urunadedi = 0;
                //        liste.Add(u);

                //    }
                    

                //}
                //else
                //{
                //    List<tblurundetayları> t = db.tblurundetayları.Where(x => x.urun_id == id && x.urunserino != null).ToList();
                //    if (t.Count() > 0)  // serı numaralı urunse lıste degerı 0 dan buyuk olursa adet sayısı lıste uzunluguna esıt olur
                //    {
                //        int adet = t.Count();
                //        liste = (from p in db.tblurunler
                //                 join k in db.tblkategori on p.urunkategori equals k.kategoriID
                //                 where p.uruid == id
                //                 select (new cls_urunler { _stokkodu = p.uruid, urunfiyati = (double)(p.urun_fiyati * k.carpan), _urunadi = p.urunadi, _urunadedi = adet })).ToList();
                //    }
                //    else // serı numaralı urun degılse count 0 olur ve adet tutarı tblurundetaylarından alınır
                //    {
                //        liste = (from p in db.tblurunler
                //                 join d in db.tblurundetayları on p.uruid equals d.urun_id
                //                 join k in db.tblkategori on p.urunkategori equals k.kategoriID
                //                 where p.uruid == id
                //                 select (new cls_urunler { _stokkodu = p.uruid, urunfiyati = (double)(p.urun_fiyati * k.carpan), _urunadi = p.urunadi, _urunadedi = (int)d.adet })).ToList();
                //    }

                //}
                

            }
            catch (Exception)
            {
                name = name.Trim().Replace('i','ı').Replace('ö', 'o').Replace('ü', 'u').Replace('ğ', 'g').Replace('ç', 'c').Replace('ş', 's');
                //List<tblurunler> product = db.tblurunler.Where(x => x.urunadi.StartsWith(name)&&x.urunadi.EndsWith(name)&x.urunadi.Contains(name)).ToList();
                List<tblurunler> product = db.tblurunlers.ToList();


                foreach (tblurunler item in product)
                {
                    if (item.urunadi.StartsWith(name) || item.urunadi.Contains(name))
                    {
                        tblurundetayları urunkontrol = db.tblurundetayları.FirstOrDefault(x => x.urun_id == item.uruid);  //urun stok bılgısı gırılmemısse detaylar kısmından urun degerı null doner
                        tblkategori kategori = db.tblkategoris.FirstOrDefault(x => x.kategoriID == item.urunkategori);
                        if (urunkontrol == null) // stok bılgısı gırılmeyen urunun adedını otomatık 0 kabul edıyoruz
                        {
                            cls_urunler u = new cls_urunler();
                            u._stokkodu = item.uruid;
                            u.urunfiyati = Convert.ToDouble(item.urun_fiyati * kategori.carpan);
                            u._urunadi = item.urunadi;
                            u._urunadedi = 0;
                            liste.Add(u);
                        }
                        else
                        {
                            List<tblurundetayları> t = db.tblurundetayları.Where(x => x.urun_id == item.uruid && x.urunserino != null).ToList();
                            if (t.Count() > 0)  // serı numaralı urunse lıste degerı 0 dan buyuk olur ve adet sayısı lıste uzunluguna esıt olur
                            {
                                int adet = t.Count();


                                var urun = from p in db.tblurunlers
                                           join k in db.tblkategoris on p.urunkategori equals k.kategoriID
                                           where p.uruid == item.uruid
                                           select new { p.uruid, p.urun_fiyati, p.urunadi, k.carpan };
                                foreach (var p in urun)
                                {
                                    cls_urunler u = new cls_urunler();
                                    u._stokkodu = p.uruid;
                                    u.urunfiyati = Convert.ToDouble(p.urun_fiyati * p.carpan);
                                    u._urunadi = p.urunadi;
                                    u._urunadedi = adet;
                                    liste.Add(u);
                                }

                            }
                            else // serı numaralı urun degılse count 0 olur ve adet tutarı tblurundetaylarından alınır
                            {
                                var urun = from p in db.tblurunlers
                                           join d in db.tblurundetayları on p.uruid equals d.urun_id
                                           join k in db.tblkategoris on p.urunkategori equals k.kategoriID
                                           where p.uruid == item.uruid
                                           select new { p.uruid, p.urun_fiyati, p.urunadi, d.adet, k.carpan };
                                foreach (var p in urun)
                                {
                                    cls_urunler u = new cls_urunler();
                                    u._stokkodu = p.uruid;
                                    u.urunfiyati = Convert.ToDouble(p.urun_fiyati * p.carpan);
                                    u._urunadi = p.urunadi;
                                    u._urunadedi = (int)p.adet;
                                    liste.Add(u);
                                }
                            }

                        }
                    }
                    


                   
                }
                
            }
            
                switch (parabirimi)
                {
                    case "DOLAR":
                        foreach (cls_urunler tt in liste)
                        {
                            tt.parabirimi = "DOLAR";
                            tt.urunfiyati = kurcevir("DOLAR",tt.urunfiyati);
                        }
                        break;
                   
                    case "TÜRK LİRASI":
                        foreach (cls_urunler tt in liste)
                        {
                            tt.parabirimi = "TL";
                            tt.urunfiyati =kurcevir("TL",tt.urunfiyati);
                        }
                        break;
                    case "EURO":
                        foreach (cls_urunler tt in liste)
                        {
                            tt.parabirimi = "EURO";
                           
                        }
                        break;
                }

            
           return Json(liste, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public ActionResult urunteklifgecmisi(int id)
        {
            List<cls_teklif> liste = (from m in db.tblCustomers
                                      join t in db.tblteklifs on m.firmaid equals t.musteri_id
                                      join tu in db.tblteklif_urunler on t.teklifid equals tu.teklif_id
                                      join p in db.tblparabirimleris on t.parabirimi equals p.paraid
                                      where tu.urun_id==id
                                      select (new cls_teklif { firmaadi = m.firmaadi, tarih = t.tarih.ToString(), fiyat = (double)tu.birimfiyat, parabirimi = p.parabirimi })).ToList();
            foreach (cls_teklif item in liste)
            {
                item.tarih = tarihduzelt_gun_ay_yıl(item.tarih);
            }
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
    }
}