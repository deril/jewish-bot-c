namespace JewishBot.Data
{
    using System.Linq;
    using JewishBot.Models;

    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
    }
}
