using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLMS.Models
{
    public class LoanApproved
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int la_id { get; set; }
        public int? u_id { get; set; }
        public int? d_id { get; set; }
        public int? c_id { get; set; }
        public int? lender_id { get; set; }
        public int? loan_id { get; set; }

        public String status { get; set; }

        [ForeignKey("u_id")]
        public virtual User User { get; set; }
        [ForeignKey("d_id")]
        public virtual Dealer Dealer { get; set; }
        [ForeignKey("c_id")]
        public virtual Car Car { get; set; }
        [ForeignKey("lender_id")]
        public virtual Lender Lender { get; set; }
        [ForeignKey("loan_id")]
        public virtual LoanRequest LoanRequest { get; set; }

    }
}