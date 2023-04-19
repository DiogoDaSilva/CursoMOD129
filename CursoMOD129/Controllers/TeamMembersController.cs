using CursoMOD129.Data;
using CursoMOD129.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var teamMembers = _context
                .TeamMembers
                .ToList();

            return View(teamMembers);
        }

        // Get: TeamMembers/Create
        public IActionResult Create()
        {
            ViewData["WorkRoleID"] = new SelectList(_context.WorkRoles, "ID", "Name");

            WorkRole medicWorkRole = _context.WorkRoles.First(wr => wr.Name == "Medic"); // SelectList * top(1) from WorkRoles where Name = "Medic"
            ViewData["MedicWorkRoleID"] = medicWorkRole.ID;

            return View();
        }


        [HttpPost]
        public IActionResult Create(TeamMember newTeamMember)
        {
            if (!newTeamMember.IsSpecialtyValid(_context))
            {
                ViewData["IsSpecialtyValidError"] = "Specialty is not valid!";
            }
            else if (ModelState.IsValid)
            {
                _context.TeamMembers.Add(newTeamMember);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }


            ViewData["WorkRoleID"] = new SelectList(_context.WorkRoles, "ID", "Name");
            return View(newTeamMember);
        }



        public IActionResult Edit(int id)
        {
            return View("Create");
        }
    }
}
