using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LearnDockerWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LearnDockerWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly LearnDockerDbContext _dbContext;

        public IEnumerable<RequestEntry> Requests { get; set; }

        public IndexModel(ILogger<IndexModel> logger, LearnDockerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task OnGetAsync(/*CancellationToken cancellationToken*/)
        {
            Requests = await _dbContext.Requests
                .AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Take(20)
                .ToListAsync();
        }
    }
}
