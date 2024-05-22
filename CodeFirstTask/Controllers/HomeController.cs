using CodeFirstTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CodeFirstTask.Controllers
{
    public class HomeController : Controller
    {
        #region Fields

        private readonly StudentDBContext dbContext;
        private readonly IWebHostEnvironment _environment;

        #endregion

        #region Ctor

        public HomeController(StudentDBContext _dbContext, IWebHostEnvironment _environment)
        {
            dbContext = _dbContext;
            this._environment = _environment;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Index()
        {
            List<Student> list = await dbContext.Students.ToListAsync();
            return View(list);
        }

        private List<CheckBoxOption> getDataFrom()
        {
            List<CheckBoxOption> e = new List<CheckBoxOption>
                {
                    new CheckBoxOption {
                        IsChecked = false,
                        Text = "Reading",
                        Value = "Reading",
                    },
                     new CheckBoxOption {
                        IsChecked = false,
                        Text = "Music",
                        Value = "Music",
                    },
                      new CheckBoxOption {
                        IsChecked = false,
                        Text = "Travelling",
                        Value = "Travelling",
                    }
                };
            return e;
        }

        public IActionResult Create()
        {
            StudentCopy model = new StudentCopy();
            List<CheckBoxOption> m = getDataFrom();
            model.checkBoxHobbiesList = m;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCopy model)
        {
            if (!ModelState.IsValid)
            {

                if (model.UploadedFile != null && model.UploadedFile.Length > 0)
                {
                    try
                    {
                        var uploadsDirectory = Path.Combine(_environment.WebRootPath, "uploads");

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadedFile.FileName;

                        var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

                        if (!Directory.Exists(uploadsDirectory))
                        {
                            Directory.CreateDirectory(uploadsDirectory);
                        }

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.UploadedFile.CopyToAsync(fileStream);
                        }
                        var fruits = string.Join(",", model.hobbies);

                        Student student = new Student()
                        {
                            ID = model.ID,
                            StudentName = model.StudentName,
                            Standard = model.Standard,
                            hobbies = fruits,
                            Gender = model.Gender,
                            PhoneNumber = model.PhoneNumber,
                            ProfilePicture = uniqueFileName,
                        };

                        await dbContext.AddAsync(student);
                        await dbContext.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                    catch (IOException ex)
                    {
                        ModelState.AddModelError("UploadedFile", $"Error saving file: {ex.Message}");
                        return View("Index", model);
                    }
                }

            }

            StudentCopy e = model;
            e.checkBoxHobbiesList = getDataFrom();
            return View(e);

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return RedirectToAction("Index");
            }

            List<CheckBoxOption> options = getDataFrom();
            List<string> list = getSelectedList(options,student.hobbies);

            StudentCopy copyModel = new StudentCopy()
            {
                ID = student.ID,
                StudentName = student.StudentName,
                Standard = student.Standard,
                checkBoxHobbiesList = options,
                hobbies = list,
                Gender = student.Gender,
                PhoneNumber = student.PhoneNumber,
                ProfilePicture=student.ProfilePicture
            };
            ViewBag.ImageFile = student.ProfilePicture;
            return View(copyModel);
        }

        private List<string> getSelectedList(List<CheckBoxOption> options, string hobbies)
        {
            List<string> list = new List<string>();
            string[] hobbiesList=hobbies.Split(",");
            foreach (var item in options)
            {
                if (hobbiesList.Contains(item.Text))
                {
                    list.Add(item.Text);
                }
            }

            return list;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StudentCopy copyModel)
        {

            var studentModel = await dbContext.Students.FindAsync(id);

            if (studentModel == null)
            {
                return RedirectToAction("Index");
            }


            if (ModelState.IsValid)
            {

                if (copyModel.UploadedFile != null)
                {
                    var uploadsDirectory = Path.Combine(_environment.WebRootPath, "uploads");

                    var newFileName = Guid.NewGuid().ToString() + "_" + copyModel.UploadedFile.FileName;

                    var filePath = Path.Combine(uploadsDirectory, newFileName);

                    if (!Directory.Exists(uploadsDirectory))
                    {
                        Directory.CreateDirectory(uploadsDirectory);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await copyModel.UploadedFile.CopyToAsync(fileStream);
                    }

                    string oldPath = _environment.WebRootPath + "/uploads/" + studentModel.ProfilePicture;
                    System.IO.File.Delete(oldPath);

                    var fruits = string.Join(",", copyModel.hobbies);
                    studentModel.StudentName = copyModel.StudentName;
                    studentModel.Standard = copyModel.Standard;
                    studentModel.hobbies = fruits;
                    studentModel.Gender = copyModel.Gender;
                    studentModel.PhoneNumber = copyModel.PhoneNumber;
                    studentModel.ProfilePicture = newFileName;


                    dbContext.Update(studentModel);
                    await dbContext.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }



            return View(copyModel);

        }

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || dbContext == null)
            {
                return NotFound();
            }
            var student = await dbContext.Students.FirstOrDefaultAsync(x => x.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost, ActionName("View")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> View(int? id)
        {
            var student = await dbContext.Students.FindAsync(id);

            if (student != null)
            {
                dbContext.Students.Remove(student);
                await dbContext.SaveChangesAsync();
                TempData["deleteSuccess"] = "Delete...";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion
    }
}
