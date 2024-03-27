namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Orders = new HashSet<Orders>();
            Orders1 = new HashSet<Orders>();
            Tokens = new HashSet<Tokens>();
            Tokens1 = new HashSet<Tokens>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string fullname { get; set; }

        [StringLength(150)]
        [Required(ErrorMessage = "Vui l?ng nh?p email ho?c sai ð?nh d?ng email")]
        [EmailAddress]
        public string email { get; set; }

        [StringLength(20)]
        [Phone]
        public string phone_number { get; set; }

        [StringLength(200)]
        public string address { get; set; }

        [StringLength(32)]
        [Required(ErrorMessage = "Vui l?ng nh?p m?t kh?u")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "M?t kh?u ph?i ch?a ít nh?t 8 k? t? và ít nh?t m?t ch? cái và m?t ch? s?")]
        public string password { get; set; }

        public int? role_id { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public int? deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders1 { get; set; }

        public virtual Role Role { get; set; }

        public virtual Role Role1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tokens> Tokens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tokens> Tokens1 { get; set; }
    }
}
