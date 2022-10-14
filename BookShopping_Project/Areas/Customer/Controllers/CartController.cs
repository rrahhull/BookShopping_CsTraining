using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShopping_Project.Utility;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShopping_Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public CartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [BindProperty]
        public ShoppingCartVm shoppingCartVm { get; set; }
        public IActionResult Index()
        {
            
            var ClaimsIdentity = (ClaimsIdentity)(User.Identity);
            var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claim == null)
            {
                shoppingCartVm = new ShoppingCartVm()
                {
                    ListCart = new List<ShoppingCart>()
                };
                return View(shoppingCartVm);
            }
            shoppingCartVm  = new ShoppingCartVm()
            {
                orderHeader = new OrderHeader(),
                ListCart = _unitOfWork.shoppingCart.GetAll
                (sc => sc.ApplicationUserId == Claim.Value, 
                IncludeProperties: "Product")
            };
            shoppingCartVm.orderHeader.OrderTotal = 0;
            shoppingCartVm.orderHeader.ApplicationUser = _unitOfWork.applicationUser.FirstorDefault(u => u.Id == Claim.Value, IncludeProperties: "company");
            foreach (var list in shoppingCartVm.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.price, list.Product.price50, list.Product.price100);
                shoppingCartVm.orderHeader.OrderTotal += (list.Price * list.Count);
                list.Product.description = SD.ConvertToRawHtml(list.Product.description);
                if (list.Product.description.Length > 100)
                {
                    list.Product.description = list.Product.description.Substring(0, 99) + "...";
                }

            }
            return View(shoppingCartVm);
        }
        public IActionResult Plus(int id)
        {
            var cart = _unitOfWork.shoppingCart.FirstorDefault(sc => sc.id == id, IncludeProperties: "Product");
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int id)
        {
            var cart = _unitOfWork.shoppingCart.FirstorDefault(sc => sc.id==id, IncludeProperties: "Product");
            cart.Count -= 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int id)
        {
            var cart = _unitOfWork.shoppingCart.FirstorDefault(sc => sc.id == id, IncludeProperties: "Product");
            _unitOfWork.shoppingCart.remove(cart);
            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var count = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claims.Value).ToList().Count;
            HttpContext.Session.SetInt32(SD.Ss_Session, count - 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Summary()
        {
            var ClaimIdentity = (ClaimsIdentity)(User.Identity);
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVm = new ShoppingCartVm()
            {
                orderHeader = new OrderHeader(),
                ListCart = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value, IncludeProperties: "Product")
            };
            shoppingCartVm.orderHeader.ApplicationUser = _unitOfWork.applicationUser.FirstorDefault(u => u.Id == Claim.Value, IncludeProperties: "company");
            foreach (var list in shoppingCartVm.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.price, list.Product.price50, list.Product.price100);
                shoppingCartVm.orderHeader.OrderTotal = (list.Price) * (list.Count);
                list.Product.description = SD.ConvertToRawHtml(list.Product.description);
                shoppingCartVm.orderHeader.Name = shoppingCartVm.orderHeader.ApplicationUser.Name;
                shoppingCartVm.orderHeader.PhoneNumber = shoppingCartVm.orderHeader.ApplicationUser.PhoneNumber;
                shoppingCartVm.orderHeader.StreetAddress = shoppingCartVm.orderHeader.ApplicationUser.StreetAddress;
                shoppingCartVm.orderHeader.City = shoppingCartVm.orderHeader.ApplicationUser.City;
                shoppingCartVm.orderHeader.State = shoppingCartVm.orderHeader.ApplicationUser.State;
                shoppingCartVm.orderHeader.PostalCode = shoppingCartVm.orderHeader.ApplicationUser.PostalCode;
                
            }
            return View(shoppingCartVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost(string StripeToken)
        {
            var ClaimIdentitiy = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentitiy.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVm.orderHeader.ApplicationUser = _unitOfWork.applicationUser.FirstorDefault(u => u.Id == Claim.Value, IncludeProperties: "company");
            shoppingCartVm.ListCart = _unitOfWork.shoppingCart.GetAll(sc => sc.ApplicationUserId == Claim.Value, IncludeProperties: "Product");
            shoppingCartVm.orderHeader.PaymentStatus = SD.PaymentStatusPending;
            shoppingCartVm.orderHeader.OrderStatus = SD.StatusPending;
            shoppingCartVm.orderHeader.OrderDate = DateTime.Now;
            shoppingCartVm.orderHeader.ApplicationUserId = Claim.Value;
            _unitOfWork.orderHeader.Add(shoppingCartVm.orderHeader);
            _unitOfWork.Save();
            foreach (var item in shoppingCartVm.ListCart)
            {
                item.Price = SD.GetPriceBasedOnQuantity(item.Count, item.Product.price, item.Product.price50, item.Product.price100);
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderId = shoppingCartVm.orderHeader.Id,
                    Price = item.Price,
                    Count = item.Count
                };
                shoppingCartVm.orderHeader.OrderTotal = orderDetails.Price * orderDetails.Count;
                _unitOfWork.orderDetails.Add(orderDetails);
                _unitOfWork.Save();
            }
            _unitOfWork.shoppingCart.RemoveRange(shoppingCartVm.ListCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.Ss_Session, 0);
            #region StripePayment
            if (StripeToken == null)
            {
                shoppingCartVm.orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                shoppingCartVm.orderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                shoppingCartVm.orderHeader.OrderStatus = SD.PaymentStatusApproved;
            }
            else
            //Payment Process
            {
                var options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(shoppingCartVm.orderHeader.OrderTotal),
                    Currency = "inr",
                    Description = "OrderId: " + shoppingCartVm.orderHeader.Id,
                    Source = StripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                if (charge.BalanceTransaction == null)
                    shoppingCartVm.orderHeader.PaymentStatus = SD.PaymentStatusRejected;
                else
                    shoppingCartVm.orderHeader.TransactionId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {
                    shoppingCartVm.orderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    shoppingCartVm.orderHeader.OrderStatus = SD.StatusApproved;
                    shoppingCartVm.orderHeader.PaymentDate = DateTime.Now;

                }
            }
            _unitOfWork.Save();

            #endregion
                return RedirectToAction("orderConfirmation", "Cart", new { id = shoppingCartVm.orderHeader.Id });
        }
        
        public IActionResult orderConfirmation(int id)
        {
            return View(id);
        }
    }
}