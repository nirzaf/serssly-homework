using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using serssly.Data;
using serssly.Models;

namespace serssly.Pages.Feeds
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext context;

        public DeleteModel(ApplicationDbContext context)
        {
            this.context = context;
        }

        [BindProperty]
        public Feed? Feed { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feed = await context.Feeds.SingleOrDefaultAsync(m => m.Id == id);
            if (Feed == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userFeed = new UserFeed { UserId = userId, FeedId = id.Value };
            context.Entry(userFeed).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return RedirectToPage("./List");
        }
    }
}
