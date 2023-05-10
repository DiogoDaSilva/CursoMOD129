using CursoMOD129.Data;
using CursoMOD129.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CursoMOD129.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var appointments = _context
                                    .Appointments
                                    .Include(ap => ap.Medic)
                                    .Include(ap => ap.Client)
                                    .ToList();

            return View(appointments);
        }

        // GET Appointments/Create
        public IActionResult Create() 
        {
            this.SetupAppointments();

            return View();
        }

        // POST Appointments/Create
        [HttpPost]
        public IActionResult Create(Appointment newAppointment)
        {
            if (ModelState.IsValid)
            {
                _context.Appointments.Add(newAppointment);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            this.SetupAppointments();

            return View(newAppointment);
        }


        // GET Appointments/Edit/id
        public IActionResult Edit(int id)
        {
            Appointment? editingAppointment = _context.Appointments.Find(id);

            if (editingAppointment == null) 
            {
                // return NotFound();
                // TODO: Apresentar uma mensagem de erro com um texto explicativo
                return RedirectToAction(nameof(Index));
            }

            this.SetupAppointments();

            return View(editingAppointment);

        }

        // POST Appointment/Edit/id
        [HttpPost]
        public IActionResult Edit(int id, Appointment editingAppointment)
        {
            if (!ModelState.IsValid)
            {
                this.SetupAppointments();
                return View(editingAppointment);
            }
            else
            {
                Appointment? dbAppointment = _context.Appointments
                    .AsNoTracking()
                    .First(ap => ap.ID == id);
                    

                if (dbAppointment == null)
                {
                    // return NotFound();
                    // TODO: Apresentar uma mensagem de erro com um texto explicativo
                    return RedirectToAction(nameof(Index));
                }

                _context.Appointments.Update(editingAppointment);
                _context.SaveChanges();
                // TODO: Apresentar uma mensagem de sucesso
                return RedirectToAction(nameof(Index));
            }
        }



        // GET: Appointments/Delete/id
        public IActionResult Delete(int id)
        {
            Appointment? appointment = _context.Appointments.Find(id);

            if (appointment == null)
            {
                return NotFound();
            }

            SetupAppointments();

            return View(appointment);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            Appointment? deletingAppointment = _context.Appointments.Find(id);

            if (deletingAppointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(deletingAppointment);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        // GET: Appointments/MedicAppointmentsHistory/medicID
        public IActionResult MedicAppointmentsHistory(int id)
        {
            TeamMember? medic = _context.TeamMembers.Find(id);
            if (medic == null)
            {
                return NotFound();
            }

            ViewData["MedicName"] = medic.Name;

            var appointmentsHistory = _context.Appointments
                .Include(ap => ap.Client)
                .Where(ap => ap.MedicID == id)
                .Where(ap => ap.Date < DateTime.Now)
                .ToList();

            return View(appointmentsHistory);
        }

        // GET: Appointments/MedicAppointmentsScheduled/medicID
        public IActionResult MedicAppointmentsScheduled(int id)
        {
            TeamMember? medic = _context.TeamMembers.Find(id);
            if (medic == null)
            {
                return NotFound();
            }

            ViewData["MedicName"] = medic.Name;

            var appointmentsScheduled = _context.Appointments
                .Include(ap => ap.Client)
                .Where(ap => ap.MedicID == id)
                .Where(ap => ap.Date >= DateTime.Now)
                .ToList();

            return View("MedicAppointmentsHistory", appointmentsScheduled);
        }



        private void SetupAppointments()
        {
            ViewData["ClientsList"] = new SelectList(_context.Clients, "ID", "Name");

            WorkRole medic = _context.WorkRoles.First(wr => wr.Name == "Medic");

            var dbMedicList = _context.TeamMembers.Where(tm => tm.WorkRoleID == medic.ID);

            ViewData["MedicsList"] = new SelectList(dbMedicList, "ID", "Name");
        }


    }
}
