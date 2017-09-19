﻿using System.Collections.Generic;
using System.Linq;
using ImageManager.Data;
using ImageManager.Data.Domains;
using Microsoft.EntityFrameworkCore;

namespace ImageManager.Services
{
    public class AlbumService : Service<Album>
    {
        public AlbumService(NeptuneContext context) : base(context)
        {
        }

        public override IEnumerable<Album> GetAll()
        {
            return DbSet.Include(x => x.Images);
        }

        public IEnumerable<Album> GetAll(string userId)
        {
            return DbSet.Include(x => x.Images).Where(x => x.User.Id == userId);
        }
    }
}