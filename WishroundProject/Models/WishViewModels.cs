using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WishroundProject.Models
{
    public class CreateWishModel
    {
        [Required]
        [DataType(DataType.Url)]
        [Display(Name = "URL")]
        public string URL { get; set; }
    }

    public class ConfirmWish
    {
        [Required]
        [DataType(DataType.Url)]
        public string URL { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public float Cost { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public string Currency { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; }
    }

    public class ConfirmWishModel : ConfirmWish
    {
        [Required]
        public bool IsClientParsing { get; set; }
    }

    public class WishPreview
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Currency { get; set; }
        public string ImageUrl { get; set; }
    }
    public class AllWishesModel
    {
        public List<WishPreview> Wishes { get; set; }
    }
}