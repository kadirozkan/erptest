﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Uruntakip.db
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class uruntakipdbEntities6 : DbContext
    {
        public uruntakipdbEntities6()
            : base("name=uruntakipdbEntities6")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tblarizaislemleri> tblarizaislemleris { get; set; }
        public virtual DbSet<tblarizakategorileri> tblarizakategorileris { get; set; }
        public virtual DbSet<tblarizalar> tblarizalars { get; set; }
        public virtual DbSet<tblarızasonuctipleri> tblarızasonuctipleri { get; set; }
        public virtual DbSet<tblCustomer> tblCustomers { get; set; }
        public virtual DbSet<tblefirmaurun_kategorisi> tblefirmaurun_kategorisi { get; set; }
        public virtual DbSet<tblkategori> tblkategoris { get; set; }
        public virtual DbSet<tbllogin> tbllogins { get; set; }
        public virtual DbSet<tblmakina> tblmakinas { get; set; }
        public virtual DbSet<tblmakinatipi> tblmakinatipis { get; set; }
        public virtual DbSet<tblparabirimleri> tblparabirimleris { get; set; }
        public virtual DbSet<tblsevk> tblsevks { get; set; }
        public virtual DbSet<tblsevkdetay> tblsevkdetays { get; set; }
        public virtual DbSet<tblsevkiyat> tblsevkiyats { get; set; }
        public virtual DbSet<tblteklif> tblteklifs { get; set; }
        public virtual DbSet<tblteklif_urunler> tblteklif_urunler { get; set; }
        public virtual DbSet<tblteslimattipleri> tblteslimattipleris { get; set; }
        public virtual DbSet<tblurun_islem_Gecmisi> tblurun_islem_Gecmisi { get; set; }
        public virtual DbSet<tblurundetayları> tblurundetayları { get; set; }
        public virtual DbSet<tblurunharekettipi> tblurunharekettipis { get; set; }
        public virtual DbSet<tblurunler> tblurunlers { get; set; }
        public virtual DbSet<vwyeniteklif> vwyeniteklifs { get; set; }
    }
}
