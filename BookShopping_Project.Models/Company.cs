using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Display(Name ="Postal Code")]
        [Required]
        public string PostalCode { get; set; }
        [Display(Name ="Phone No")]
        [Required]
        public string PhoneNo { get; set; }
        [Display(Name ="Is Authorized Company")]
        public bool IsAuthorizedCompany { get; set; }
    }
}
