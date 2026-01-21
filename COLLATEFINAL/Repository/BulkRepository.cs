using COLLATEFINAL.Data;
using COLLATEFINAL.Models;

namespace COLLATEFINAL.Repository
{
    public class BulkRepository
    {
        private readonly ApplicationDbContext _context;

        public BulkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void BulkInsertEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            if (entities == null || !entities.Any())
                return;

            _context.Set<TEntity>().AddRange(entities);
            _context.SaveChanges();
        }



    }
}
