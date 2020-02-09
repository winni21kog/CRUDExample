using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDExample.Models;

namespace CRUDExample
{
    public class EditModel : PageModel
    {
        private readonly CRUDExample.Models.JournalDbContext _context;

        public EditModel(CRUDExample.Models.JournalDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DailyRecord DailyRecord { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DailyRecord = await _context.Records.FirstOrDefaultAsync(m => m.Id == id);

            if (DailyRecord == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Razor Page Scaffolding
            //_context.Attach(DailyRecord).State = EntityState.Modified;

            // 由資料庫取得修改前資料
            var origin = _context.Records.SingleOrDefault(o => o.Id == DailyRecord.Id);
            if (origin == null) return NotFound();

            // 檢查日期是否被更動，若是則拒絕更新 早於:<0 等於=0 晚於>0
            if (origin.Date.CompareTo(DailyRecord.Date) != 0)
            {
                ModelState.AddModelError("DailyRecord.Date", "日期不可修改");
                return Page();
            }

            // 正向表列方式更新欄位，只更新有異動的部分
            origin.Status = DailyRecord.Status;
            origin.EventSummary = DailyRecord.EventSummary;
            origin.Remark = DailyRecord.Remark;
            origin.User = DailyRecord.User;

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool DailyRecordExists(int id)
        {
            return _context.Records.Any(e => e.Id == id);
        }
    }
}
