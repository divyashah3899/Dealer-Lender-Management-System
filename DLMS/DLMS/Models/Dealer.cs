using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLMS.Models
{
    public class Dealer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int d_id { get; set; }
        [Required(ErrorMessage = "Company Name field is required.")]
        [Display(Name = "Company: ")]
        public String company_name { get; set; }
        [Required(ErrorMessage = "Address field is required.")]
        [Display(Name = "Address: ")]
        public String d_address { get; set; }
        [Required(ErrorMessage = "Email Address field is required.")]
        [Display(Name = "Email: ")]
        [EmailAddress]
        public String email { get; set; }
        [Required(ErrorMessage = "Contact Number field is required.")]
        [Display(Name = "Contact No.: ")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid mobile number")]
        public String contact_no { get; set; }
        [Required(ErrorMessage = "Password field is required.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^((?=.*[A-Z])(?=.*\d)(?=.*[a-z])|(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%&\/=?_.-])|(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%&\/=?_.-])|(?=.*\d)(?=.*[a-z])(?=.*[!@#$%&\/=?_.-])).{7,15}$", ErrorMessage = "Use a strong password with numbers,alphabets and symbols.")]
        [Display(Name = "Password: ")]
        public String password { get; set; }
        public virtual ICollection<LoanApproved> LoanApproveds { get; set; }
        public virtual ICollection<FullPayment> FullPayments { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<LoanRequest> LoanRequests { get; set; }



    }
}