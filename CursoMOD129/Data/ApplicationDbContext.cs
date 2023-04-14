using CursoMOD129.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CursoMOD129.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Represents Table Clients
        public DbSet<Client> Clients { get; set; } = default!;

        public DbSet<WorkRole> WorkRoles { get; set; } = default!;

        public DbSet<TeamMember> TeamMembers { get; set; } = default!;
    }
}