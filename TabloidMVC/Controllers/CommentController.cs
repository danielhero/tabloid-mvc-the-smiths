﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentRepository _commentRepo;
        private readonly PostRepository _postRepo;
        public CommentController(IConfiguration config)
        {
            _commentRepo = new CommentRepository(config);
            _postRepo = new PostRepository(config);
        }
        // GET: CommentController
        public ActionResult Index(int id)
        {
            var vm = new PostComments();
            vm.Comment = _commentRepo.GetCommentsByPostId(id);
            vm.Post = _postRepo.GetPublisedPostById(id);
            return View(vm);
        }


        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
            var Comment = new Comment();
            Comment.PostId = id;
            return View(Comment);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment comment)
        {
            try
            {
                comment.CreateDateTime = DateAndTime.Now;
                comment.UserProfileId = GetCurrentUserProfileId();
                _commentRepo.AddComment(comment);
                return RedirectToAction("Index", new {id = comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: CommentController/Edit/5
        public IActionResult Edit(int id)
        {
            var comment = _commentRepo.GetCommentById(id);

            if (comment == null || comment.UserProfileId != GetCurrentUserProfileId())
            {
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepo.UpdateComment(comment);

                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        // GET: CommentController/Delete/5
        public IActionResult Delete(int id)
        {
            var comment = _commentRepo.GetCommentById(id);

            if (comment == null || comment.UserProfileId != GetCurrentUserProfileId())
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Comment comment)
        {
            try
            {
                _commentRepo.DeleteComment(comment);
                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch (Exception ex)
            {
                return View(comment);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
