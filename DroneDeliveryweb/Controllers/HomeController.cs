using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using DroneDeliveryweb.helpers;
using DroneDeliveryweb.Models;
using EASendMail;

namespace DroneDeliveryweb.Controllers
{
    [Authorize]
    
    public class HomeController : Controller
    {
        private DroneDeilveryContext db = new DroneDeilveryContext();
        
        public ActionResult Index()
        {

            return View(new Viewmodel());
        }

        [HttpPost]
        public ActionResult GetCustomerData(int id)
        {
            var customer = db.Customers.FirstOrDefault(i => i.Id == id);
            if (customer != null)
                // model.Customer = 
                return View("Index", new Viewmodel { Customer = customer });
            // return View(model.Customer.Name, model.Customer.LastName,model.Customer.Email, model.Customer.Street, model.Customer.City, model.Customer.Zip, model.Customer.Id);
            return View("Index", new Viewmodel());
               
        }
        
        [HttpPost]
        public ActionResult GetToCoordinates(string fromStreet, string fromCity, int fromZip, string toStreet, string toCity, int toZip, int customerId, string weight)//[Bind(Include = "FromAddress,FromCity,FromZip,ToAddress,ToCity,ToZip")] Customer customer)
        {
            var customer = db.Customers.FirstOrDefault(i => i.Id == customerId);

            string tolat = null;
            string tolng = null;
            string fromlat = null;
            string fromLng = null;
            string distanceresult = null;
            string weather = null;
            
            var coordinates = new Coordinates();
            
            var address = fromStreet + "+" + fromCity+ "+" + fromZip+ "+" + "Sweden";
            var requestUri = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}+&key=AIzaSyCvFNvcph_tb-gfx6u6PhnerNM3VC9zg8s", Uri.EscapeDataString(address));
            //AIzaSyCvFNvcph_tb-gfx6u6PhnerNM3VC9zg8s
            var proxy = WebProxy.GetDefaultProxy();
            proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            var request = System.Net.WebRequest.Create(requestUri);
            request.Proxy = proxy;
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var result = xdoc.Element("GeocodeResponse").Element("result");

            if (result != null)
            {
                var locationElement = result.Element("geometry").Element("location");

                var lat = locationElement.Element("lat");
                var lng = locationElement.Element("lng");
                fromlat = (string)lat;
                fromLng = (string)lng;
                if (lng == null && lat == null)
                {
                    fromlat = "-";
                    fromLng = "-";
                }

                
                //model.Coordinates.Fromlat = fromlat;
                //model.Coordinates.Fromlng = fromLng;
            }
            else
            {
              
            }

            var addressto = toStreet + "+" + toCity+ "+" + toZip+ "+" + "Sweden";
            var requestUrito = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}+&key=AIzaSyCvFNvcph_tb-gfx6u6PhnerNM3VC9zg8s", Uri.EscapeDataString(addressto));
            //AIzaSyCvFNvcph_tb-gfx6u6PhnerNM3VC9zg8s
            var requestto = System.Net.WebRequest.Create(requestUrito);
            requestto.Proxy = proxy;
            var responseto = requestto.GetResponse();
            var xdocto = XDocument.Load(responseto.GetResponseStream());

            var resultto = xdocto.Element("GeocodeResponse").Element("result");

            if (resultto != null)
            {
                var locationElement = resultto.Element("geometry").Element("location");

                var lat = locationElement.Element("lat");
                var lng = locationElement.Element("lng");
                tolat = (string)lat;
                tolng = (string)lng;
                if (lng == null && lat == null)
                {
                    tolat = "-";
                    tolng = "-";
                }

                //model.Coordinates.ToLat = tolat;
                //model.Coordinates.ToLng = tolng;
            }
            else
            {

            }

            var FromAdress = fromStreet + "+" + fromCity + "+" + fromZip + "+" + "Sweden";
            var ToAdress = toStreet + "+" + toCity + "+" + toZip + "+" + "Sweden";
            try
            {
                var requestUriDistance = string.Format("https://maps.googleapis.com/maps/api/distancematrix/xml?origins={0}&destinations={1}&key", Uri.EscapeDataString(FromAdress), Uri.EscapeDataString(ToAdress));

                var requestDistance = System.Net.WebRequest.Create(requestUriDistance);
                requestDistance.Proxy = proxy;
                var responseDistance = requestDistance.GetResponse();
                var xdocDistance = XDocument.Load(responseDistance.GetResponseStream());

                var resultDistance = xdocDistance.Element("DistanceMatrixResponse").Element("row");
                if (resultDistance != null)
                {
                    var distanceElement = resultDistance.Element("element").Element("distance");
                    var distance = distanceElement.Element("text");

                    distanceresult = (string)distance;
                }
                else
                {
                    //MessageBox.Show("Check the input data so that it is a correct address");
                }

            }
            catch (Exception)
            {

                throw;
            }
            var ToCity = toCity + ",SE";
            string s = null;
            if (toCity.Contains("å"))
            {
                s = ToCity.Replace("å", "a");
            }
            if (toCity.Contains("ä"))
            {
                s = ToCity.Replace("ä", "a");
            }
            if (toCity.Contains("ö"))
            {
                s = ToCity.Replace("ö", "o");
            }
            var requestUriWeather = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&mode=xml&APPID=cb9c8d20e2631b4753a6b1854ea02f8a", Uri.EscapeDataString(s));

            var requestWeather = System.Net.WebRequest.Create(requestUriWeather);
            requestWeather.Proxy = proxy;
            var responseWeather = requestWeather.GetResponse();
            var xdocWeather = XDocument.Load(responseWeather.GetResponseStream());

            var resultWeather = xdocWeather.Element("weatherdata").Element("forecast");
            if (resultWeather != null)
            {
                var time = resultWeather.Descendants().First();
                var windDir = time.Element("windDirection").Attribute("name");
                var Windspeed = time.Element("windSpeed").Attribute("name");
                var preciptation = time.Element("precipitation").Attribute("type");


                //var windname = result.Element("speedname");
                //var winddir = result.Element("direction name");
                weather = "WindSpeed: " + (string)Windspeed + "\r\n Wind Direction: " + (string)windDir + "\r\n Type of weather: " + (string)preciptation;

            }


            //calcprice calcpriceweight = new calcprice();
            //weight = coordinates.Weight;
                

            string price = GetPrice(distanceresult, weight);


            return View("Index", new Viewmodel
            {
                Customer = customer,
                Coordinates = new Coordinates
                {
                    ToStreet = toStreet,
                    ToCity = toCity,
                    ToZip = toZip,
                    Fromlat = fromlat,
                    Fromlng = fromLng,
                    ToLat = tolat,
                    ToLng = tolng,
                    Distance = distanceresult,
                    Weather = weather,
                    Weight = weight,
                    Price = price
                }
                
            });
        }

        
        public string GetPrice(string Distance, string Weight)
        {
            
           
            string price = null;
            
            if (Weight != null && Distance != null)
            {
                string w = Weight.Replace("kg", "");
                int weight = Convert.ToInt32(w);

                string distance = Distance.Replace("km", "").Trim();
                string distance2 = distance.Replace(".", ",");
                double distanceint = Convert.ToDouble(distance2);


                if (Enumerable.Range(1, 10).Contains(weight))//weight <= 10 && distanceint <= 20)
                {
                    if (distanceint < 10 || distanceint > 0)
                    {
                        price = "15$";
                    }
                    else if (distanceint < 50 || distanceint > 10)
                    {
                        price= "25$";
                    }
                    else if (distanceint < 100 || distanceint > 50)
                    {
                        price = "45$";
                    }
                    else if (distanceint > 100)
                    {
                        price = "80$";
                    }

                }
                else if (Enumerable.Range(10, 20).Contains(weight)) // distanceint >= 20 && distanceint <= 70)
                {
                    if (distanceint < 10 && distanceint > 0)
                    {
                        price = "25$";
                    }
                    else if (distanceint < 50 && distanceint > 10)
                    {
                        price = "35$";
                    }
                    else if (distanceint < 100 && distanceint > 50)
                    {
                        price = "55$";
                    }
                    else if (distanceint > 100)
                    {
                        price = "105$";
                    }
                }
                else if (Enumerable.Range(30, 20).Contains(weight))
                {
                    if (distanceint < 10 && distanceint > 0)
                    {
                        price = "35$";
                    }
                    else if (distanceint < 50 && distanceint > 10)
                    {
                        price = "45$";
                    }
                    else if (distanceint < 100 && distanceint > 50)
                    {
                        price = "65$";
                    }
                    else if (distanceint > 100)
                    {
                        price = "155$";
                    }
                }
                else if (Enumerable.Range(50, 50).Contains(weight))
                {

                    if (distanceint < 10 && distanceint > 0)
                    {
                        price = "65$";
                    }
                    else if (distanceint < 50 && distanceint > 10)
                    {
                        price = "75$";
                    }
                    else if (distanceint < 100 && distanceint > 50)
                    {
                        price = "95$";
                    }
                    else if (distanceint > 100)
                    {
                        price = "205$";
                    }
                }
                else if (Enumerable.Range(100, 50).Contains(weight))
                {
                    price = "100$";
                    if (distanceint < 10 && distanceint > 0)
                    {
                        price = "100$";
                    }
                    else if (distanceint < 50 && distanceint > 10)
                    {
                        price = "110$";
                    }
                    else if (distanceint < 100 && distanceint > 50)
                    {
                        price = "130$";
                    }
                    else if (distanceint > 100)
                    {
                        price = "255$";
                    }
                }
                else if (weight > 150)
                {
                    price = "1337$";
                }

            }

            return price;
            
        }
        [HttpPost]
        public ActionResult CreateOrder(int customerId, Coordinates cords ) //[Bind(Include = "FromAddress,FromCity,Distance,ToStreet,ToCity,Price, Weight")] (int customerId, string fromStreet, string fromCity, int fromZip, string toStreet, string toCity, int toZip, string weight, Coordinates cords)
        {

            try
            {
               var customer = db.Customers.FirstOrDefault(i => i.Id == customerId);

                var order = new OrderHistory();
                order.Customer = customer;
                order.ToCoordinates = cords;
                db.OrderHistories.Add(order);
                db.SaveChanges();
                var body = $"Hi "+customer.Name+"! \r\n \r\n Thank you for choosing to deliver you pakage with our Drone service. \r\n \r\n Order Information:" + "\r\n" + "Order Number: " + order.Orderid + "\r\n " +
                "\r\n Pakage sent from: " + customer.Name + " " + customer.LastName + "\r\n" + "Adress: " + customer.Street + " " + customer.City +
                "\r\n" + "To Adress: " + cords.ToStreet + " " + cords.ToCity + "\r\n" +" Distance: "+cords.Distance + "\r\n Price: " + cords.Price + "\r\n Weight: "+ cords.Weight + "\r\n \r\n" + "Br, The Droney Family";

                Email.Sendmail(customer.Email, body, "Order Confirmation");

                return View("Index", new Viewmodel
                {
                    Customer = customer,
                    Coordinates = cords,
                });
            }
            catch (Exception e)
            {

            }
            //
            return View("Index", new Viewmodel());
        }

        
        public ActionResult ClearAll()
        {
            return View("Index", new Viewmodel());
        }

       



    }
    
}