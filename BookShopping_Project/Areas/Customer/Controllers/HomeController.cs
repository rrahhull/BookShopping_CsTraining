using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShopping_Project.Utility;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShopping_Project.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        
        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var ClaimsIdentity = (ClaimsIdentity)(User.Identity);
            var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(Claim!=null)
            {
                var Count = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_Session, Count);
            }
            var ProductList = _unitOfWork.Product.GetAll(IncludeProperties: "Category,CoverType");
            return View(ProductList);
        }
        public IActionResult Details(int Id)
        {
            var ProductInDb = _unitOfWork.Product.FirstorDefault(p => p.id==Id,IncludeProperties:"Category,CoverType");
            var ShoppingCart = new ShoppingCart()
            {
                Product = ProductInDb,
                ProductId = ProductInDb.id
            };
            return View(ShoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart ShoppingCartObj)
        {
            ShoppingCartObj.id = 0;
            if (ModelState.IsValid)
            {
                var ClaimsIdentity = (ClaimsIdentity)User.Identity;
                var Claims = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                ShoppingCartObj.ApplicationUserId = Claims.Value;
                var ShoppingCartFromDb = _unitOfWork.shoppingCart.FirstorDefault(u => u.ApplicationUserId == Claims.Value && u.ProductId == ShoppingCartObj.ProductId);
                if (ShoppingCartFromDb == null)
                    _unitOfWork.shoppingCart.Add(ShoppingCartObj);
                else
                    ShoppingCartFromDb.Count += ShoppingCartObj.Count;
                _unitOfWork.Save();
                var count = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId ==Claims.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_Session, count);
                return RedirectToAction(nameof(Index));
            }
            
            else
            {
                var ProductInDb = _unitOfWork.Product.FirstorDefault(p => p.id == ShoppingCartObj.ProductId, IncludeProperties: "Category,CoverType");
                var ShoppingCart = new ShoppingCart()
                {
                    Product = ProductInDb,
                    ProductId = ProductInDb.id
                };
                return View(ShoppingCart);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
