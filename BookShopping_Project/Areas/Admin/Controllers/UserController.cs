using BookShopping_Project.DataAccess.Data;
using BookShopping_Project.Models;
using BookShopping_Project.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopping_Project.Areas.Admin.Controllers

{
    [Area("Admin")]
    

    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
       [HttpGet]
           public IActionResult GetAll()
        {
            var UserList = _context.applicationUsers.Include(c => c.company).ToList();
            var userrole = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();
            foreach(var User in UserList)
            {
                var RoleId = userrole.FirstOrDefault(u => u.UserId == User.Id).RoleId;
                User.Role = roles.FirstOrDefault(r => r.Id == RoleId).Name;
                if (User.company == null)
                {
                    User.company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            if (!User.IsInRole(SD.Role_Admin))
            {
                var UserAdmin = UserList.FirstOrDefault(u => u.Role == SD.Role_Admin);
                UserList.Remove(UserAdmin);
            }
            return Json(new { data = UserList });
        }
        [HttpPost]
        public IActionResult lockUnlock([FromBody] string id)
        {
            bool islocked = false;
            var UserInDb = _context.applicationUsers.FirstOrDefault(u => u.Id == id);
            if (UserInDb == null)
                return Json(new { success = false, message = "Error while lock and unlock the User!!" });
            if(UserInDb!=null && UserInDb.LockoutEnd > DateTime.Now)
            {
                UserInDb.LockoutEnd = DateTime.Now;
                islocked = false;
            }
            else
            {
                UserInDb.LockoutEnd = DateTime.Now.AddYears(20);
                islocked = true;
            }
            _context.SaveChanges();
            return Json(new { success = true, message = islocked == true ? "User Successfully Locked" : "User Successfully Unlocked" });
        }
        #endregion
    }
}
