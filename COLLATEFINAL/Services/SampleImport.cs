using Microsoft.AspNetCore.Identity.UI.Services;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using COLLATE.Helpers.Models;
using CsvHelper;
using CsvHelper.Configuration;

namespace COLLATEFINAL.Services
{
    public class EventCsvMap : ClassMap<EventsModel>
    {
        public EventCsvMap()
        {
            Map(m => m.Category).Name("Category");
            Map(m => m.Title).Name("Title");
            Map(m => m.Objectives).Name("Objectives");
            Map(m => m.PostedDate).Name("PostedDate");
            Map(m => m.ImageUrl).Name("ImageUrl");
            Map(m => m.Content).Name("Content");
            Map(m => m.IsDone).Name("IsDone");
            Map(m => m.IFrame).Name("IFrame");
        }
    }
    public class ResearchPaperCsvMap : ClassMap<ResearchPapersModel>
    {
        public ResearchPaperCsvMap()
        {
            Map(m => m.Header).Name("Header");
            Map(m => m.Title).Name("Title");
            Map(m => m.Authors).Name("Authors");
            Map(m => m.PostedDate).Name("PostedDate");
            Map(m => m.ImageUrl).Name("ImageUrl");
            Map(m => m.YearSec).Name("YearSec");
            Map(m => m.Description).Name("Description");
            Map(m => m.FileUrl).Name("FileUrl");
        }
    }
    public class SoftwareProjectCsvMap : ClassMap<GameAndWebDevModel>
    {
        public SoftwareProjectCsvMap()
        {
            Map(m => m.GroupName).Name("GroupName");
            Map(m => m.Title).Name("Title");
            Map(m => m.DevelopersName).Name("DevelopersName");
            Map(m => m.PostedDate).Name("PostedDate");
            Map(m => m.ImageUrl).Name("ImageUrl");
            Map(m => m.YearSec).Name("YearSec");
            Map(m => m.Description).Name("Description");
            Map(m => m.VidLink).Name("VidLink");
            Map(m => m.GameLink).Name("GameLink");
        }
    }
    public class SubjectCsvMap : ClassMap<SubjectModel>
    {
        public SubjectCsvMap()
        {
            Map(m => m.Subject).Name("Subject");
            Map(m => m.Code).Name("Code");
            Map(m => m.PostedDate).Name("PostedDate");
            Map(m => m.ImageUrl).Name("ImageUrl");
        }
    }
    public class LectureCsvMap : ClassMap<LectureModel>
    {
        public LectureCsvMap()
        {
            Map(m => m.Subject).Name("Subject");
            Map(m => m.Title).Name("Title");
            Map(m => m.PostedDate).Name("PostedDate");
            Map(m => m.FileUrl).Name("FileUrl");
        }
    }
    public class VideoCsvMap : ClassMap<VideosModel>
    {
        public VideoCsvMap()
        {
            Map(m => m.Subject).Name("Subject");
            Map(m => m.Title).Name("Title");
            Map(m => m.IFrame).Name("IFrame");
            Map(m => m.PostedDate).Name("PostedDate");
        }
    }
    public class SampleImportService
    {
        public IEnumerable<TModel> ParseCsvFile<TModel, TMap>(IFormFile file)
        where TMap : ClassMap<TModel>
        {
            if (file == null || !file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                return Enumerable.Empty<TModel>();

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null
            });

            csv.Context.RegisterClassMap<TMap>();
            return csv.GetRecords<TModel>().ToList();
        }


    }
}
