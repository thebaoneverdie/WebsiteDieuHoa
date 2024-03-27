using Antlr.Runtime;
using DoAnCoSo.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Other;

namespace DoAnCoSo.Controllers
{
    public class ShoppingCartController : Controller
    {

        public List<CartItem> GetShoppingCartFromSession()
        {
            var listShoppingCart = Session["ShoppingCart"] as List<CartItem>;
            if (listShoppingCart == null)
            {
                listShoppingCart = new List<CartItem>();
                Session["ShoppingCart"] = listShoppingCart;
            }
            return listShoppingCart;
        }


        public ActionResult Cart()
        {
            List<CartItem> shoppingCart = GetShoppingCartFromSession();
            if (shoppingCart.Count == 0)
            {
                return RedirectToAction("Index", "Shop");
            }
            ViewBag.Tongsoluong = shoppingCart.Sum(p => p.quantity);
            ViewBag.Tongtien = shoppingCart.Sum(p => p.quantity * p.price);
            TempData.Remove("MyValue");
            TempData["MyValue"] = ViewBag.Tongtien;

            decimal tongtien = 0;
            if (Session["Tongtien"] != null)
            {
                ViewBag.giamgia = Session["giamgiathanhcong"];
                Session.Remove("giamgiathanhcong");
                
                tongtien = (decimal)Session["Tongtien"]; // lấy giá trị giảm giá từ Session
                Session.Remove("Tongtien");
                ViewBag.Tongtien = tongtien;

            }
            else
            {
                ViewBag.giamgiaError = Session["giamgiakhongthanhcong"];
                
            }
 //
            TempData["tongtiengiamgia"] = ViewBag.Tongtien;
            return View(shoppingCart);
        }


        public RedirectToRouteResult AddToCart(int id)
        {
            if (Session["idUser"] != null)
            {
                MayLanhModel db = new MayLanhModel();
                List<CartItem> ShoppingCart = GetShoppingCartFromSession();
                CartItem findCartItem = ShoppingCart.FirstOrDefault(m => m.id == id);
                if (findCartItem == null)
                {
                    Product findProduct = db.Product.First(m => m.id == id);
                    CartItem newItem = new CartItem()
                    {
                        id = findProduct.id,
                        title = findProduct.title,
                        quantity = 1,
                        image = findProduct.thumbnail,
                        price = findProduct.price.Value
                    };
                    ShoppingCart.Add(newItem);
                }
                else
                {
                    findCartItem.quantity++;
                }

                return RedirectToAction("Cart", "ShoppingCart");
            }
            else
            {
                return RedirectToAction("DangNhap", "Account");
            }
        }

        public RedirectToRouteResult UpdateCart(int id, int txtQuantity)
        {
            var itemFind = GetShoppingCartFromSession().FirstOrDefault(p => p.id == id);
            if (itemFind != null)
                itemFind.quantity = txtQuantity;
            return RedirectToAction("Cart");
        }




        public ActionResult CartSumary()
        {
            ViewBag.CartCount = GetShoppingCartFromSession().Count();
            return PartialView("CartSumary");
        }


        public RedirectToRouteResult RemoveCartItem(int id)
        {
            List<CartItem> ShoppingCart = GetShoppingCartFromSession();
            CartItem findCartItem = ShoppingCart.FirstOrDefault(m => m.id == id);
            if (findCartItem != null)
                ShoppingCart.Remove(findCartItem);
            return RedirectToAction("Cart");
        }


        public ActionResult Order()
        {
            MayLanhModel db = new MayLanhModel();
            int idUser = (int)Session["idUser"];
            int currentUserId = idUser;
            var infoUserOrder = db.User.FirstOrDefault(p => p.id == currentUserId);
            var listShoppingCart = Session["ShoppingCart"] as List<CartItem>;
            //    int tongTien = (int)listShoppingCart.Sum(sp => sp.Money);
            int tongtien;
            object value = TempData["tongtiengiamgia"];
            int.TryParse(value.ToString(), out int result);
            tongtien = result;
            // Tính toán giá trị giảm giá

            try
            {
                Orders userOrder = new Orders()
                {
                    user_id = currentUserId,
                    fullname = infoUserOrder.fullname,
                    email = infoUserOrder.email,
                    phone_number = infoUserOrder.phone_number,
                    address = infoUserOrder.address,
                    order_date = DateTime.Now,
                    isPaid = false,
                    isComplete = false,
                    total_money = tongtien,
                };
                userOrder = db.Orders.Add(userOrder);
                db.SaveChanges();
                List<CartItem> listCartItem = GetShoppingCartFromSession();
                foreach (var item in listCartItem)
                {

                    Order_Details cthd = new Order_Details()
                    {
                        order_id = userOrder.id,
                        product_id = item.id,
                        num = item.quantity,
                        price = (int?)item.price,
                        total_money = (int)item.price * item.quantity,
                    };
                    db.Order_Details.Add(cthd);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Content("Error Cart");
            }
            Session["ShoppingCart"] = null;



            return RedirectToAction("ConfirmOrder", "ShoppingCart");
        }


        public ActionResult ConfirmOrder()
        {
            return View();
        }


        /////////////////////////////////////////////payment
        public ActionResult Payment()
        {

            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "test";
            string returnUrl = "https://localhost:44324/ShoppingCart/Payment";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = "1000";
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public ActionResult ConfirmPaymentClient(Result result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            string rMessage = result.message;
            string rOrderId = result.orderId;
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công
            return View();
        }

        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
            String a = "";
        }


        public ActionResult ApplyDiscountCode(FormCollection form)
        {
            MayLanhModel db = new MayLanhModel();
            string giamgia = form["discountCode"];
            var discount = db.Galery.FirstOrDefault(d => d.id.ToString() == giamgia);
            object value = TempData["MyValue"];
            if (discount != null && value != null && int.TryParse(value.ToString(), out int result))
            {
                int myValue = result;
                // Tính toán giá trị giảm giá
                decimal discountAmount = 0;
                discountAmount = myValue * 90 / 100;
                Session["Tongtien"] = discountAmount;
                Session["giamgiathanhcong"] = "Mã giảm giá hợp lệ";
                // Điều hướng trở về trang giỏ hàng
                return RedirectToAction("Cart");
            }
            else
            {
                // Hiển thị thông báo lỗi nếu mã giảm giá không hợp lệ
                Session["giamgiakhongthanhcong"] = "Mã giảm giá không tồn tại hoặc hết hạn";
                return RedirectToAction("Cart");
            }
        }
    }
















}

