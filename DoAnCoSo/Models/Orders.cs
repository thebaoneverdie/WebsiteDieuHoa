namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            Order_Details = new HashSet<Order_Details>();
            Order_Details1 = new HashSet<Order_Details>();
        }

        public int id { get; set; }

        public int? user_id { get; set; }

        [StringLength(50)]
        public string fullname { get; set; }

        [StringLength(150)]
        public string email { get; set; }

        [StringLength(20)]
        public string phone_number { get; set; }

        [StringLength(200)]
        public string address { get; set; }

        [StringLength(1000)]
        public string note { get; set; }

        public DateTime? order_date { get; set; }

        public bool? isPaid { get; set; }

        public bool? isComplete { get; set; }

        public int? total_money { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Details> Order_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Details> Order_Details1 { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
