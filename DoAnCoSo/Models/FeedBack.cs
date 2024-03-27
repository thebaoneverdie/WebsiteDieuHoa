namespace DoAnCoSo.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FeedBack")]
    public partial class FeedBack
    {
        public int id { get; set; }

        [StringLength(30)]
        public string firstname { get; set; }

        [StringLength(30)]
        public string lastname { get; set; }

        [StringLength(250)]
        public string email { get; set; }

        [StringLength(20)]
        public string phone_number { get; set; }

        [StringLength(350)]
        public string subject_name { get; set; }

        [StringLength(1000)]
        public string note { get; set; }

        public bool? isPaid { get; set; }

        public bool? isComplete { get; set; }

        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }
    }
}
