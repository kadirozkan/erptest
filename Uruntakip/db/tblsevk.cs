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
    
    public partial class tblsevk
    {
        public int sevkid { get; set; }
        public Nullable<int> musteri_id { get; set; }
        public Nullable<System.DateTime> sevktarihi { get; set; }
        public string sevknotu { get; set; }
        public Nullable<int> odemetipi { get; set; }
        public Nullable<int> durum { get; set; }
    }
}
