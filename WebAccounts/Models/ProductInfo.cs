using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Installments.Models
{
    public class ProductInfo
    {
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Alternate Title")]
        public string AlternateTitle { get; set; }
       
        public decimal Price { get; set; }
        [Display(Name = "Purchase Price")]
        [Required]
        public decimal PurchasePrice { get; set; }

        [Required]
        public decimal Discount { get; set; }
        public decimal DiscountPrice { get
            {

                return (Price * Discount) / 100;

            } }
        public decimal SaleWithDiscountPrice
        {
            get
            {
                return Price-DiscountPrice;
            }
        }

        public string Ampere { get; set; }
        public bool InActive { get; set; }
        [Display(Name = "For UPS")]
        public bool UPS { get; set; }
        public string Description { get; set; }
        [Display(Name = "Product Image")]
        public string ProductImage { get; set; }
        public HttpPostedFileBase Imagefile { get; set; }
        [Display(Name = "Category")]
        public int CatagoryID { get; set; }

        [Display(Name = "Sale Account")]
        [Required]
        public int SalesAccountID { get; set; }
        [Display(Name = "Purchase Account")]
        [Required]
        public int PurchaseAccountID { get; set; }

        [Display(Name = "Category")]
        public string CategoryTitle { get; set; }

        [Display(Name = "Tag")]
        public string TagTitle { get; set; }

        [Display(Name = "Sale Account")]
        public string SalesAccountTitle { get; set; }
        [Display(Name = "Purchase Account")]
        public string PurchaseAccountTitle { get; set; }
        public List<ProductVariantAssociation> lstProductVariantAssociation = new List<ProductVariantAssociation>();
        public List<ProductBarcodes> lstProductBarcodes = new List<ProductBarcodes>();
        public List<ProductColorAssociation> lstProductColorAssociations = new List<ProductColorAssociation>();
        public List<ProductSizeAssociation> lstProductSizeAssociations = new List<ProductSizeAssociation>();
        public List<ProductTagAssociation> lstproductTagAssociation = new List<ProductTagAssociation>();
        public List<ProductTandCAssociation> lstTandCAssociation = new List<ProductTandCAssociation>();
        public int DefaultVariant { get; set; }
        public int ColourID { get; set; }
        public int SizeID { get; set; }
        public int TagsID { get; set; }
        public int WarehouseID { get; set; }
        public int BranchID { get; set; }
        public String Warehouse { get; set; }
        public String Branch { get; set; }
        public decimal OpeningStock { get; set; }
        public DateTime EffectiveDate { get; set; }
        
        public int TandCID { get; set; }

        [Display(Name = "Product Type")]
        public int ProductType { get; set; }
        public string Dimension { get; set; }
        [Display(Name = "No of Plates")]
        public int NoofPlates { get; set; }
        public int Weight { get; set; }
        [Display(Name = "Total Capacity")]
        public int TotalCapacity { get; set; }
        [Display(Name = "Solar Capacity")]
        public int SolarCapacity { get; set; }
        [Display(Name = "Charger Capacity")]
        public int BatteryChargerCapacity { get; set; }
        [Display(Name = "Maximum Power")]
        public int PMax { get; set; }
        [Display(Name = "Volate at Power Max")]
        public int VMax { get; set; }
        [Display(Name = "Current at Power Max")]
        public int IMP { get; set; }
        [Display(Name = "Short Circuit Current")]
        public int ISC { get; set; }
        [Display(Name = "Open Circuit Voltage")]
        public int OCV { get; set; }
        [Display(Name = "Max System Voltage")]
        public int MSV { get; set; }
        [Display(Name = "Temperature Operating from")]
        public int TemperatureOperatingfrom { get; set; }
        [Display(Name = "Temperature Operating To")]
        public int TemperatureOperatingTo { get; set; }

    }
}