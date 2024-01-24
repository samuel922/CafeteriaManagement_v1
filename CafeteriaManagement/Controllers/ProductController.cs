using CafeteriaManagement.Data.Repository.IRepository;
using CafeteriaManagement.Models;
using CafeteriaManagement.Models.ViewModels;
using CafeteriaManagement.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeteriaManagement.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ProductVM productVM = new()
            {
                Products = _unitOfWork.Product.GetAll().ToList()
            };

            return View(productVM);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => 
                    new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                )
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                try
                {
                    productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                }
                catch(Exception ex)
                {
                    TempData["error"] = "An error occurred while processing the request";
                }
                //update
                
                return View(productVM);
            }
            
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file) {

            try
            {
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string menuPath = Path.Combine(wwwRootPath, @"images\menu");

                        if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                        {
                            //delete the image
                            var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(menuPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        productVM.Product.ImageUrl = @"\images\menu\" + fileName;
                    }

                    if (productVM.Product.Id == 0)
                    {
                        _unitOfWork.Product.Add(productVM.Product);
                        TempData["success"] = "Product created successfully";
                    }
                    else
                    {
                        _unitOfWork.Product.Update(productVM.Product);
                        TempData["success"] = "Product updated successfully";
                    }

                    productVM.Product.Count = 1;
                    _unitOfWork.Save();

                    TempData["success"] = "Product created successfully";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u =>

                        new SelectListItem
                        {
                            Text = u.Name,
                            Value = u.Id.ToString()
                        }
                    );
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(productVM);
        }

        #region APIs
        //API
        [HttpGet]
        public IActionResult GetProduct(int id)
        {
            var product = _unitOfWork.Product.Get(u => u.Id == id);

            return Json(product);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var productsList = _unitOfWork.Product.GetAll().ToList();

            return Json(new { data = productsList });
;        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                //Get the object to be deleted
                var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);

                if (productToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error deleting an object" });
                }

                var productImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
                    productToBeDeleted.ImageUrl!.TrimStart('\\'));

                if (System.IO.File.Exists(productImagePath))
                {
                    System.IO.File.Delete(productImagePath);
                }

                _unitOfWork.Product.Remove(productToBeDeleted);
                _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                TempData["error"] = "Error processing request";
            }
           

            return Json(new { success = true, message = "Product Deleted Successfully" });
        }
        #endregion

    }
}
