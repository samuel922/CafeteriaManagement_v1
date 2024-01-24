using CafeteriaManagement.Data.Repository.IRepository;
using CafeteriaManagement.Models;
using CafeteriaManagement.Models.ViewModels;
using CafeteriaManagement.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaManagement.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            CategoryVM categoryVM = new()
            {
                Categories = _unitOfWork.Category.GetAll().ToList(),
            };

            return View(categoryVM);
        }

        public IActionResult Upsert(int? id)
        {
            CategoryVM categoryVM = new()
            {
                Category = new Category()
            };

            if (id != null)
            {
                //Update
                categoryVM.Category = _unitOfWork.Category.Get(u => u.Id == id);
                return View(categoryVM);
            }
            else
            {
                //Create
                return View(categoryVM);
            }

            
        }

        [HttpPost]
        public IActionResult Upsert(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                if (categoryVM.Category.Id != 0)
                {
                    _unitOfWork.Category.Update(categoryVM.Category);
                    TempData["succes"] = "Category Updated Successfully";
                }
                else
                {
                    _unitOfWork.Category.Add(categoryVM.Category);
                    TempData["succes"] = "Category Created Successfully";
                }
                
                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }

            return View(categoryVM);
        }

        #region APIs

        [HttpGet]
        public IActionResult GetAll()
        {
            var categoryList = _unitOfWork.Category.GetAll().ToList();

            return Json(new { data = categoryList });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var categoryToBeDeleted = _unitOfWork.Category.Get(u => u.Id == id);

            if (categoryToBeDeleted == null)
            {
                return Json(new { success = false, message = true });
            }

            _unitOfWork.Category.Remove(categoryToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Category Deleted successfully" });
        }

        #endregion


    }
}
