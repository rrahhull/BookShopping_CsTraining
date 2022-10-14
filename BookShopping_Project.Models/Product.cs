using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopping_Project.Models
{
   public class Product
    {
        public int id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1,10000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1,10000)]
        public double price50 { get; set; }
        [Required]
        [Range(1,10000)]
        public double price100 { get; set; }
        [Required]
        [Range(1,10000)]
        public double price { get; set; }
        [Display(Name ="image url")]
        public string ImageUrl { get; set; }
        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId") ]
        public Category Category { get; set; }
        [Display (Name ="CoverType")]
        public int CoverTypeId { get; set; }
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType { get; set; }

    }
}
