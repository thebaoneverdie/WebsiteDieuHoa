using DoAnCoSo.Models;

using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace DoAnCoSo.Controllers
{
    public class ShopController : Controller
    {
        MayLanhModel dbContext = new MayLanhModel();
        // GET: Shop
        //public ActionResult Index()
        //{
            
        //    return View(dbContext.Product.ToList());
        //}

        // test category
        public ActionResult GetCategory()
        {
            var listCategory = dbContext.Category.ToList();
            return PartialView(listCategory);
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? categoryId)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.PriceDes = "price_Des";
            ViewBag.PriceAsc = "price_Asc";
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

            if (categoryId != null)
            {
                ViewBag.category = categoryId;
                product = product.Where(s => s.category_id == categoryId);
            }

           

            int pageSize = 20;
            int pageIndex = page.HasValue ? page.Value:1;



            //  var listProduct = dbContext.Product.Where(p =>p.title == name)
            //var listProduct = dbContext.Product.ToList();
            return View(product.ToList().ToPagedList(pageIndex, pageSize));

        }

        public ActionResult ChiTiet(int id)
        {
            var detailsProduct = dbContext.Product.FirstOrDefault(Product => Product.id == id);
            if (detailsProduct != null)
            {
                return View(detailsProduct);
            }
            else
            {
                return HttpNotFound("khong tim thay");
            }
        }

        public ActionResult CheckOrder_User(string sortOrder, string currentFilter, string searchString, int? page, int? id)
        {
            int userId = (int)Session["idUser"];
            if (Session["idUser"] != null)
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

                var orders = dbContext.Orders.Where(s=>s.user_id== userId);
                if (!String.IsNullOrEmpty(searchString))
                {
                    orders = orders.Where(s => s.id.ToString().Contains(searchString));
                }



            

                int pageSize = 20;
                int pageIndex = page.HasValue ? page.Value : 1;



                //  var listProduct = dbContext.Product.Where(p =>p.title == name)
                //var listProduct = dbContext.Product.ToList();
                return View(orders.ToList().ToPagedList(pageIndex, pageSize));

            }
            return RedirectToAction("Index", "Home");
        }
        
    }




}