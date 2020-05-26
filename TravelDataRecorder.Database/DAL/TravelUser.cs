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
    
    public partial class TravelUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TravelUser()
        {
            this.TravelUserRoleMappings = new HashSet<TravelUserRoleMapping>();
            this.TravelNotifications = new HashSet<TravelNotification>();
            this.TravelDetails = new HashSet<TravelDetail>();
            this.TravelNotifications1 = new HashSet<TravelNotification>();
        }
    
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Nullable<long> ContactNumber { get; set; }
        public Nullable<short> IsActive { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string Address { get; set; }
        public string OTP { get; set; }
        public Nullable<System.Guid> GeneratePasswordKey { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelUserRoleMapping> TravelUserRoleMappings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelNotification> TravelNotifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelDetail> TravelDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TravelNotification> TravelNotifications1 { get; set; }
    }
}
