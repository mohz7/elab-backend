using eLab.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Midicare_eLab.DAL.Data;
using Midicare_eLab.DAL.Models;

namespace eLab.DAL.Utils
{
    public class SeedData : ISeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public SeedData(ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task IdentityDataSeedingAsync()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                await _context.Database.MigrateAsync();

            // Roles
            if (!await _roleManager.Roles.AnyAsync())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Staff"));
                await _roleManager.CreateAsync(new IdentityRole("Patient"));
            }

            // Users
            if (!await _userManager.Users.AnyAsync())
            {
                // Admin
                var admin = new User()
                {
                    FullName = "Admin eLab",
                    Email = "admin@elab.com",
                    UserName = "admin_elab",
                    PhoneNumber = "0599000001",
                    EmailConfirmed = true,
                    IsActive = true,
                    IdentityNumber = "000000001",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1990, 1, 1),
                };
                // Staff
                var staff = new User()
                {
                    FullName = "Ahmed Staff",
                    Email = "staff@elab.com",
                    UserName = "staff_elab",
                    PhoneNumber = "0599000002",
                    EmailConfirmed = true,
                    IsActive = true,
                    IdentityNumber = "000000002",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1992, 5, 15),
                };
                // Patient
                var patient = new User()
                {
                    FullName = "Omar Patient",
                    Email = "patient@elab.com",
                    UserName = "patient_elab",
                    PhoneNumber = "0599000003",
                    EmailConfirmed = true,
                    IsActive = true,
                    IdentityNumber = "000000003",
                    Gender = Gender.Male,
                    DateOfBirth = new DateOnly(1995, 8, 20),
                };

                await _userManager.CreateAsync(admin, "Pass@1212");
                await _userManager.CreateAsync(staff, "Pass@1212");
                await _userManager.CreateAsync(patient, "Pass@1212");

                await _userManager.AddToRoleAsync(admin, "Admin");
                await _userManager.AddToRoleAsync(staff, "Staff");
                await _userManager.AddToRoleAsync(patient, "Patient");

                
                var adminId = admin.Id;

                // Branch
                if (!await _context.Branches.AnyAsync())
                {
                    await _context.Branches.AddRangeAsync(
                        new Branch
                        {
                            Name = "Branch Ramallah",
                            Address = "Ramallah - Main Street",
                            Phone = "02-2960001",
                            Email = "ramallah@elab.com",
                            City = "Ramallah",
                            IsActive = true,
                            CreatedById = adminId
                        },
                        new Branch
                        {
                            Name = "Branch Nablus",
                            Address = "Nablus - City Center",
                            Phone = "09-2330001",
                            Email = "nablus@elab.com",
                            City = "Nablus",
                            IsActive = true,
                            CreatedById = adminId
                        }
                    );
                }

                // TestCatalog
                if (!await _context.TestCatalogs.AnyAsync())
                {
                    await _context.TestCatalogs.AddRangeAsync(
                        new TestCatalog
                        {
                            Name = "Complete Blood Count (CBC)",
                            Description = "Measures different components of blood",
                            Category = "Hematology",
                            SampleType = "Blood",
                            TurnaroundHours = 24,
                            IsActive = true,
                            CreatedById = adminId
                        },
                        new TestCatalog
                        {
                            Name = "Blood Sugar (Glucose)",
                            Description = "Measures blood sugar level",
                            Category = "Biochemistry",
                            SampleType = "Blood",
                            TurnaroundHours = 12,
                            IsActive = true,
                            CreatedById = adminId
                        },
                        new TestCatalog
                        {
                            Name = "Urine Analysis",
                            Description = "General urine examination",
                            Category = "Urinalysis",
                            SampleType = "Urine",
                            TurnaroundHours = 6,
                            IsActive = true,
                            CreatedById = adminId
                        }
                    );
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}