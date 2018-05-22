namespace JewishBot.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using JewishBot.Models;

    public class FakeUserRepository : IUserRepository
    {
        public IQueryable<User> Users => new List<User>
        {
            new User { LunchName = "Andriy Vozniak" },
            new User { LunchName = "Taras Shpachenko" }
        }.AsQueryable();
    }
}
