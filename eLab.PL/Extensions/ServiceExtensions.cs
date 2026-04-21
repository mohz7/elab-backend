using eLab.BLL.Services.Classes;
using eLab.BLL.Services.Interface;
using eLab.DAL.Repository.Classes;
using eLab.DAL.Repository.Interface;
using eLab.DAL.Utils;
using eLab.PL.Helper;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace eLab.PL.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSetting>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ISeedData, SeedData>();

            // Branch
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IBranchRepository, BranchRepository>();

            // Booking
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingService, BookingService>();

            // BookingItems
            services.AddScoped<IBookingItemRepository, BookingItemRepository>();

            // TestCatalog
            services.AddScoped<ITestCatalogService, TestCatalogService>();
            services.AddScoped<ITestCatalogRepository, TestCatalogRepository>();

            // Offer
            services.AddScoped<IOfferRepository, OfferRepository>();
            services.AddScoped<IOfferService, OfferService>();

            // Price
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddScoped<IPriceService, PriceService>();

            // ReferenceRange
            services.AddScoped<IReferenceRangeRepository, ReferenceRangeRepository>();
            services.AddScoped<IReferenceRangeService, ReferenceRangeService>();

            // ReportTemplate
            services.AddScoped<IReportTemplateRepository, ReportTemplateRepository>();
            services.AddScoped<IReportTemplateService, ReportTemplateService>();

            // StaffProfile
            services.AddScoped<IStaffProfileRepository, StaffProfileRepository>();
            services.AddScoped<IStaffProfileService, StaffProfileService>();

            // PatientProfile
            services.AddScoped<IPatientProfileRepository, PatientProfileRepository>();
            services.AddScoped<IPatientProfileService, PatientProfileService>();

            // Notification
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationService, NotificationService>();

            // PatientRecord
            services.AddScoped<IPatientRecordRepository, PatientRecordRepository>();
            services.AddScoped<IPatientRecordService, PatientRecordService>();

            // Result
            services.AddScoped<IResultRepository, ResultRepository>();
            services.AddScoped<IResultService, ResultService>();

            // StaffChat
            services.AddScoped<IStaffChatRepository, StaffChatRepository>();
            services.AddScoped<IStaffChatService, StaffChatService>();

            // AIChat
            services.AddScoped<IAIChatRepository, AIChatRepository>();
            services.AddScoped<IAIChatService, AIChatService>();
            services.AddHttpClient<IAIService, GeminiAIService>(client =>
            {
                client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}
