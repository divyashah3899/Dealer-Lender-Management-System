using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DLMS.Models
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int c_id { get; set; }
        public int? d_id { get; set; }
        [Required(ErrorMessage = "Car manufacturer field is required.")]
        [Display(Name ="Car manufacturer :")]
        public String car_manufacturer { get; set; }
        [Required(ErrorMessage = "Car model field is required.")]
        [Display(Name = "Car Model :")]
        public String car_model { get; set; }
        [Required(ErrorMessage = "Car type field is required.")]
        [Display(Name = "Car Type :")]
        public String car_type { get; set; }
        [Required(ErrorMessage = "Price field is required.")]
        [Display(Name = "Price :")]
        public int price { get; set; }
        [Required(ErrorMessage = "Power field is required.")]
        [Display(Name = "Power :")]
        public int power { get; set; }
        [Required(ErrorMessage = "Fuel Tank Capacity field is required.")]
        [Display(Name = "Fuel Tank Capacity :")]
        public int fuel_tank { get; set; }
        [Required(ErrorMessage = "Airbag field is required.")]
        [Display(Name = "Airbag :")]
        public String airbag { get; set; }
        [Required(ErrorMessage = "Gear Type field is required.")]
        [Display(Name = "Gear Type :")]
        public String gear { get; set; }
        [Required(ErrorMessage = "Mileage field is required.")]
        [Display(Name = "Mileage :")]
        public int mileage { get; set; }
        public string img { get; set; }
        //[Required(ErrorMessage = "Image is Required.")]
        [Display(Name = "Image :")]
      //  [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif|.jpeg)$", ErrorMessage = "Only Image files allowed.")]
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public virtual ICollection<LoanApproved> LoanApproveds { get; set; }
        public virtual ICollection<FullPayment> FullPayments { get; set; }
        public virtual ICollection<LoanRequest> LoanRequests { get; set; }
        [ForeignKey("d_id")]
        public virtual Dealer Dealer { get; set; }
    }
}