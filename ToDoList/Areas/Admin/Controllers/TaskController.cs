using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ToDoList.DataAccess;
using ToDoList.Models;

namespace ToDoList.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaskController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public ActionResult NotFoundPage()
        {
            Response.StatusCode = 404;
            return View();
        }

        public IActionResult Index()
        {
            var tasks = dbContext.Tasks;
            return View(tasks.ToList());
        }
        public IActionResult ViewTasks()
        {
            var tasks = dbContext.Tasks;
            return View(tasks.ToList());
        }
        public IActionResult Delete(int id)
        {
            var task = dbContext.Tasks.FirstOrDefault(e => e.Id == id);
            dbContext.Tasks.Remove(task);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            //Validation
            var tasks = dbContext.Tasks;
            ViewData["Tasks"] = tasks.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(ToDoTask task, IFormFile file)
        {
            //Validation
            if (file !=null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\attachment", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                task.Attachment = fileName;
            }
            if (task != null)
            {
                dbContext.Tasks.Add(task);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("NotFoundPage", "Task");
        }
        [HttpGet]
        public IActionResult Edit(int TaskId)
        {
            //Validation
            var task = dbContext.Tasks.FirstOrDefault(e => e.Id == TaskId);
            if(task != null)
            {
                return View(task);
            }
            return RedirectToAction("NotFoundPage", "Task");

        }
        [HttpPost]
        public IActionResult Edit(ToDoTask task, IFormFile file)
        {
            var taskinDB = dbContext.Tasks.AsNoTracking().FirstOrDefault(e => e.Id == task.Id);

            if (taskinDB == null)
            {
                return RedirectToAction("NotFoundPage", "Task");
            }

            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "attachment", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(taskinDB.Attachment))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "attachment", taskinDB.Attachment);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                task.Attachment = fileName;
            }
            else
            {
                task.Attachment = taskinDB.Attachment;
            }
            if (Request.Form.TryGetValue("IsCompleted", out var isCompletedValue))
            {
                taskinDB.IsCompleted = isCompletedValue == "true";
            }

            dbContext.Tasks.Update(task);
            dbContext.SaveChanges();

            return RedirectToAction(nameof(ViewTasks));
        }
        public IActionResult DownloadFile(int id)
        {
            var tasks = dbContext.Tasks.AsNoTracking().FirstOrDefault(e => e.Id == id);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\attachment", tasks.Attachment);

            if (!System.IO.File.Exists(filePath))
            {
                return RedirectToAction("NotFoundPage", "Home");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var contentType = "application/pdf";
            return File(fileBytes, contentType, tasks.Attachment); 
        }

    }
}
