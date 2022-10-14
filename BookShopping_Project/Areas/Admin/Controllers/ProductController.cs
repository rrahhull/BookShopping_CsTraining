using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.Models;
using BookShopping_Project.Models.ViewModels;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopping_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.name,
                    Value = cl.id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(ct => new SelectListItem()
                {
                    Text = ct.name,
                    Value = ct.id.ToString()
                })
            };
            if (id == null)
                return View(productVM);
            //productVM.product = _unitOfWork.Product.FirstorDefault(p => p.id == id.GetValueOrDefault(), IncludeProperties: "Category,CoverType");
            productVM.product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var WebRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(WebRootPath, @"Images/Products");
                    var Extension = Path.GetExtension(files[0].FileName);
                    if (productVM.product.id != 0)
                    {
                        var ImageExist = _unitOfWork.Product.Get(productVM.product.id).ImageUrl;
                        productVM.product.ImageUrl = ImageExist;
                    }
                    if (productVM.product.ImageUrl != null)
                    {
                        var ImagePath = Path.Combine(WebRootPath, productVM.product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(ImagePath))
                        {
                            System.IO.File.Delete(ImagePath);
                        }
                    }
                    using (var FileStream = new FileStream(Path.Combine(uploads, fileName + Extension), FileMode.Create))
                    {
                        files[0].CopyTo(FileStream);
                    }
                    productVM.product.ImageUrl = @"/Images/Products/" + fileName + Extension;
                }

                else
                {
                    if (productVM.product.id != 0)
                    {
                        var ImageExists = _unitOfWork.Product.Get(productVM.product.id).ImageUrl;
                        productVM.product.ImageUrl = ImageExists;
                    }
                }
 
                if (productVM.product.id == 0)
                    _unitOfWork.Product.Add(productVM.product);
                else
                    _unitOfWork.Product.update(productVM.product);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                productVM = new ProductVM()
                {
                    CategoryList = _unitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.name,
                        Value = cl.id.ToString()
                    }),
                    CoverTypeList = _unitOfWork.Category.GetAll().Select(ct => new SelectListItem()
                    {
                        Text = ct.name,
                        Value = ct.id.ToString()
                    })
                };
                if (productVM.product.id != 0)
                {
                    productVM.product = _unitOfWork.Product.Get(productVM.product.id);
                
            }
            return View(productVM);
}
            
        }


        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var ProductList = _unitOfWork.Product.GetAll(IncludeProperties: "Category,CoverType");
            return Json(new { data = ProductList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var ProductInDb = _unitOfWork.Product.Get(id);
            if (ProductInDb == null)
                return Json(new { success = false, message = "Error while Deleting Data!!" });
            if (ProductInDb.ImageUrl == "")
            {
                var WebRootPath = _webHostEnvironment.WebRootPath;
                var ImagePath = Path.Combine(WebRootPath, ProductInDb.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(ImagePath)){
                    System.IO.File.Delete(ImagePath);
                }
            }
            _unitOfWork.Product.remove(ProductInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!" });
        }
        #endregion
    }
}
