using Image_manupulation.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_manupulation.Data.Data
{
   public class ApplicationDbcontext:DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }


    }
}
