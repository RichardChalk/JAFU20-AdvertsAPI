namespace AdvertsAPI.Data
{
    public class DataInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        public DataInitializer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void SeedData()
        {
            _dbContext.Database.Migrate();
        }

    }
}
