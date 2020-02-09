using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CRUDExample.Models;

namespace CRUDExample
{
    public class IndexModel : PageModel
    {
        private readonly JournalDbContext _context;

        public IndexModel(JournalDbContext context)
        {
            _context = context;
        }

        public IList<DailyRecord> DailyRecord { get; set; }

        /// <summary>
        /// 畫面載入動作
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public async Task OnGetAsync(int? year = null, int? month = null)
        {
            year = year ?? DateTime.Today.Year;
            // C# 8.0
            month ??= DateTime.Today.Month;
            var startDate = new DateTime(year.Value, month.Value, 1);

            DailyRecord = await _context.Records
                .Where(x => x.Date >= startDate && x.Date < startDate.AddMonths(1))
                .OrderBy(x => x.Date).ToListAsync();
        }
    }
}
