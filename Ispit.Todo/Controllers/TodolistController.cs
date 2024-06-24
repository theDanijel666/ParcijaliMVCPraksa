using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ispit.Todo.Data;
using Ispit.Todo.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ispit.Todo.Controllers
{
    [Authorize]
    public class TodolistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TodolistController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if(user == null)
            {
                return "";
            }
            return user.Id;
        }

        // GET: Todolist
        public async Task<IActionResult> Index()
        {
            var lists=await _context.Todolist.Where(t => t.UserId == GetUserId()).ToListAsync();
            return View(lists);
        }

        // GET: Todolist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todolist = await _context.Todolist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todolist == null)
            {
                return NotFound();
            }
            if(todolist.UserId != GetUserId())
            {
                return NotFound();
            }

            return View(todolist);
        }

        // GET: Todolist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todolist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,UserId,Description")] Todolist todolist)
        {
            todolist.UserId = GetUserId();
            ModelState.Remove("UserId");
            ModelState.Remove("Tasks");
            if (ModelState.IsValid)
            {
                _context.Add(todolist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todolist);
        }

        // GET: Todolist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todolist = await _context.Todolist.FindAsync(id);
            if (todolist == null)
            {
                return NotFound();
            }
            if(todolist.UserId != GetUserId())
            {
                return NotFound();
            }
            return View(todolist);
        }

        // POST: Todolist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,UserId,Description")] Todolist todolist)
        {
            if (id != todolist.Id)
            {
                return NotFound();
            }
            //TODO: FIX edit!
            ModelState.Remove("UserId");
            ModelState.Remove("Tasks");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(todolist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodolistExists(todolist.Id))
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
            return View(todolist);
        }

        // GET: Todolist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todolist = await _context.Todolist
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todolist == null)
            {
                return NotFound();
            }
            if(todolist.UserId != GetUserId())
            {
                return NotFound();
            }

            return View(todolist);
        }

        // POST: Todolist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todolist = await _context.Todolist.FindAsync(id);
            if (todolist != null)
            {
                if(todolist.UserId == GetUserId())
                {
                    _context.Todolist.Remove(todolist);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodolistExists(int id)
        {
            return _context.Todolist.Any(e => e.Id == id);
        }
    }
}
