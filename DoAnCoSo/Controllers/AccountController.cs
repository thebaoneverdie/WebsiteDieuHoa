using DoAnCoSo.Models;
using DoAnCoSo.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Controllers
{
    public class AccountController : Controller
    {
        MayLanhModel dbContext = new MayLanhModel();
        // GET: Account
        // GET: Account
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        // GET: Account
        public ActionResult DangKy()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Register(User user)
        //{
        //    var dbContext = new ShopNT();
        //    if (ModelState.IsValid)
        //    {
        //        var check = dbContext.User.FirstOrDefault(s => s.email == user.email);
        //        if (check == null)
        //        {
        //            user.role_id = 2;
        //            user.password = GetMD5(user.password);
        //            dbContext.Configuration.ValidateOnSaveEnabled = false;
        //            dbContext.User.Add(user);
        //            dbContext.SaveChanges();
        //            return RedirectToAction("Home", "Explore");
        //        }
        //        else
        //        {
        //            ViewBag.error = "Email already exists";
        //            return View();
        //        }


        //    }
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(RegisterModel user)
        {
            var dbContext = new MayLanhModel();
            var newUser = new User();
            if (ModelState.IsValid)
            {
                var check = dbContext.User.FirstOrDefault(s => s.email == user.email);
                if (check == null)
                {
                    newUser.role_id = 2;
                    user.password = GetMD5(user.password);
                    dbContext.Configuration.ValidateOnSaveEnabled = false;
                    newUser.email = user.email;
                    newUser.fullname = user.fullname;
                    newUser.password = user.password;
                    newUser.phone_number = user.phone_number;
                    newUser.created_at = DateTime.Now;
                    dbContext.User.Add(newUser);
                    dbContext.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại";
                    return View();
                }


            }
            return View();
        }



        // Login

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string email, string password)
        {
            var dbContext = new MayLanhModel();
            if (ModelState.IsValid)
            {

                var f_password = GetMD5(password);
                var data = dbContext.User.Where(s => s.email.Equals(email) && s.password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {

                    Session["FullName"] = data.FirstOrDefault().fullname;
                    Session["idUser"] = data.FirstOrDefault().id;
                    Session["Role"] = data.FirstOrDefault().Role.name;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error1 = "Sai email hoặc mật khẩu";
                    return View();
                }
            }
            return View();
        }


        public ActionResult LogOff()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //return RedirectToAction("Index", "Home");
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }



        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword editUser)
        {
            var dbContext = new MayLanhModel();
            var f_password = GetMD5(editUser.password);
            var check = dbContext.User.FirstOrDefault(s => s.email.Equals(editUser.email) && s.password.Equals(f_password));
            if (check != null)
            {
                var new_password = GetMD5(editUser.newPassword);
                check.password = new_password;
                dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return HttpNotFound("tim khong thay");
            }

        }
        public ActionResult CheckOrder(int id)
        {
           
            
                ViewBag.ListUserId = dbContext.User.ToList();
                var _orderDetails = dbContext.Orders.FirstOrDefault(p => p.id == id);
                return View(_orderDetails);



        }


        [HttpPost]
        public ActionResult CheckOrder(Orders editOrder)
        {

            var _order = dbContext.Orders.FirstOrDefault(p => p.id == editOrder.id);
            if (_order != null)
            {
                _order.isPaid = editOrder.isPaid;
                _order.isComplete = editOrder.isComplete;
                dbContext.SaveChanges();
                return RedirectToAction("ManagerOrder", "Manager");
            }
            else
            {
                return HttpNotFound("khong tim thay hoac loi");
            }
        }

    }
}
