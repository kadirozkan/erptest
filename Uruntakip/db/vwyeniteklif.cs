//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class vwyeniteklif
    {
        public int teklifid { get; set; }
        public string teklifno { get; set; }
        public Nullable<System.DateTime> tarih { get; set; }
        public string teslimat_notu { get; set; }
        public string gdr_adres { get; set; }
        public string gdr_email { get; set; }
        public string parabirimi { get; set; }
        public string sembol { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string email { get; set; }
        public string telefon { get; set; }
        public string adres { get; set; }
        public string firmaadi { get; set; }
        public Nullable<int> urun_id { get; set; }
        public string urunadi { get; set; }
        public Nullable<decimal> birimfiyat { get; set; }
        public Nullable<int> urunadeti { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> iskonto { get; set; }
    }
}