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
    
    public partial class tblurunler
    {
        public int uruid { get; set; }
        public Nullable<int> urunkategori { get; set; }
        public string urunadi { get; set; }
        public Nullable<decimal> urun_fiyati { get; set; }
        public Nullable<int> tedarikci_id { get; set; }
    }
}