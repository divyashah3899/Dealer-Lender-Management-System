using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLMS.Models
{
    public class LoanRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int loan_id { get; set; }
        [Display(Name = "Bank Name :")]
        public int? lender_id { get; set; }
        [Required(ErrorMessage = "Downpayment field is Required.")]
        [Display(Name = "Downpayment :")]
        public int downpayment { get; set; }
        public int? u_id { get; set; }
        public int? c_id { get; set; }
        public int? d_id { get; set; }
        [Required(ErrorMessage = "Loan Period field is Required.")]
        [Display(Name = "Loan Period :")]
        public String loan_period { get; set; }
        [Required(ErrorMessage = "EMI field is Required.")]
        [Display(Name = "EMI :")]
        public int emi { get; set; }
        [Required(ErrorMessage = "User Address field is Required.")]
        [Display(Name = "User Address :")]
        public String user_address { get; set; }
        public String id_proof { get; set; }
        public String income_certificate { get; set; }
        public String income_tax_return { get; set; }
        public String address_proof { get; set; }
        [Required(ErrorMessage ="Guarantor Nominal Name field is Required.")]
        [Display(Name ="Guarantor Nominal Name :")]
        public String gnn { get; set; }
        [Required(ErrorMessage = "Guarantor Nominal Address field is Required.")]
        [Display(Name = "Guarantor Nominal Address :")]
        public String gnn_address { get; set; }
        public String gnn_address_p { get; set; }
        [NotMapped]
        [Display(Name ="Id Proof :")]
        public HttpPostedFileBase id_proof_file { get; set; }
        [NotMapped]
        [Display(Name = "Income Certificate :")]
        public HttpPostedFileBase income_certificate_file { get; set; }
        [NotMapped]
        [Display(Name = "Income Tax Return :")]
        public HttpPostedFileBase income_tax_return_file { get; set; }
        [NotMapped]
        [Display(Name = "Address Proof :")]
        public HttpPostedFileBase address_proof_file { get; set; }
        [NotMapped]
        [Display(Name = "Guarantor Nominal Address Proof :")]
        public HttpPostedFileBase gnn_address_file { get; set; }

        [ForeignKey("lender_id")]
        public virtual Lender Lender { get; set; }
        [ForeignKey("u_id")]
        public virtual User User { get; set; }
        [ForeignKey("d_id")]
        public virtual Dealer Dealer { get; set; }
        [ForeignKey("c_id")]
        public virtual Car Car { get; set; }
        public virtual ICollection<LoanApproved> LoanApproveds { get; set; }
    }
}