using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SampleWebApp.Data;
using SampleWebApp.Models;

namespace SampleWebApp.Controllers
{
    public class SamplesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SamplesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Samps
        public async Task<IActionResult> Index()
        {
              return _context.Sample != null ? 
                          View(await _context.Sample.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Sample'  is null.");
        }


        // GET: Samples/ ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST:  Samples/ ShowSearchResult
        public async Task<IActionResult>  ShowSearchResult( String SearchPhrase)
        {
            return  View ( "index",  await _context.Sample.Where( t => t.SampleQuestion.Contains(SearchPhrase)).ToListAsync());
                       
        }


        //GET CREATE


        [Microsoft.AspNetCore.Authorization.Authorize]

        public IActionResult Create()
        {
            return View();
        }





        // GET: Samples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sample == null)
            {
                return NotFound();
            }

            var sample = await _context.Sample
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sample == null)
            {
                return NotFound();
            }

            return View(sample);
        }

        // GET: Samples/Create
     

        // POST: Samples/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SampleQuestion,SampleAnswer")] Sample sample)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sample);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sample);
        }

        // GET: Samples/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sample == null)
            {
                return NotFound();
            }

            var sample = await _context.Sample.FindAsync(id);
            if (sample == null)
            {
                return NotFound();
            }
            return View(sample);
        }

        // POST: Samples/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SampleQuestion,SampleAnswer")] Sample sample)
        {
            if (id != sample.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sample);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SampleExists(sample.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sample);
        }

        // GET: Samples/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sample == null)
            {
                return NotFound();
            }

            var sample = await _context.Sample
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sample == null)
            {
                return NotFound();
            }

            return View(sample);
        }

        // POST: Samples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sample == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Sample'  is null.");
            }
            var sample = await _context.Sample.FindAsync(id);
            if (sample != null)
            {
                _context.Sample.Remove(sample);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SampleExists(int id)
        {
          return (_context.Sample?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
