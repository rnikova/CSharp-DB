using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetStore.Services;
using PetStore.Services.Models.Category;
using PetStore.Web.ViewModels;

namespace PetStore.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var serviceModel = new CreateCategoryServiceModel
            {
                Name = model.Name,
                Description = model.Description
            };

            this.categoryService.Create(serviceModel);

            return this.RedirectToAction("All", "Categories");
        }

        public IActionResult All()
        {
            var categories = categoryService.All()
                .Select(c => new CategoryListingViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToArray();

            return this.View(categories);
        }
    }
}