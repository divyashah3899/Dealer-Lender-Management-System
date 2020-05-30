using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLMS.Models
{
    public class FullPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int fp_id { get; set; }
        public int? u_id { get; set; }
        public int? d_id { get; set; }
        public int? c_id { get; set; }
        [Required(ErrorMessage ="Advance Token field is required.")]
        [Display(Name ="Advance Token :")]
        public int downpayment { get; set; }
        [Required(ErrorMessage ="Address field is required.")]
        [Display(Name ="Address :")]
        public String user_address { get; set; }
        [Required(ErrorMessage ="Payment Mode is required.")]
        [Display(Name ="Payment Mode :")]
        public String mode { get; set; }

        [ForeignKey("u_id")]
        public virtual User User { get; set; }
        [ForeignKey("d_id")]
        public virtual Dealer Dealer { get; set; }
        [ForeignKey("c_id")]
        public virtual Car Car { get; set; }

    }
}