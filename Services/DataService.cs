using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoviePro.Data;
using MoviePro.Services.Interfaces;
using System.IO;
using System;
using System.Threading.Tasks;

namespace MoviePro.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IConfiguration _configuration;

        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IImageService imageService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _imageService = imageService;
            _configuration = configuration;
        }

        public async Task ManageDataAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }
    }
}
