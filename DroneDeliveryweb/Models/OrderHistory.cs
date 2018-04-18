using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DroneDeliveryweb.Models
{
    public class OrderHistory
    {
        [Key] 
        public int Orderid { get; set; }
        public Customer Customer { get; set; }
        public Coordinates ToCoordinates { get; set; }


    }
}