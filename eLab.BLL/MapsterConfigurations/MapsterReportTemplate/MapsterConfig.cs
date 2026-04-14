using eLab.DAL.Dto.Requests;
using eLab.DAL.Dto.Responses;
using eLab.DAL.Models;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eLab.BLL.MapsterConfigurations.MapsterReportTemplate
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister(this IServiceCollection services)
        {
            // ReportTemplate → ReportTemplateRequest
            TypeAdapterConfig<ReportTemplate, ReportTemplateRequest>
                .NewConfig()
                .Map(d => d.Fields,
                     s => string.IsNullOrEmpty(s.FieldsSchema)
                        ? new List<ReportTemplateRequest.FieldDefinition>()
                        : DeserializeFieldsSchema(s.FieldsSchema));

            // ReportTemplateRequest → ReportTemplate
            TypeAdapterConfig<ReportTemplateRequest, ReportTemplate>
                .NewConfig()
                .Map(d => d.FieldsSchema,
                     s => SerializeFieldsSchema(s.Fields));

            TypeAdapterConfig<ReportTemplate, ReportTemplateResponse>
                .NewConfig()
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
