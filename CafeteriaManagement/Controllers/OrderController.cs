using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using CafeteriaManagement.Data.Repository.IRepository;
using CafeteriaManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeteriaManagement.Utilities;
using Microsoft.AspNetCore.Authorization;


namespace CafeteriaManagement.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment
        ) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment= hostEnvironment;

        public IActionResult RecentOrders()
        {
            IEnumerable<Order> orders = _unitOfWork.Order.GetAll().ToList();
            return View(orders);
        }

        [HttpPost]
        public IActionResult ShowOrderReceipt([FromBody] Order order)
        {
            try
            {
                
                if (order != null && order.Products != null && order.Products.Any())
                {
                    //var productIds = order.Products.Select(prod => prod.Id).ToList();
                    //var existingProducts = _unitOfWork.Product.GetAll(p => productIds.Contains(p.Id));
                    // order.Products = productIds.ToList();
                    Order order1 = new Order();
                    order1.PaymentMethod = order.PaymentMethod;
                    order1.AmountPaid = order.AmountPaid;
                    order1.Balance = order.Balance;
                    order1.TotalPrice = order.TotalPrice;
                    order1.OrderTime = DateTime.Now;
                    _unitOfWork.Order.Add(order1);
                    _unitOfWork.Save();

                    byte[] pdfBytes = GeneratePdfReceipt(order);
                    string wwwRootPath = _hostingEnvironment.WebRootPath;
                    string filePath = Path.Combine(wwwRootPath, "pdfs", $"OrderReceipt_{order1.Id}.pdf");

                    System.IO.File.WriteAllBytes(filePath, pdfBytes);

                    

                    return Ok(new { success = true, filePath });
                }
                else
                {
                    // Handle the case when order or products are not valid
                    return Json(new { success = false, errors = "Invalid order or products" });
                }
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                return StatusCode(500, "An error occurred while processing the order");
            }
        }

        private byte[] GeneratePdfReceipt(Order order)
        {
            var oderno = GenerateOrderNo.GenerateOrderNumber();
            using (var pdfDocument = new PdfDocument())
            {
                var page = pdfDocument.AddPage();
                var gfx = XGraphics.FromPdfPage(page);
                var font = new XFont("Verdana", 10, XFontStyle.Regular);

                int yPosition = 40;
                const int lineHeight = 20; // Set line height for each line of text

                gfx.DrawString("Order Receipt: ", font, XBrushes.Black, new XRect(40, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += lineHeight; // Increment yPosition for the next line

                gfx.DrawString($"Order ID: {oderno}", font, XBrushes.Black, new XRect(40, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += lineHeight;

                gfx.DrawString($"Order Time: {DateTime.Now.ToString()}", font, XBrushes.Black, new XRect(40, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += lineHeight;

                gfx.DrawString("Food Ordered", font, XBrushes.Black, new XRect(40, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Qty.", font, XBrushes.Black, new XRect(180, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Price", font, XBrushes.Black, new XRect(260, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                gfx.DrawString("Total", font, XBrushes.Black, new XRect(340, yPosition, page.Width, page.Height), XStringFormats.TopLeft);

                yPosition += lineHeight;

                XPen pen1 = new XPen(XColors.Black, 1);
                pen1.DashStyle = XDashStyle.Dot;
                gfx.DrawLine(pen1, 40, yPosition, page.Width - 40, yPosition);
                yPosition += 5;

                foreach (var product in order.Products)
                {
                    gfx.DrawString(product.Name, font, XBrushes.Black, new XRect(40, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString(product.Count.ToString(), font, XBrushes.Black, new XRect(180, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString($"{product.Price}", font, XBrushes.Black, new XRect(260, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    gfx.DrawString($"{product.Price * product.Count}", font, XBrushes.Black, new XRect(340, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                    yPosition += lineHeight; // Increment yPosition for the next product line
                }

                yPosition += lineHeight; // Adding extra line spacing

                XPen pen = new XPen(XColors.Black, 1);
                pen.DashStyle = XDashStyle.Dot;
                gfx.DrawLine(pen, 40, yPosition, page.Width - 40, yPosition);
                yPosition += 5;

                gfx.DrawString($"Total Price:  {order.TotalPrice}", font, XBrushes.Black, new XRect(40, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += lineHeight;

                gfx.DrawString($"Amount Paid:  {order.AmountPaid}", font, XBrushes.Black, new XRect(40, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += lineHeight;

                gfx.DrawString($"Balance:  {order.Balance}", font, XBrushes.Black, new XRect(40, yPosition, page.Width, page.Height), XStringFormats.TopLeft);
                yPosition += lineHeight;

                using (var memoryStream = new MemoryStream())
                {
                    pdfDocument.Save(memoryStream, false);
                    return memoryStream.ToArray();
                }
            }
        }
       
    }

}
