using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using System;
using System.Collections.Generic;

namespace SampleMVC.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleBLL _articleBLL;
        private readonly ICategoryBLL _categoryBLL;
        private readonly IMapper _mapper;

        public ArticleController(IArticleBLL articleBLL, ICategoryBLL categoryBLL, IMapper mapper)
        {
            _articleBLL = articleBLL;
            _categoryBLL = categoryBLL;
            _mapper = mapper;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 5, string search = "", string act = "", int? categoryId = null)
        {
            try
            {
                if (act == "next")
                {
                    pageNumber++;
                }
                else if (act == "prev" && pageNumber > 1)
                {
                    pageNumber--;
                }
                else if (act == "first") {
                    pageNumber = 1;
                }

                var articles = _articleBLL.GetArticlesWithPaging(pageNumber, pageSize, search, categoryId);
                var articleDTOs = _mapper.Map<IEnumerable<ArticleDTO>>(articles);

                ViewData["pageNumber"] = pageNumber;

                var categories = _categoryBLL.GetAll();
                ViewBag.Categories = categories;

                return View(articleDTOs);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while retrieving articles: " + ex.Message;
                return View();
            }
        }

        public IActionResult Create()
        {
            var categories = _categoryBLL.GetAll();
            ViewBag.Categories = categories;

            return View(new ArticleCreateDTO());
        }

        [HttpPost]
        public IActionResult Create(ArticleCreateDTO articleCreate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _articleBLL.Insert(articleCreate);
                    TempData["SuccessMessage"] = "Article created successfully.";
                    return RedirectToAction("Index", "Article");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error occurred while creating the article: " + ex.Message;
            }

            return View(articleCreate);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var model = _articleBLL.GetArticleById(id);
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while retrieving the article for editing: " + ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, ArticleUpdateDTO articleUpdate)
        {
            try
            {
                articleUpdate.ArticleID = id;
                _articleBLL.Update(articleUpdate);
                TempData["SuccessMessage"] = "Article updated successfully.";
                return RedirectToAction("Index", "Article");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error occurred while updating the article: " + ex.Message;
                return View(articleUpdate);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _articleBLL.Delete(id);
                TempData["SuccessMessage"] = "Article deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error occurred while deleting the article: " + ex.Message;
            }

            return RedirectToAction("Index", "Article");
        }
    }
}
