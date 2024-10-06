using DataSetBase.Models;
using Microsoft.EntityFrameworkCore;

namespace DataSetBase.Database
{
    public interface ICategoryRepo
    {
        public CategoryModel Get(int ID, bool getWithDeep = false);
        public List<CategoryModel> Gets(bool getWithDeep = false);
        public CategoryModel Create(CategoryModel _m, int parentID = -1);
        public void Del(int ID);
    }

    public class CategoryRepo : ICategoryRepo
    {
        public DbSet<CategoryModel> Category { get; set; }

        public SystemDBContext context;

        public CategoryRepo(SystemDBContext _context)
        {
            this.context = _context;
            this.context.StartUp();
        }

        public CategoryModel Get(int ID, bool getWithDeep = false)
        {
            CategoryModel c = this.context.Category.Include(x => x.ParentCategories).Include(x => x.SubCategories).FirstOrDefault(x => x.CategoryID == ID);
            if (c != null)
            {
                c.parentCategories = c.ParentCategories.ToList();
                c.subCategories = c.SubCategories.ToList();

                if (getWithDeep)
                {
                    c.parentCategories.ForEach(x =>
                    {
                        x = this.Get(x.CategoryID);
                    });
                }

            }

            return c;
        }

        public void Del(int ID)
        {
            CategoryModel m = this.Get(ID);
            if (m != null)
            {
                this.context.Remove(m);
            }
        }

        public List<CategoryModel> Gets(bool getWithDeep = false)
        {
            List<CategoryModel> cats = new List<CategoryModel>();
            cats = this.context.Category.ToList();

            return cats;
        }

        public CategoryModel Create(CategoryModel _m, int parentID = -1)
        {
            CategoryModel m = new CategoryModel() { Name = _m.Name, Description = _m.Description };
            m.parentCategories = [];

            if(parentID > -1)
            {
                CategoryModel c = this.Get(parentID);
                m.ParentCategories.Add(c);
            }

            m.parentCategories = m.ParentCategories.ToList();

            this.context.Category.Add(m);
            this.context.SaveChanges();
            return m;
        }

    }
}
