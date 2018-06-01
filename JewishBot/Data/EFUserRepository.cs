namespace JewishBot.Data
{
    using System.Linq;
    using JewishBot.Models;

    public class EFUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;

        public EFUserRepository(ApplicationDbContext ctx)
        {
            this.context = ctx;
        }

        public IQueryable<User> Users => this.context.Users;

        public void SaveUser(User user)
        {
            this.context.Users.Add(user);
            this.context.SaveChanges();
        }
    }
}
