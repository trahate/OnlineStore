﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TravelDataRecoderEntities : DbContext
    {
        public TravelDataRecoderEntities()
            : base("name=TravelDataRecoderEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TravelCity> TravelCities { get; set; }
        public virtual DbSet<TravelCountry> TravelCountries { get; set; }
        public virtual DbSet<TravelEmailException> TravelEmailExceptions { get; set; }
        public virtual DbSet<TravelErrorLog> TravelErrorLogs { get; set; }
        public virtual DbSet<TravelPlaceDetail> TravelPlaceDetails { get; set; }
        public virtual DbSet<TravelRole> TravelRoles { get; set; }
        public virtual DbSet<TravelState> TravelStates { get; set; }
        public virtual DbSet<TravelStatusMaster> TravelStatusMasters { get; set; }
        public virtual DbSet<TravelUser> TravelUsers { get; set; }
        public virtual DbSet<TravelUserRoleMapping> TravelUserRoleMappings { get; set; }
        public virtual DbSet<TravelNotification> TravelNotifications { get; set; }
        public virtual DbSet<TravelDetailTrail> TravelDetailTrails { get; set; }
        public virtual DbSet<TravelDetail> TravelDetails { get; set; }
        public virtual DbSet<EmailException> EmailExceptions { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
    }
}
