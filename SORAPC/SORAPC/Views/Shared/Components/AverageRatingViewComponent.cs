using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SORAPC_API.Models;

namespace SORAPC.Views.Shared.Components
{
    public class AverageRatingViewComponent : ViewComponent
    {
        private readonly SorapcContext _context;

        public AverageRatingViewComponent(SorapcContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int productId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            double averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            return View(averageRating);
        }
    }
}