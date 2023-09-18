    using COLLATEFINAL.Common;
using COLLATEFINAL.Models;
using COLLATEFINAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;

namespace COLLATEFINAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppIdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PrototypeModel> Prototypes { get; set; }
        public DbSet<GameAndWebDevModel> GameAndWebDevelopments { get; set; }
        public DbSet<ResearchPapersModel> ResearchPapers { get; set; }

        public DbSet<EventsModel> Events { get; set; }
        public DbSet<SubjectModel> Subjects { get; set; }
        public DbSet<LectureModel> Lectures { get; set; }
        public DbSet<VideosModel> Videos { get; set; }
        public DbSet<FeedbackModel> Feedbacks { get; set; }

        public DbSet<Like> PostLikes { get; set; }
        public DbSet<Comment> PostComments { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppIdentityUser>()
                .HasMany(a => a.EventsAttendance)
                .WithMany(e => e.Attendees)
                .UsingEntity(j => j.ToTable("AttendanceEvents"));
        }


        //Instructional Materials
        public async Task<PaginatedResult<PrototypeModel>> GetPaginated(int page, int pageSize, Expression<Func<PrototypeModel, bool>> condition)
        {

            var count = await Prototypes.Where(condition).CountAsync();
            var records = await Prototypes.Where(condition).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<PrototypeModel>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)
            };
            
        }

        public async Task<PaginatedResult<PrototypeModel>> GetPaginated(int page, int pageSize, string keyword)
        {
            return await GetPaginated(page, pageSize, t => t.Title.Contains(keyword ?? string.Empty));
        }

        //Software Projects
        public async Task<SoftwarePaginatedResult<GameAndWebDevModel>> SoftwareGetPaginated(int page, int pageSize, Expression<Func<GameAndWebDevModel, bool>> condition)
        {
            var count = await GameAndWebDevelopments.Where(condition).CountAsync();
            var records = await GameAndWebDevelopments.Where(condition).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new SoftwarePaginatedResult<GameAndWebDevModel>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)
            };

        }

        public async Task<SoftwarePaginatedResult<GameAndWebDevModel>> SoftwareGetPaginated(int page, int pageSize, string keyword)
        {

            return await SoftwareGetPaginated(page, pageSize, t => t.Title.Contains(keyword ?? string.Empty));
        }


        //Research Papers
        public async Task<ResearchPaginatedResult<ResearchPapersModel>> ResearchGetPaginated(int page, int pageSize, Expression<Func<ResearchPapersModel, bool>> condition)
        {
            var count = await ResearchPapers.Where(condition).CountAsync();
            var records = await ResearchPapers.Where(condition).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return new ResearchPaginatedResult<ResearchPapersModel>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)

            };

        }

        public async Task<ResearchPaginatedResult<ResearchPapersModel>> ResearchGetPaginated(int page, int pageSize, string keyword)
        {
            return await ResearchGetPaginated(page, pageSize, t => t.Title.Contains(keyword ?? string.Empty));
        }

        //Events
        public async Task<EventsPaginatedResult<EventsModel>> EventsGetPaginated(int page, int pageSize, Expression<Func<EventsModel, bool>> condition)
        {
            var count = await Events.Where(condition).CountAsync();
            var records = await Events.Where(condition).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new EventsPaginatedResult<EventsModel>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)

            };

        }

        public async Task<EventsPaginatedResult<EventsModel>> EventsGetPaginated(int page, int pageSize, string keyword)
        {
            return await EventsGetPaginated(page, pageSize, t => t.Title.Contains(keyword ?? string.Empty));
        }

        //Lectures
        public async Task<LecturesPaginatedResult<LectureModel>> LecturesGetPaginated(int page, int pageSize, Expression<Func<LectureModel, bool>> condition)
        {
            var count = await Lectures.Where(condition).CountAsync();
            var records = await Lectures.Where(condition).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new LecturesPaginatedResult<LectureModel>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)

            };

        }

        public async Task<LecturesPaginatedResult<LectureModel>> LecturesGetPaginated(int page, int pageSize, string keyword)
        {
            return await LecturesGetPaginated(page, pageSize, t => t.Title.Contains(keyword ?? string.Empty));
        }

        //Subjects
        public async Task<SubjectsPaginatedResult<SubjectModel>> SubjectsGetPaginated(int page, int pageSize, Expression<Func<SubjectModel, bool>> condition)
        {
            var count = await Subjects.Where(condition).CountAsync();
            var records = await Subjects.Where(condition).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new SubjectsPaginatedResult<SubjectModel>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)

            };

        }

        public async Task<SubjectsPaginatedResult<SubjectModel>> SubjectsGetPaginated(int page, int pageSize, string keyword)
        {
            return await SubjectsGetPaginated(page, pageSize, t => t.Subject.Contains(keyword ?? string.Empty));
        }

        //Videos
        public async Task<VideosPaginatedResult<VideosModel>> VideosGetPaginated(int page, int pageSize, Expression<Func<VideosModel, bool>> condition)
        {
            var count = await Videos.Where(condition).CountAsync();
            var records = await Videos.Where(condition).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new VideosPaginatedResult<VideosModel>
            {
                Result = records,
                Page = page,
                TotalCount = (int)Math.Ceiling(count / (double)pageSize)

            };

        }

        public async Task<VideosPaginatedResult<VideosModel>> VideosGetPaginated(int page, int pageSize, string keyword)
        {
            return await VideosGetPaginated(page, pageSize, t => t.Title.Contains(keyword ?? string.Empty));
        }


    }
}