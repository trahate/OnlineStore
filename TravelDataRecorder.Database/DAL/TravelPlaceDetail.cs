//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TravelDataRecorder.Database.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class TravelPlaceDetail
    {
        public int Id { get; set; }
        public int TravelId { get; set; }
        public string Type { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
    
        public virtual TravelCity TravelCity { get; set; }
        public virtual TravelState TravelState { get; set; }
        public virtual TravelDetail TravelDetail { get; set; }
    }
}
