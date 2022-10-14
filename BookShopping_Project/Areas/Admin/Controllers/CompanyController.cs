using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using BookShoppinhg_Project.DataAccess.Repository;
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
    [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null)
                return View(company);
            company = _unitOfWork.company.Get(id.GetValueOrDefault());
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (company == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(company);
            if (company.Id == 0)
                _unitOfWork.company.Add(company);
            else
                _unitOfWork.company.Update(company);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }            
        

        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            var CompanyList = _unitOfWork.company.GetAll();
            return Json(new { data = CompanyList });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CompanyInDb = _unitOfWork.company.Get(id);
            if (CompanyInDb == null)
                return (Json(new { success = false, message = "Error while Deleting data!!!" }));
            else
                _unitOfWork.company.remove(CompanyInDb);
            _unitOfWork.Save();
            return (Json(new { success = true, message = "Data Deleted Successfully!!!" }));
        }
        #endregion
    }
}
