using CafeteriaManagement.Data.Repository.IRepository;
using CafeteriaManagement.Models;
using CafeteriaManagement.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaManagement.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {

            List<Order> OrderList = _unitOfWork.Order.GetAll().ToList();

            return View(OrderList);
        }
    }
}
