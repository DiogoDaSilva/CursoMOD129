using CursoMOD129.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CursoMOD129.Controllers
{
    public class TeamMembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamMembersController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        // Get: TeamMembers/Create
        public IActionResult Create()
        {
            ViewData["WorkRoleID"] = new SelectList(_context.WorkRoles, "ID", "Name");
            return View();
        }
    }
}
