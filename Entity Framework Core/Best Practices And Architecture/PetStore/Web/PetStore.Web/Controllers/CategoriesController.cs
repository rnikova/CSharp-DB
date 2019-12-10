using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PetStore.Services;
using PetStore.Services.Models.Category;
using PetStore.Web.ViewModels.Category;

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

        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = this.categoryService.GetById(id);

            if (category.Name == null)
            {
                return BadRequest();
            }

            var viewModel = new CategoryDetailsViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Decription = category.Description
            };

            if (viewModel.Decription == null)
            {
                viewModel.Decription = "No description";
            }

            return this.View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = this.categoryService.GetById(id);

            if (category.Name == null)
            {
                return BadRequest();
            }

            var viewModel = new CategoryDetailsViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Decription = category.Description
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditInputModel model)
        {
            if (!this.categoryService.IsExist(model.Id))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var categoryServiceModel = new EditCategoryServiceModel
            {
                Id = model.Id,
                Name = model.Name,
                Decription = model.Decription
            };

            this.categoryService.Edit(categoryServiceModel);

            return RedirectToAction("All", "Categories");
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