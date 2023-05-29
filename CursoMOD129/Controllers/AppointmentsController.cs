using Azure.Core;
using CursoMOD129.Data;
using CursoMOD129.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;
using static CursoMOD129.CursoMOD129Constants;

namespace CursoMOD129.Controllers
{
    [Authorize(Policy = POLICIES.APP_POLICY_ADMIN.NAME)]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;

        public AppointmentsController(ApplicationDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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


        // GET: Appointments/TodaysAppointments
        public IActionResult TodaysAppointments()
        {
            var todaysAppointments = _context.Appointments
                 .Include(ap => ap.Client)
                 .Include(ap => ap.Medic)
                .Where(ap => ap.Date.Date == DateTime.Now.Date)
                .ToList();

            return View(todaysAppointments);
        }

        // GET: Appointments/TomorrowsAppointments
        public IActionResult TomorrowsAppointments()
        {
            var tomorrowsAppointments = _context.Appointments
                    .Include(ap => ap.Client)
                    .Include(ap => ap.Medic)
                    .Where(ap => ap.Date.Date == DateTime.Today.AddDays(1))
                    .ToList();

            return View(tomorrowsAppointments);
        }

        // GET: Appointments/NextWeeksAppointments
        public IActionResult NextWeeksAppointments()
        {
            var nextweeksAppointments = _context.Appointments
                    .Include(ap => ap.Client)
                    .Include(ap => ap.Medic)
                    .Where(ap => ap.Date.Date >= DateTime.Today.AddDays(1) && ap.Date.Date < DateTime.Today.AddDays(8))
                    .ToList();

            return View(nextweeksAppointments);
        }


        // GET: Appointments/SendEmail
        // id is AppointmentID
        public IActionResult SendEmail(int id)
        {
            Appointment? appointment = _context
                                            .Appointments
                                            .Include(ap => ap.Client)
                                            .Include(ap => ap.Medic)
                                            .First(ap => ap.ID == id);
            
            if (appointment == null)
            {
                return NotFound();
            }


            string clientEmail = appointment.Client.Email;
            string subject = "Do not forget you Today's Appointment";
            string htmlMessage = $"Today, you have an appointment with" +
                          $" Medic: {appointment.Medic.Name} at: {appointment.Time:hh:mm}";

            _emailSender.SendEmailAsync(clientEmail, subject, htmlMessage);

            return RedirectToAction(nameof(TodaysAppointments));
        }

        // GET: Appointments/SendTomorrowsEmails
        public IActionResult SendTomorrowsEmails()
        {
            ICollection<Appointment> tomorrowsAppointments = _context
                                            .Appointments
                                            .Include(ap => ap.Client)
                                            .Include(ap => ap.Medic)
                                            .Where(ap => ap.Date.Date == DateTime.Today.AddDays(1))
                                            .ToList();

            if (tomorrowsAppointments.Count == 0)
            {
                return NotFound();
            }

            string subject = "Do not forget you Tomorrow's Appointment";
            string path = Directory.GetCurrentDirectory();
            string template = System.IO.File.ReadAllText(
                Path.Combine(path, EMAIL.EMAIL_TEMPLATES_FOLDER, EMAIL.TOMORROW_APPOINTMENT_EMAIL_TEMPLATE));

            foreach (Appointment ap in tomorrowsAppointments)
            {
                string clientEmail = ap.Client.Email;
                
                StringBuilder body = new StringBuilder(template);
                body.Replace("#CLIENT_NAME", ap.Client.Name);
                body.Replace("#MEDIC_NAME", ap.Medic.Name);
                body.Replace("#APPOINTMENT_TIME", ap.Time.ToShortTimeString());

                _emailSender.SendEmailAsync(clientEmail, subject, body.ToString());
            }

            return RedirectToAction(nameof(TodaysAppointments));
        }


        // GET: Appointments/SendTomorrowsEmails
        public IActionResult SendNextWeeksEmails()
        {
            ICollection<Appointment> nextWeeksAppointments = _context
                            .Appointments
                            .Include(ap => ap.Client)
                            .Include(ap => ap.Medic)
                            .Where(ap => ap.Date.Date >= DateTime.Today.AddDays(1) && ap.Date.Date < DateTime.Today.AddDays(8))
                            .ToList();

            if (nextWeeksAppointments.Count == 0)
            {
                return NotFound();
            }

            string subject = "Do not forget you Next Week's Appointment";
            string path = Directory.GetCurrentDirectory();
            string template = System.IO.File.ReadAllText(
                Path.Combine(path, EMAIL.EMAIL_TEMPLATES_FOLDER, EMAIL.TOMORROW_APPOINTMENT_EMAIL_TEMPLATE));

            foreach (Appointment ap in nextWeeksAppointments)
            {
                string clientEmail = ap.Client.Email;

                StringBuilder body = new StringBuilder(template);
                body.Replace("#CLIENT_NAME", ap.Client.Name);
                body.Replace("#MEDIC_NAME", ap.Medic.Name);
                body.Replace("#APPOINTMENT_TIME", ap.Time.ToShortTimeString());

                _emailSender.SendEmailAsync(clientEmail, subject, body.ToString());
            }

            return RedirectToAction(nameof(Index));
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
