using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLMS.Models
{
    public class Lender
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int lender_id { get; set; }
        [Required(ErrorMessage = "Bank Name field is required.")]
        public String bankname { get; set; }
        [Required(ErrorMessage = "Branch Name field is required.")]
        public String branchname { get; set; }
        [Required(ErrorMessage = "IFSC field is required.")]
        public String ifsc { get; set; }
        [Required(ErrorMessage = "Email Address field is required.")]
        [Display(Name = "Email: ")]
        [EmailAddress]
        public String b_email { get; set; }
        [Required(ErrorMessage = "Mobile Number field is required.")]
        [Display(Name = "Mobile Number: ")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid mobile number")]
        public String contact_no { get; set; }
        [Required(ErrorMessage = "Password field is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^((?=.*[A-Z])(?=.*\d)(?=.*[a-z])|(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%&\/=?_.-])|(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%&\/=?_.-])|(?=.*\d)(?=.*[a-z])(?=.*[!@#$%&\/=?_.-])).{7,15}$", ErrorMessage = "Use a strong password with numbers,alphabets and symbols.")]
        [Display(Name = "Password: ")]
        public String password { get; set; }

        public virtual ICollection<LoanApproved> LoanApproveds { get; set; }
        public virtual ICollection<LoanRequest> LoanRequests { get; set; }
    }
}