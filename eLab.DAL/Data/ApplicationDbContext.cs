using eLab.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Midicare_eLab.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Midicare_eLab.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Branch> Branches { get; set; }
        public DbSet<StaffProfile> StaffProfiles { get; set; }
        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<TestCatalog> TestCatalogs { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingItem> BookingItems { get; set; }
        public DbSet<ReportTemplate> ReportTemplates { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public DbSet<ReferenceRange> ReferenceRanges { get; set; }
        public DbSet<StaffChat> StaffChats { get; set; }
        public DbSet<StaffChatMessage> StaffChatMessages { get; set; }
        public DbSet<AIChat> AIChats { get; set; }
        public DbSet<AIChatMessage> AIChatMessages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");

            // ignore
            builder.Ignore<IdentityUserClaim<string>>();
            builder.Ignore<IdentityUserLogin<string>>();
            builder.Ignore<IdentityUserToken<string>>();
            builder.Ignore<IdentityRoleClaim<string>>();

            // ── StaffProfile ───────────────────────────────────────────────────────
            builder.Entity<StaffProfile>(e =>
            {
                e.HasOne(sp => sp.User)
                 .WithOne(u => u.StaffProfile)
                 .HasForeignKey<StaffProfile>(sp => sp.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(sp => sp.Branch)
                 .WithMany(b => b.StaffProfiles)
                 .HasForeignKey(sp => sp.BranchId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(sp => sp.CreatedBy)
                 .WithMany()
                 .HasForeignKey(sp => sp.CreatedById)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── PatientProfile ─────────────────────────────────────────────────────
            builder.Entity<PatientProfile>(e =>
            {
                e.HasOne(pp => pp.User)
                 .WithOne(u => u.PatientProfile)
                 .HasForeignKey<PatientProfile>(pp => pp.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(pp => pp.CreatedBy)
                 .WithMany(u => u.CreatedPatientProfiles)
                 .HasForeignKey(pp => pp.CreatedById)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Price ──────────────────────────────────────────────────────────────
            builder.Entity<Price>(e =>
            {
                e.HasOne(p => p.TestCatalog)
                 .WithMany(t => t.Prices)
                 .HasForeignKey(p => p.TestCatalogId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(p => p.Branch)
                 .WithMany(b => b.Prices)
                 .HasForeignKey(p => p.BranchId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(p => p.CreatedBy)
                 .WithMany()
                 .HasForeignKey(p => p.CreatedById)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Offer ──────────────────────────────────────────────────────────────
            builder.Entity<Offer>(e =>
            {
                e.HasOne(o => o.TestCatalog)
                 .WithMany(t => t.Offers)
                 .HasForeignKey(o => o.TestCatalogId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(o => o.Branch)
                 .WithMany(b => b.Offers)
                 .HasForeignKey(o => o.BranchId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Booking ────────────────────────────────────────────────────────────
            builder.Entity<Booking>(e =>
            {
                e.HasOne(b => b.PatientProfile)
                 .WithMany(pp => pp.Bookings)
                 .HasForeignKey(b => b.PatientProfileId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(b => b.Branch)
                 .WithMany(br => br.Bookings)
                 .HasForeignKey(b => b.BranchId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(b => b.StaffProfile)
                 .WithMany(sp => sp.Bookings)
                 .HasForeignKey(b => b.StaffProfileId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── BookingItem ────────────────────────────────────────────────────────
            builder.Entity<BookingItem>(e =>
            {
                e.HasOne(bi => bi.Booking)
                 .WithMany(b => b.BookingItems)
                 .HasForeignKey(bi => bi.BookingId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(bi => bi.TestCatalog)
                 .WithMany(t => t.BookingItems)
                 .HasForeignKey(bi => bi.TestCatalogId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(bi => bi.Offer)
                 .WithMany(o => o.BookingItems)
                 .HasForeignKey(bi => bi.OfferId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── ReportTemplate ─────────────────────────────────────────────────────
            builder.Entity<ReportTemplate>(e =>
            {
                e.HasOne(rt => rt.TestCatalog)
                 .WithMany(t => t.ReportTemplates)
                 .HasForeignKey(rt => rt.TestCatalogId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Result ─────────────────────────────────────────────────────────────
            builder.Entity<Result>(e =>
            {
                e.ToTable("Results");

                // stores "Normal" / "High" / "Low" as string in DB
                e.Property(r => r.ResultFlags)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                // stores "Pending" / "Approved" / "Rejected" as string in DB
                e.Property(r => r.Status)
                    .HasConversion<string>()
                    .HasMaxLength(50);

                e.HasOne(res => res.BookingItem)
                 .WithOne(bi => bi.Results)
                 .HasForeignKey<Result>(res => res.BookingItemId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(res => res.PatientProfile)
                 .WithMany(pp => pp.Results)
                 .HasForeignKey(res => res.PatientProfileId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(res => res.ReportTemplate)
                 .WithMany(rt => rt.Results)
                 .HasForeignKey(res => res.ReportTemplateId)
                 .OnDelete(DeleteBehavior.SetNull);

                e.HasOne(res => res.UploadedBy)
                 .WithMany()
                 .HasForeignKey(res => res.UploadedById)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(res => res.ApprovedBy)
                 .WithMany()
                 .HasForeignKey(res => res.ApprovedById)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── PatientRecord ──────────────────────────────────────────────────────
            builder.Entity<PatientRecord>(e =>
            {
                e.HasOne(pr => pr.PatientProfile)
                 .WithMany(pp => pp.PatientRecords)
                 .HasForeignKey(pr => pr.PatientProfileId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(pr => pr.Result)
                 .WithMany(res => res.PatientRecords)
                 .HasForeignKey(pr => pr.ResultId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(pr => pr.Branch)
                 .WithMany(b => b.PatientRecords)
                 .HasForeignKey(pr => pr.BranchId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(pr => pr.Booking)
                 .WithMany(b => b.PatientRecords)
                 .HasForeignKey(pr => pr.BookingId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── ReferenceRange ─────────────────────────────────────────────────────
            builder.Entity<ReferenceRange>(e =>
            {
                e.HasOne(rr => rr.ReportTemplate)
                 .WithMany(rt => rt.ReferenceRanges)
                 .HasForeignKey(rr => rr.ReportTemplateId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── StaffChat ──────────────────────────────────────────────────────────
            builder.Entity<StaffChat>(e =>
            {
                e.HasOne(sc => sc.Booking)
                 .WithMany(b => b.StaffChats)
                 .HasForeignKey(sc => sc.BookingId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(sc => sc.PatientProfile)
                 .WithMany(pp => pp.StaffChats)
                 .HasForeignKey(sc => sc.PatientProfileId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(sc => sc.StaffProfile)
                 .WithMany(sp => sp.StaffChats)
                 .HasForeignKey(sc => sc.StaffProfileId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── StaffChatMessage ───────────────────────────────────────────────────
            builder.Entity<StaffChatMessage>(e =>
            {
                e.HasOne(m => m.Chat)
                 .WithMany(sc => sc.StaffChatMessages)
                 .HasForeignKey(m => m.ChatId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(m => m.Sender)
                 .WithMany(u => u.StaffChatMessages)
                 .HasForeignKey(m => m.SenderId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── AIChat ─────────────────────────────────────────────────────────────
            builder.Entity<AIChat>(e =>
            {
                e.HasOne(ac => ac.PatientProfile)
                 .WithMany(pp => pp.AIChats)
                 .HasForeignKey(ac => ac.PatientProfileId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ac => ac.Result)
                 .WithMany(res => res.AIChats)
                 .HasForeignKey(ac => ac.ResultId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── AIChatMessage ──────────────────────────────────────────────────────
            builder.Entity<AIChatMessage>(e =>
            {
                e.HasOne(m => m.AIChat)
                 .WithMany(ac => ac.AIChatMessages)
                 .HasForeignKey(m => m.AIChatId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(m => m.Sender)
                 .WithMany(u => u.AIChatMessages)
                 .HasForeignKey(m => m.SenderId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Notification ───────────────────────────────────────────────────────
            builder.Entity<Notification>(e =>
            {
                e.HasOne(n => n.User)
                 .WithMany(u => u.Notifications)
                 .HasForeignKey(n => n.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(n => n.Result)
                 .WithMany(res => res.Notifications)
                 .HasForeignKey(n => n.ResultId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── User ───────────────────────────────────────────────────────────────
            builder.Entity<User>()
                .HasIndex(u => u.IdentityNumber)
                .IsUnique();

            // ── Cart ───────────────────────────────────────────────────────────────
            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}