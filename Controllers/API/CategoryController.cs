using DataSetBase.Models;
using Microsoft.AspNetCore.Mvc;
using DataSetBase.Database;

namespace DataSetBase.Controllers
{
    public class CategoryController : Controller
    {
        ICategoryRepo catRepo;

        public CategoryController(ICategoryRepo _catRepo)
        {
            catRepo = _catRepo;
        }

        [HttpPost("/api/category/create")]
        public JsonResult CreateCategory(string Name, string Description, int? parentID = -1)
        {
            if(Name == null || Description == null)
            {
                return Json(new
                {
                    Query = false,
                    Info = new
                    {
                        Name = (Name == null ? false : true),
                        Description = (Description == null ? false : true),
                    }
                });
            }

            int pID = (int)((parentID != null) ? parentID : -1);
            CategoryModel m = this.catRepo.Create(new CategoryModel(){ Name = Name, Description = Description }, pID);

            return Json(new
            {
                Query = true,
                Info = new
                {
                    Name = m.Name,
                    Description = m.Description,
                    Parents = m.ParentCategories.ToList()
                }
            });
        }

        [HttpPost("/api/category/delete")]
        public JsonResult DeleteCategory(int ID)
        {

            if(this.catRepo.Get(ID) != null)
            {
                this.catRepo.Del(ID);
            }

            return Json(new
            {
                Query = true
            });
        }

        [HttpPost("/api/category/update")]
        public JsonResult UpdateCategory(int ID, string? Name, string? Description)
        {
            return Json(new
            {
                Query = true
            });
        }

        [HttpPost("/api/category/get")]
        public JsonResult GetCategory(int ID)
        {
            CategoryModel c = this.catRepo.Get(ID);

            return Json(new
            {
                Query = (c == null) ? false : true,
                Info = c
            });
        }

        [HttpPost("/api/category/all")]
        public JsonResult AllCategory()
        {
            return Json(new
            {
                Query = true,
                Info = this.catRepo.Gets()
            });
        }

    }
}
