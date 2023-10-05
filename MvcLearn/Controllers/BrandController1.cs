using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcLearn.Data;
using MvcLearn.Models;

namespace MvcLearn.Controllers
{
    public class BrandController1 : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController1(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _dbContext.table.ToList();
            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            string webrootpath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webrootpath, @"image\brand");
                var extension = Path.GetExtension(file[0].FileName);
                var filePath = Path.Combine(upload, newFileName + extension);

                // Create the directory if it doesn't exist
                Directory.CreateDirectory(upload);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                    brand.BrandLogo = @"\images\brand\" + newFileName + extension;
                }
            }


            if (ModelState.IsValid)
            {
                _dbContext.table.Add(brand);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View();

        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _dbContext.table.FirstOrDefault(x => x.Id == id);

            return View(brand);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            Brand brand=_dbContext.table.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            string webrootpath = _webHostEnvironment.WebRootPath;
            var file = HttpContext.Request.Form.Files;

            if (file.Count > 0)
            {
                string newFileName = Guid.NewGuid().ToString();
                var upload = Path.Combine(webrootpath, @"image\brand");
                var extension = Path.GetExtension(file[0].FileName);

                //Delete Old Image

                var objfromDb = _dbContext.table.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if(objfromDb.BrandLogo != null)
                {
                    var OldImagepath = Path.Combine(webrootpath, objfromDb.BrandLogo.Trim('\\'));
                    if (System.IO.File.Exists(OldImagepath))
                    {
                        System.IO.File.Delete(OldImagepath);
                    }

                }


                var filePath = Path.Combine(upload, newFileName + extension);

                // Create the directory if it doesn't exist
                Directory.CreateDirectory(upload);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file[0].CopyTo(fileStream);
                    brand.BrandLogo = @"\images\brand\" + newFileName + extension;
                }
            }
            if (ModelState.IsValid)
            {
                var objfromDb = _dbContext.table.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                objfromDb.Name = brand.Name;
                objfromDb.EstablishedYear = brand.EstablishedYear;

                if(brand.BrandLogo != null){
                    objfromDb.BrandLogo = brand.BrandLogo;
                }
                _dbContext.table.Update(brand);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            Brand brand = _dbContext.table.FirstOrDefault(x => x.Id == id);
            return View(brand);
        }

        [HttpPost]

        public IActionResult Delete(Brand brand)
        {
            string webrootpath = _webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(brand.BrandLogo))
            {
                var objFromDb=_dbContext.table.AsNoTracking().FirstOrDefault(x => x.Id == brand.Id);

                if(objFromDb.BrandLogo != null)
                {
                    var oldImagepath=Path.Combine(webrootpath, objFromDb.BrandLogo.Trim('\\'));
                    if (System.IO.File.Exists(oldImagepath))
                    {
                        System.IO.File.Delete(oldImagepath);
                    }
                } 
            }
            _dbContext.table.Remove(brand);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
