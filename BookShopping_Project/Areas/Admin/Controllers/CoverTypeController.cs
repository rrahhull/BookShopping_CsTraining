using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using BookShoppinhg_Project.DataAccess.Repository.IRepository;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopping_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller

    {
        private readonly IUnitOfWork _UnitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)

        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)
                return View (coverType);
            var param = new DynamicParameters();
            param.Add("@id", id.GetValueOrDefault());
            coverType = _UnitOfWork.SP_CALL.OneRecord<CoverType>(SD.proc_CoverType_GetCoverType, param);
            //coverType = _UnitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (coverType == null)
                return NotFound();
            return View(coverType);

        }
        [HttpPost]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null)
                return NotFound();
            if (!ModelState.IsValid)
                return View(coverType);
            var param = new DynamicParameters();
            param.Add("@name", coverType.name);
            if (coverType.id == 0)
                _UnitOfWork.SP_CALL.Execute(SD.Proc_CoverType_Create, param);
            else
            {
                param.Add("@id", coverType.id);
                _UnitOfWork.SP_CALL.Execute(SD.Proc_CoverType_Update, param);
            }
            //if (coverType.id == 0)
            //    _UnitOfWork.CoverType.Add(coverType);
            //else
            //    _UnitOfWork.CoverType.update(coverType);
            //_UnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _UnitOfWork.CoverType.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var CoverTypeInDb = _UnitOfWork.CoverType.Get(id);
                if (CoverTypeInDb == null)
                    return Json(new { success = false, message = "Error while Delete Data.." });
            var param = new DynamicParameters();
            param.Add("@id", id);
            _UnitOfWork.SP_CALL.Execute(SD.Proc_CoverType_Delete, param);
            /*_UnitOfWork.CoverType.remove(CoverTypeInDb);
            _UnitOfWork.Save();*/
            return Json(new { success = true, message = "Data Deleted Successfully" });
        }
        #endregion
    }
}

