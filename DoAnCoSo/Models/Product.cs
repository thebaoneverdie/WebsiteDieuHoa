namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Galery = new HashSet<Galery>();
            Galery1 = new HashSet<Galery>();
            Order_Details = new HashSet<Order_Details>();
            Order_Details1 = new HashSet<Order_Details>();
        }

        public int id { get; set; }

        public int? category_id { get; set; }

        [StringLength(250)]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string title { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        [Range(minimum: 1000, maximum: 1000000000, ErrorMessage = "Giá ti?nn t? 1 ngàn đ?n 100 tri?u")]
        public int? price { get; set; }
        public string FormattedAmount =>
        string.Format("{0:N0} VND", price);

        public int? discount { get; set; }

        [StringLength(500)]
        public string thumbnail { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }

        public int? deleted { get; set; }

        public virtual Category Category { get; set; }

        public virtual Category Category1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Galery> Galery { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Galery> Galery1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Details> Order_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Details> Order_Details1 { get; set; }
    }
}
