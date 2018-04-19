using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DroneDeliveryweb.Models
{
    public class Viewmodel
    {
        public Viewmodel()
        {
            Customer = new Customer();
            Coordinates = new Coordinates();
            OrderHistory = new OrderHistory();
        }


        public OrderHistory OrderHistory { get; set; }

        public Customer Customer { get; set; }
        public Coordinates Coordinates { get; set; }
    }
}