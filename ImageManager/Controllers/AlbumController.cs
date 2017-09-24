﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageManager.Common;
using ImageManager.Data.Domains;
using ImageManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ImageManager.Controllers
{
    [Authorize]
    public class AlbumController : Controller
    {
        private readonly AlbumService _albumService;
        private readonly UnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public AlbumController(AlbumService albumService, UserManager<User> userManager, UnitOfWork unitOfWork)
        {
            _albumService = albumService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _albumService.GetUserAlbums(user.Id).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Album model, List<IFormFile> files)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Images = new List<Image>();
            foreach (var file in files)
            {
                var filePath = $"{Constant.UploadPath}/{DateTime.Now.ToFileTime()}_{file.FileName}";
                using (var stream = new FileStream($"{Constant.RootPath}/{filePath}", FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    model.Images.Add(new Image
                    {
                        Path = filePath
                    });
                }
            }
            model.User = await _userManager.GetUserAsync(User);
            await _albumService.AddAsync(model);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index", "Image", new {albumId = model.Id});
        }
    }
}