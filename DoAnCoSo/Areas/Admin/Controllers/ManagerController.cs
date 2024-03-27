using DoAnCoSo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Areas.Admin.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {


            return View();
        }



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

        //////////////////////////////////////////////PRODUCT
        // GET: Admin/Manager
        MayLanhModel dbContext = new MayLanhModel();
        public ActionResult ManagerProduct(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.AdminPriceDes = "price_Des";
            ViewBag.AdminPriceAsc = "price_Asc";

            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var product = from s in dbContext.Product
                              select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    product = product.Where(s => s.title.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "price_Des":
                        product = product.OrderByDescending(s => s.price);
                        break;
                    case "price_Asc":
                        product = product.OrderBy(s => s.price);
                        break;
                    default:  // Name ascending                    
                        break;
                }


                int pageSize = 6;
                int pageIndex = page.HasValue ? page.Value : 1;

                return View(product.ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }


        }



        /////////////////////////////////Create
        [HttpGet]
        public ActionResult CreateProduct()
        {
            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                ViewBag.ListCategory = dbContext.Category.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }


        }

        [HttpPost]
        public ActionResult CreateProduct(Product newProduct)
        {

            if (ModelState.IsValid)
            {
                //if (newProduct.thumbnail != null && newProduct.ImageFile.ContentLength > 0)
                //{
                //    var fileName = Path.GetFileName(newProduct.ImageFile.FileName);
                //    var filePath = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                //    newProduct.ImageFile.SaveAs(filePath);
                //    newProduct.thumbnail = "/Content/Images/" + fileName;
                //}
                dbContext.Product.Add(newProduct);
                dbContext.SaveChanges();
                return RedirectToAction("ManagerProduct");
            }
            else
                return RedirectToAction("Create");
        }


        [HttpGet]
        public ActionResult EditProduct(int id)
        {

            //string admin = (string)Session["Role"];
            //if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            //{
            var _product = dbContext.Product.FirstOrDefault(p => p.id == id);
            ViewBag.ListCategory = dbContext.Category.ToList();
            if (_product != null)
            {
                return View(_product);
            }
            else
            {
                return HttpNotFound("khong tim thay hoac loi");
            }
            //}
            //else
            //{
            //    return RedirectToAction("DangNhap", "Account", new { area = "" });
            //}
        }

        [HttpPost]
        public ActionResult EditProduct(Product editProduct)
        {

            var _product = dbContext.Product.FirstOrDefault(p => p.id == editProduct.id);
            if (_product != null)
            {
                //var f_password = GetMD5(editUser.password);

                _product.title = editProduct.title;
                _product.category_id = editProduct.category_id;
                _product.price = editProduct.price;
                _product.thumbnail = editProduct.thumbnail;
                _product.description = editProduct.description;
                dbContext.SaveChanges();
                return RedirectToAction("ManagerProduct", "Manager");
            }
            else
            {
                return HttpNotFound("tim khong thay");
            }
        }


        public ActionResult DeleteProduct(int id)
        {
            var _product = dbContext.Product.FirstOrDefault(p => p.id == id);
            return View(_product);
        }

        [HttpGet]
        public ActionResult DeleteProduct(Product delProduct)
        {
            var _product = dbContext.Product.FirstOrDefault(p => p.id == delProduct.id);
            if (_product != null)
            {
                dbContext.Product.Remove(_product);
                dbContext.SaveChanges();
                return RedirectToAction("ManagerProduct", "Manager");
            }
            else
            {
                return HttpNotFound("khong tim thay hoac loi");
            }
        }





        ////////////////////////////////////////////////////////////USER
        public ActionResult ManagerUser(string sortUser, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortUser;
            ViewBag.AdminUserDes = "user_Des";
            ViewBag.AdminUserAsc = "user_Asc";


            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var user = from s in dbContext.User
                           select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    user = user.Where(s => s.fullname.Contains(searchString));
                }


                switch (sortUser)
                {
                    case "user_Des":
                        user = user.OrderByDescending(s => s.fullname);
                        break;
                    case "user_Asc":
                        user = user.OrderBy(s => s.fullname);
                        break;
                    default:  // Name ascending                    
                        break;
                }


                int pageSize = 6;
                int pageIndex = page.HasValue ? page.Value : 1;



                //  var listProduct = dbContext.Product.Where(p =>p.title == name)
                //var listProduct = dbContext.Product.ToList();
                return View(user.ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }
        }

        [HttpGet]
        public ActionResult CreateUser()
        {
            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                ViewBag.ListRoleUser = dbContext.Role.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }



        }

        [HttpPost]
        public ActionResult CreateUser(User newUser)
        {
            newUser.password = GetMD5(newUser.password);

            if (ModelState.IsValid)
            {
                var check = dbContext.User.FirstOrDefault(s => s.email == newUser.email);
                if (check == null)
                {
                    dbContext.User.Add(newUser);
                    dbContext.SaveChanges(); ;
                    return RedirectToAction("ManagerUser");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại";
                    ViewBag.ListRoleUser = dbContext.Role.ToList();
                    return View();
                }
            }
            else
                return RedirectToAction("CreateUser");
        }

        public ActionResult EditUser(int id)
        {
            var _user = dbContext.User.FirstOrDefault(p => p.id == id);
            ViewBag.ListRoleUser = dbContext.Role.ToList();
            return View(_user);
        }
        [HttpPost]
        public ActionResult EditUser(User editUser)
        {

            var _user = dbContext.User.FirstOrDefault(p => p.id == editUser.id);
            if (_user != null)
            {
                var f_password = GetMD5(editUser.password);
                _user.fullname = editUser.fullname;
                _user.role_id = editUser.role_id;
                _user.password = f_password;
                _user.address = editUser.address;
                dbContext.SaveChanges();
                return RedirectToAction("ManagerUser", "Manager");
            }
            else
            {
                return HttpNotFound("khong tim thay hoac loi");
            }
        }

        public ActionResult DeleteUser(int id)
        {
            var _User = dbContext.User.FirstOrDefault(p => p.id == id);
            return View(_User);
        }

        [HttpGet]
        public ActionResult DeleteUser(User delUser)
        {


            var _User = dbContext.User.FirstOrDefault(p => p.id == delUser.id);
            if(_User != null)
            {
                var _UserFind = dbContext.Orders.FirstOrDefault(p => p.user_id == _User.id);
                if (_UserFind != null)
                {
                    ViewBag.ErroDellUser = "Ban khong the xoa nguoi dung nay";
                    return View();

                }
                else
                {
                    dbContext.User.Remove(_User);
                    dbContext.SaveChanges();
                    return RedirectToAction("ManagerUser", "Manager");
                }
            }
            else
            {
                ViewBag.ErroDellUser1 = "Khong co nguoi dung hoac nguoi dung da bi xoa";
                return View();
            }


            


        }


        ///////////////////////////////////////////Order
        ///
        public ActionResult ManagerOrder(string currentFilter, string searchString, int? page, string sortday)
        {
            string admin = (string)Session["Role"];
            ViewBag.CurrentSort = sortday;
            ViewBag.AdminDayDes = "day_Des";
            ViewBag.AdminDayAsc = "day_Asc";
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var orders = from s in dbContext.Orders
                             select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(s => s.fullname.Contains(searchString));
                }


                switch (sortday)
                {
                    case "day_Des":
                        orders = orders.OrderByDescending(s => s.order_date);
                        break;
                    case "day_Asc":
                        orders = orders.OrderBy(s => s.order_date);
                        break;
                    default:  // Name ascending                    
                        break;
                }

                int pageSize = 6;
                int pageIndex = page.HasValue ? page.Value : 1;



                //  var listProduct = dbContext.Product.Where(p =>p.title == name)
                //var listProduct = dbContext.Product.ToList();
                return View(orders.ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }

        }

        [HttpGet]
        public ActionResult DetailOrder(int id, string currentFilter, string searchString, int? page)
        {
            //var _orderDetails = dbContext.Orders.FirstOrDefault(p => p.id == id);
            //return View(_orderDetails);
            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                List<Order_Details> listOrder = dbContext.Order_Details.Where(p => p.order_id == id).ToList();

                int pageSize = 6;
                int pageIndex = page.HasValue ? page.Value : 1;



                //  var listProduct = dbContext.Product.Where(p =>p.title == name)
                //var listProduct = dbContext.Product.ToList();
                return View(listOrder.ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }
        }


        public ActionResult CheckOrder(int id)
        {
            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                ViewBag.ListUserId = dbContext.User.ToList();
                var _orderDetails = dbContext.Orders.FirstOrDefault(p => p.id == id);
                return View(_orderDetails);
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }
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

        /////////////////////////////////////////danh muc
        public ActionResult ManagerCategory(string currentFilter, string searchString, int? page, string sortname)
        {
            ViewBag.CurrentSort = sortname;
            ViewBag.AdminnameDes = "name_Des";
            ViewBag.AdminnameAsc = "name_Asc";
            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var category = from s in dbContext.Category
                               select s;
                if (!String.IsNullOrEmpty(searchString))
                {
                    category = category.Where(s => s.name.Contains(searchString));
                }


                switch (sortname)
                {
                    case "name_Des":
                        category = category.OrderByDescending(s => s.name);
                        break;
                    case "name_Asc":
                        category = category.OrderBy(s => s.name);
                        break;
                    default:  // Name ascending                    
                        break;
                }
                int pageSize = 6;
                int pageIndex = page.HasValue ? page.Value : 1;

                return View(category.ToList().ToPagedList(pageIndex, pageSize));
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }
        }

        [HttpGet]
        public ActionResult CreateCategory()
        {
            string admin = (string)Session["Role"];
            if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            {
                ViewBag.ListCate = dbContext.Category.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("DangNhap", "Account", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult CreateCategory(Category newCategory)
        {

            if (ModelState.IsValid)
            {
                dbContext.Category.Add(newCategory);
                dbContext.SaveChanges();
                return RedirectToAction("ManagerCategory");
            }
            else
                return RedirectToAction("CreateCategory");
        }
        [HttpGet]
        public ActionResult EditCategory(int id)
        {

            //string admin = (string)Session["Role"];
            //if (Session["idUser"] != null && admin.CompareTo("Admin") == 0)
            //{
            var category = dbContext.Category.FirstOrDefault(p => p.id == id);
            ViewBag.ListCate = dbContext.Category.ToList();
            if (category != null)
            {
                return View(category);
            }
            else
            {
                return HttpNotFound("khong tim thay hoac loi");
            }
            //}
            //else
            //{
            //    return RedirectToAction("DangNhap", "Account", new { area = "" });
            //}


        }

        [HttpPost]
        public ActionResult EditCategory(Category editCategory)
        {

            var category = dbContext.Category.FirstOrDefault(p => p.id == editCategory.id);
            if (category != null)
            {
                //var f_password = GetMD5(editUser.password);

                category.name = editCategory.name;
                dbContext.SaveChanges();
                return RedirectToAction("ManagerCategory", "Manager");
            }
            else
            {
                return HttpNotFound("tim khong thay");
            }



        }


        public ActionResult DeleteCategory(int id)
        {
            var category = dbContext.Category.FirstOrDefault(p => p.id == id);
            return View(category);
        }

        [HttpGet]
        public ActionResult DeleteCategory(Category delcategory)
        {
            var category = dbContext.Category.FirstOrDefault(p => p.id == delcategory.id);
            if (category != null)
            {
                var _CateFind = dbContext.Product.FirstOrDefault(p => p.category_id == category.id);
                if (_CateFind != null)
                {
                    ViewBag.ErroDellCate = "Ban khong the xoa loai san pham nay";
                    return View();

                }
                else
                {
                    dbContext.Category.Remove(category);
                    dbContext.SaveChanges();
                    return RedirectToAction("ManagerCategory", "Manager");
                }
            }
            else
            {
                ViewBag.ErroDellCate1 = "Khong co loai san pham nay hoac da bi xoa";
                return View();
            }





        }



    }

}

