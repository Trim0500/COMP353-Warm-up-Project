﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYVCApp.Contexts;
using MYVCApp.Models;

namespace MYVCApp.Controllers
{
    public class TeammembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeammembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teammembers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Teammembers.Include(t => t.CmnFkNavigation).Include(t => t.TeamFormationIdFkNavigation);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Teammembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teammembers == null)
            {
                return NotFound();
            }

            var teammember = await _context.Teammembers
                .Include(t => t.CmnFkNavigation)
                .Include(t => t.TeamFormationIdFkNavigation)
                .FirstOrDefaultAsync(m => m.TeamFormationIdFk == id);
            if (teammember == null)
            {
                return NotFound();
            }

            return View(teammember);
        }

        // GET: Teammembers/Create
        public IActionResult Create()
        {
            ViewData["CmnFk"] = new SelectList(_context.Clubmembers, "Cmn", "Cmn");
            ViewData["TeamFormationIdFk"] = new SelectList(_context.Teamformations, "Id", "Id");
            return View();
        }

        // POST: Teammembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamFormationIdFk,CmnFk,Role,AssignmentDateTime")] Teammember teammember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teammember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CmnFk"] = new SelectList(_context.Clubmembers, "Cmn", "Cmn", teammember.CmnFk);
            ViewData["TeamFormationIdFk"] = new SelectList(_context.Teamformations, "Id", "Id", teammember.TeamFormationIdFk);
            return View(teammember);
        }

        // GET: Teammembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teammembers == null)
            {
                return NotFound();
            }

            var teammember = await _context.Teammembers.FindAsync(id);
            if (teammember == null)
            {
                return NotFound();
            }
            ViewData["CmnFk"] = new SelectList(_context.Clubmembers, "Cmn", "Cmn", teammember.CmnFk);
            ViewData["TeamFormationIdFk"] = new SelectList(_context.Teamformations, "Id", "Id", teammember.TeamFormationIdFk);
            return View(teammember);
        }

        // POST: Teammembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamFormationIdFk,CmnFk,Role,AssignmentDateTime")] Teammember teammember)
        {
            if (id != teammember.TeamFormationIdFk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teammember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeammemberExists(teammember.TeamFormationIdFk))
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
            ViewData["CmnFk"] = new SelectList(_context.Clubmembers, "Cmn", "Cmn", teammember.CmnFk);
            ViewData["TeamFormationIdFk"] = new SelectList(_context.Teamformations, "Id", "Id", teammember.TeamFormationIdFk);
            return View(teammember);
        }

        // GET: Teammembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teammembers == null)
            {
                return NotFound();
            }

            var teammember = await _context.Teammembers
                .Include(t => t.CmnFkNavigation)
                .Include(t => t.TeamFormationIdFkNavigation)
                .FirstOrDefaultAsync(m => m.TeamFormationIdFk == id);
            if (teammember == null)
            {
                return NotFound();
            }

            return View(teammember);
        }

        // POST: Teammembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teammembers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Teammembers'  is null.");
            }
            var teammember = await _context.Teammembers.FindAsync(id);
            if (teammember != null)
            {
                _context.Teammembers.Remove(teammember);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeammemberExists(int id)
        {
          return (_context.Teammembers?.Any(e => e.TeamFormationIdFk == id)).GetValueOrDefault();
        }
    }
}
