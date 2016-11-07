using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Data.DAO;
using Data.Models;
using ELEXStore.Common;
using ELEXStore.Models;
using PayPal.Api;

namespace ELEXStore.Controllers
{
    public class PaypalController : Controller
    {
        //
        // GET: /Paypal/

        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult Payment(Data.Models.Order orderInput, string YourRadioButton)
        {
            if (YourRadioButton == "cod")
            {
                return RedirectToAction("PaymentWithCOD", orderInput);
            }
            else
            {
                return RedirectToAction("PaymentWithPaypal", orderInput);
            }
          
        }

        public ActionResult PaymentWithCOD(Data.Models.Order orderInput)
        {
            try
            {

                var order = new Data.Models.Order()
                {
                    CreatedDate = DateTime.Now,
                    PaymentMethod = "COD",
                    ShipAddress = orderInput.ShipAddress,
                    ShipEmail = orderInput.ShipEmail,
                    ShipMobile = orderInput.ShipMobile,
                    ShipDescription = orderInput.ShipDescription,
                    Status = 0,
                    ShipName = orderInput.ShipName
                };
                var insert = new OrdersDao().Insert(order);
                var detailsDao = new OrderDetailsDao();
                if (insert)
                {
                    foreach (var cart in CommonConstants.listCart)
                    {
                        var orderDetail = new OrderDetails
                        {
                            ProductID = cart.Product.ID,
                            OrderID = order.ID,
                            Price = cart.Product.Price - cart.Product.Price*cart.Product.Discount/100,
                            Quantity = cart.Quantity
                        };
                        detailsDao.Insert(orderDetail);
                    }
                }
                CommonConstants.listCart.Clear();
                Session.Remove("CartSession");
                return RedirectToAction("Success", "Cart");
            }
            catch
            {
                return RedirectToAction("Index", "Cart");
            }
        }
        public ActionResult PaymentWithPaypal(Data.Models.Order orderInput)
        {
            APIContext apiContext = Configuration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (String.IsNullOrEmpty(payerId))
                {
                    var profile = new WebProfile
                    {
                        name = Guid.NewGuid().ToString(),
                        presentation = new Presentation
                        {
                            brand_name = "ELEX Store",
                            locale_code = "US"
                        },
                        input_fields = new InputFields
                        {
                            no_shipping = 1
                        }
                    };
                    var createdProfile = profile.Create(apiContext);

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                      "/Paypal/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    baseURI += "guid=" + guid + "&webProfileId=" + createdProfile.id;
                    Session["webProfileId"] = createdProfile.id;
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid,createdProfile);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    Session["OrderInput"] = orderInput;
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];
                    var webProfileId = Session["webProfileId"].ToString();
                    Data.Models.Order orderModel = (Data.Models.Order)Session["OrderInput"];
                    try
                    {

                        var order = new Data.Models.Order()
                        {
                            CreatedDate = DateTime.Now,
                            PaymentMethod = "PayPal",
                            ShipAddress = orderModel.ShipAddress,
                            ShipEmail = orderModel.ShipEmail,
                            ShipMobile = orderModel.ShipMobile,
                            ShipDescription = orderModel.ShipDescription,
                            Status = 0,
                            ShipName = orderModel.ShipName
                        };
                        var insert = new OrdersDao().Insert(order);
                        var detailsDao = new OrderDetailsDao();
                        if (insert)
                        {
                            foreach (var cart in CommonConstants.listCart)
                            {
                                var orderDetail = new OrderDetails
                                {
                                    ProductID = cart.Product.ID,
                                    OrderID = order.ID,
                                    Price = cart.Product.Price-cart.Product.Price*cart.Product.Discount/100,
                                    Quantity = cart.Quantity
                                };
                                detailsDao.Insert(orderDetail);
                            }
                        }
                        var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                        WebProfile.Delete(apiContext, webProfileId);
                        Session.Remove("webProfileId");
                        CommonConstants.listCart.Clear();
                        Session["CartSession"]=CommonConstants.listCart;
                        if (executedPayment.state.ToLower() != "approved")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Index", "Cart");
                        
                    }            
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }


            return RedirectToAction("Success", "Cart");
        }
        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl,CreateProfileResponse profile)
        {

            payment = null;
            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            foreach (CartItem cart in CommonConstants.listCart)
            {
                itemList.items.Add(new Item()
                {
                    name = cart.Product.Name,
                    currency = "USD",
                    price = (cart.Product.Price-cart.Product.Price*cart.Product.Discount/100).ToString(),
                    quantity = cart.Quantity.ToString()
                });
            }

            var payer = new Payer()
            {
                payment_method = "paypal",
            };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = Url.Action("Index", "Cart", null, this.Request.Url.Scheme),
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = CommonConstants.listCart.Sum(x => x.Quantity * (x.Product.Price - x.Product.Price * x.Product.Discount / 100)).ToString(),
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = CommonConstants.listCart.Sum(x => x.Quantity * (x.Product.Price - x.Product.Price * x.Product.Discount / 100)).ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",
                invoice_number = "your invoice number",
                amount = amount,
                item_list = itemList
            });

            payment = new Payment()
            {
                experience_profile_id = profile.id,
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext
            return this.payment.Create(apiContext);
        }

    }
}
