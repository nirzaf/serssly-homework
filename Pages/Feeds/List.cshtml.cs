using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using serssly.Data;
using serssly.Models;

namespace serssly.Pages.Feeds
{
    public class ListModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public ListModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IList<Feed> Feeds { get; set; } = null!;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Feeds = await context.UserFeeds.AsNoTracking().Where(uf => uf.UserId == userId).Select(uf => uf.Feed).ToListAsync();
        }
    }
}
