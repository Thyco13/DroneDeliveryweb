using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DroneDeliveryweb.Models
{
    public class Coordinates
    {
        [Required]
        [Display(Name = "Address:")]
        public string FromAddress { get; set; }
        [Required]
        [Display(Name = "City:")]
        public string FromCity { get; set; }
        [Required]
        [Display(Name = "Zip:")]
        public int FromZip { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string ToStreet { get; set; }
        [Required]
        [Display(Name = "City:")]
        public string ToCity { get; set; }
        [Required]
        [Display(Name = "Zip:")]
        public int ToZip { get; set; }

        public string Fromlat { get; set; }
        public string Fromlng { get; set; }
        public string ToLat { get; set; }
        public string ToLng { get; set; }

        public string Distance { get; set; }

        public string Weather { get; set; }

        public string Weight { get; set; }
        public string Price { get; set; }

    }
    }
