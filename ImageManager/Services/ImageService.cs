﻿using System.Collections.Generic;
using System.Linq;
using ImageManager.Data;
using ImageManager.Data.Domains;
using Microsoft.EntityFrameworkCore;

namespace ImageManager.Services
{
    public class ImageService : Service<Image>
    {
        public ImageService(NeptuneContext context) : base(context)
        {
        }

        public IEnumerable<Image> GetAll(string userId)
        {
            return DbSet.Include(x => x.Album).Where(x => x.Album.User.Id == userId);
        }
    }
}