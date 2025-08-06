using Microsoft.EntityFrameworkCore;
using PromptSubmissionBackend.Models;

namespace PromptSubmissionBackend.Data;


public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<PromptRequest> Prompts => Set<PromptRequest>();
    public DbSet<Admin> Admins => Set<Admin>();
}
