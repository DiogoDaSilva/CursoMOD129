using CursoMOD129.Data;
using CursoMOD129.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace CursoMOD129.Controllers
{
    public class TeamMembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        private readonly IHtmlLocalizer<Resource> _sharedLocalizer;

        public TeamMembersController(ApplicationDbContext context, IToastNotification toastNotification,
            IHtmlLocalizer<Resource> sharedLocalizer)
        {
            _context = context;
            _toastNotification = toastNotification;
            _sharedLocalizer = sharedLocalizer;
        }


        public IActionResult Index()
        {
            var teamMembers = _context
                .TeamMembers
                .Include(tm => tm.WorkRole)
                .ToList();

            SetupMedicWorkRoleID();

            return View(teamMembers);
        }

        // Get: TeamMembers/Create
        public IActionResult Create()
        {
            SetupTeamMember();

            return View();
        }

        

        [HttpPost]
        public IActionResult Create(TeamMember newTeamMember)
        {
            if (!newTeamMember.IsSpecialtyValid(_context))
            {
                ViewData["IsSpecialtyValidError"] = "Specialty is not valid!";
                _toastNotification.AddErrorToastMessage(_sharedLocalizer["Specialty is not valid!"].Value, 
                    new ToastrOptions { Title = _sharedLocalizer["Team Member Creation Error"].Value });
            }
            else if (ModelState.IsValid)
            {
                _context.TeamMembers.Add(newTeamMember);
                _context.SaveChanges();

                _toastNotification.AddSuccessToastMessage(_sharedLocalizer["Team Member successfully created."].Value,
                    new ToastrOptions { Title = _sharedLocalizer["Team Member Created Successfully"].Value });

                return RedirectToAction(nameof(Index));
            }

            _toastNotification.AddErrorToastMessage(_sharedLocalizer["Please check the form again!"].Value, 
                new ToastrOptions { Title = _sharedLocalizer["Team Member Creation Error"].Value });

            ViewData["WorkRoleID"] = new SelectList(_context.WorkRoles, "ID", "Name");
            return View(newTeamMember);
        }


        // Get: TeamMembers/Edit
        public IActionResult Edit(int id)
        {
            TeamMember? teamMember = _context.TeamMembers.Find(id);

            if (teamMember == null)
            {
                return NotFound();
            }

            SetupTeamMember();

            return View(teamMember);
        }

        [HttpPost]
        public IActionResult Edit(TeamMember editingTeamMember)
        {
            if (!editingTeamMember.IsSpecialtyValid(_context))
            {
                ViewData["IsSpecialtyValidError"] = "Specialty is not valid!";
            }
            else if (ModelState.IsValid) 
            {
                _context.TeamMembers.Update(editingTeamMember);
                _context.SaveChanges();
                
                return RedirectToAction(nameof(Index));
            }

            SetupTeamMember();

            return View(editingTeamMember);
        }

        // Get: TeamMembers/Delete/id
        public IActionResult Delete(int id)
        {
            TeamMember? teamMember = _context.TeamMembers.Find(id);

            if (teamMember == null)
            {
                return NotFound();
            }

            SetupTeamMember();

            return View(teamMember);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            TeamMember? deletingTeamMember = _context.TeamMembers.Find(id);

            if (deletingTeamMember != null)
            {
                _context.TeamMembers.Remove(deletingTeamMember);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }



        private void SetupTeamMember()
        {
            ViewData["WorkRoleID"] = new SelectList(_context.WorkRoles, "ID", "Name");
            SetupMedicWorkRoleID();
        }

        private void SetupMedicWorkRoleID()
        {
            WorkRole medicWorkRole = _context.WorkRoles.First(wr => wr.Name == "Medic"); // SelectList * top(1) from WorkRoles where Name = "Medic"
            ViewData["MedicWorkRoleID"] = medicWorkRole.ID;
        }
    }
}
