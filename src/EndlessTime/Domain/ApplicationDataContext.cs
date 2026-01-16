using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options)
        {
        }

        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<ConsultantRole> ConsultantRoles { get; set; }
        public DbSet<ConsultantAssignment> ConsultantAssignments { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
