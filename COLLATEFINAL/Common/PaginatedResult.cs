using COLLATEFINAL.Models;

namespace COLLATEFINAL.Common
{
    public class PaginatedResult<PrototypeModel>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public string? SearchKeyword { get; set; }
        public IEnumerable<PrototypeModel>? Result { get; set; }
    }

    public class SoftwarePaginatedResult<GameAndWebDevModel>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public string? SearchKeyword { get; set; }
        public IEnumerable<GameAndWebDevModel>? Result { get; set; }
    }

    public class ResearchPaginatedResult<ResearchPapersModel>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public string? SearchKeyword { get; set; }
        public IEnumerable<ResearchPapersModel>? Result { get; set; }
    }

    public class EventsPaginatedResult<EventsModel>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public string? SearchKeyword { get; set; }
        public IEnumerable<EventsModel>? Result { get; set; }
    }

    public class SubjectsPaginatedResult<SubjectModel>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public string? SearchKeyword { get; set; }
        public IEnumerable<SubjectModel>? Result { get; set; }
    }

    public class LecturesPaginatedResult<LectureModel>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public string? SearchKeyword { get; set; }
        public IEnumerable<LectureModel>? Result { get; set; }
    }

    public class VideosPaginatedResult<VideosModel>
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public string? SearchKeyword { get; set; }
        public IEnumerable<VideosModel>? Result { get; set; }
    }
}
