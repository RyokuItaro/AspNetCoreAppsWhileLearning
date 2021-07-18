using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestApp.Core;
using TestApp.Data;

namespace testapp.Pages_R2
{
    public class IndexModel : PageModel
    {
        private readonly TestApp.Data.TestAppDbContext _context;

        public IndexModel(TestApp.Data.TestAppDbContext context)
        {
            _context = context;
        }

        public IList<Restaurant> Restaurant { get;set; }

        public async Task OnGetAsync()
        {
            Restaurant = await _context.Restaurants.ToListAsync();
        }
    }
}
