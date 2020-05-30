using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DLMS.Models
{
    public class DLMSDb:DbContext
    {
        public DLMSDb():base("name=DLMSDb")
        { }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Dealer> Dealer { get; set; }
        public virtual DbSet<Lender> Lender { get; set; }
        public virtual DbSet<Car> Car { get; set; }
        public virtual DbSet<LoanApproved> LoanApproved { get; set; }
        public virtual DbSet<LoanRequest> LoanRequest { get; set; }
        public virtual DbSet<FullPayment> FullPayment { get; set; }

    }
}