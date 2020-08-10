using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace server.Models
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            var connection = Configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(connection);
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<MessageModel> Message { get; set; }
        public DbSet<Conversation> Convo { get; set; }

    }
}