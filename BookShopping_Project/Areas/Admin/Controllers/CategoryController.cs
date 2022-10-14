using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace BookShopping_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
                return View(category);
            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            return View(category);
        }
        [HttpPost]
        public IActionResult Upsert (Category category)
        {
            if (category == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(category);
            if (category.id == 0)
                _unitOfWork.Category.Add(category);
            else
                _unitOfWork.Category.update(category);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var CategoryList = _unitOfWork.Category.GetAll();
            return Json(new { Data = CategoryList });
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var CategoryInDb = _unitOfWork.Category.Get(Id);
            if (CategoryInDb == null)
                return Json(new { success = false, message = "Error while delete data!!" });
            _unitOfWork.Category.remove(CategoryInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Deleted Successfully!!" });
        }

        #endregion
    }

}
