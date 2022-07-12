using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using codebiz_employee_management.Models;
using System.Data;

namespace codebiz_employee_management.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetData()
        {
            using (DBModel db = new DBModel())
            {
                List<employee> empList = db.employees.ToList<employee>();
                return Json(new { data = empList }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new employee());
            else
            {
                using (DBModel db = new DBModel())
                {
                    return View(db.employees.Where(x => x.employeeId == id).FirstOrDefault<employee>());
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(employee emp)
        {
            using (DBModel db = new DBModel())
            {
                if (emp.employeeId == 0)
                {
                    db.employees.Add(emp);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully!" }, JsonRequestBehavior.AllowGet);
                }
                //try
                //{
                //    db.employees.Add(emp);
                //    db.SaveChanges();
                //}
                //catch (DbEntityValidationException ex)
                //{
                //    foreach(var entityValidationErrors in ex.EntityValidationErrors)
                //    {
                //        foreach (var validationError in entityValidationErrors.ValidationErrors)
                //        {
                //            Response.Write("Property: " + validationError.PropertyName + "Error: " + validationError.ErrorMessage);
                //        }
                //    }
                //}
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using(DBModel db = new DBModel())
            {
                employee emp = db.employees.Where(x => x.employeeId == id).FirstOrDefault<employee>();
                db.employees.Remove(emp);
                db.SaveChanges();
                return Json(new { success = true, message = "Delete Successfully!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}