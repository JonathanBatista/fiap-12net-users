using GeekBurger.Users.Core.Domains;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeekBurger.Users.Data
{
    public interface IUserRepository
    {
        string InsertFace(string face);

        void InserFoodRestriction(UserRestriction restrictions);

        void InsertUser(User newUser);

        Task<bool> UpdateUserAsync(User user);

        Task<User> GetUser(Expression<Func<User, bool>> expression);


    }
}