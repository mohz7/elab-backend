using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text.Json;

namespace eLab.BLL.MapsterConfigurations.MapsterBookingItem
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister(this IServiceCollection services)
        {
            TypeAdapterConfig.GlobalSettings.NewConfig<BookingItem, BookingItemResponse>()
                .Map(d => d.TestName, s => s.TestCatalog != null ? s.TestCatalog.Name : null)
                .Map(d => d.Offer, s => s.Offer != null ? s.Offer.Title : null);

            TypeAdapterConfig.GlobalSettings.NewConfig<Booking, BookingResponse>()
                .Map(d => d.Status, s => s.Status.ToString())
                .Map(d => d.PaymentStatus, s => s.PaymentStatus.ToString())
                .Map(d => d.Branch, s => s.Branch != null ? s.Branch.Name : null)
                .Map(d => d.StaffProfile, s => s.StaffProfile != null && s.StaffProfile.User != null
                    ? s.StaffProfile.User.FullName
                    : null)
                .Map(d => d.BookingItems, s => s.BookingItems != null
                    ? s.BookingItems.Adapt<List<BookingItemResponse>>()
                    : new List<BookingItemResponse>());

            TypeAdapterConfig.GlobalSettings.NewConfig<ReportTemplate, ReportTemplateRequest>()
                .Map(d => d.Fields,
                     s => string.IsNullOrEmpty(s.FieldsSchema)
                        ? new List<ReportTemplateRequest.FieldDefinition>()
                        : DeserializeFieldsSchema(s.FieldsSchema));

            TypeAdapterConfig.GlobalSettings.NewConfig<ReportTemplateRequest, ReportTemplate>()
                .Map(d => d.FieldsSchema,
                     s => SerializeFieldsSchema(s.Fields));

            TypeAdapterConfig.GlobalSettings.NewConfig<ReportTemplate, ReportTemplateResponse>()
                .Map(d => d.FieldsSchema,
                     s => string.IsNullOrEmpty(s.FieldsSchema)
                        ? new List<ReportTemplateResponse.FieldDefinition>()
                        : DeserializeForResponse(s.FieldsSchema));
        }

        private static List<ReportTemplateRequest.FieldDefinition> DeserializeFieldsSchema(string json)
            => JsonSerializer.Deserialize<List<ReportTemplateRequest.FieldDefinition>>(json)
               ?? new List<ReportTemplateRequest.FieldDefinition>();

        private static List<ReportTemplateResponse.FieldDefinition> DeserializeForResponse(string json)
            => JsonSerializer.Deserialize<List<ReportTemplateResponse.FieldDefinition>>(json)
               ?? new List<ReportTemplateResponse.FieldDefinition>();

        private static string SerializeFieldsSchema(List<ReportTemplateRequest.FieldDefinition> fields)
            => JsonSerializer.Serialize(fields);
    }
}