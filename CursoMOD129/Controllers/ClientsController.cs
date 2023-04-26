using CursoMOD129.Data;
using CursoMOD129.Models;
using Microsoft.AspNetCore.Mvc;

namespace CursoMOD129.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // Get: Clients/[Index]
        public IActionResult Index() 
        {
            var clients = _context.Clients.ToList();

            return View(clients);
        }

        // Get: Clients/Create
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Client newClient)
        {
            if (ModelState.IsValid) 
            {
                _context.Clients.Add(newClient);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(newClient);
        }

        public IActionResult Edit(int id)
        {
            Client? client = _context.Clients.Find(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost]
        public IActionResult Edit(Client editingClient)
        {
            if (ModelState.IsValid)
            {
                _context.Clients.Update(editingClient);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(editingClient);
        }

        public IActionResult Delete(int id)
        {
            Client? client = _context.Clients.Find(id);

            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Client? deletingClient = _context.Clients.Find(id);
            
            if (deletingClient != null)
            {
                _context.Clients.Remove(deletingClient);
                _context.SaveChanges();
            }
            
            return RedirectToAction(nameof(Index));
        }



    }
}
