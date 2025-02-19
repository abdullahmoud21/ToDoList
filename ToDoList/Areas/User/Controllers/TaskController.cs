using Microsoft.AspNetCore.Mvc;
using ToDoList.DataAccess;

namespace ToDoList.Areas.User.Controllers
{
    [Area("User")]
    public class TaskController : Controller
    {
        ApplicationDbContext DbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            var tasks = DbContext.Tasks;
            return View(tasks.ToList());
        }
        public IActionResult DownloadFile(int id)
        {
            var task = DbContext.Tasks.FirstOrDefault(t => t.Id == id);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\attachment", task.Attachment);

            if (!System.IO.File.Exists(filePath))
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/pdf";
            return File(fileBytes, contentType, task.Attachment);
        }


    }
}
