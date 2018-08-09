using GeekBurger.Users.Core.Domains;
using GeekBurger.Users.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GeekBurger.Users.Data
{

    public class UserRepository : IUserRepository
    {
        static List<User> _usuarios;

        public UserRepository()
        {
            if (_usuarios == null)
                _usuarios = new List<User>();
        }
        public string InsertFace(string face)
        {
            int maxRetry = 3;
            int tries = 1;

            ///Goto do terceiro milenio
            string executaInsercao()
            {
                using (var userDb = new UsersContext())
                {
                    if (!userDb.Users.Any(u => u.FaceBase64 == face))
                    {
                        var novo = new User(face);
                        userDb.Users.Add(novo);
                        userDb.SaveChanges();
                        return novo.UserId.ToString();
                    }
                    else
                    {
                        var usuario = userDb.Users.First(u => u.FaceBase64 == face);
                        return usuario.UserId.ToString();
                    }
                }
            }

            try
            {
                if (tries > maxRetry)
                    throw new Exception();

                var result = executaInsercao();
                return result;
            }
            catch(Exception ex)
            {
                if (tries >= maxRetry)
                    throw;

                tries++;
                var result = executaInsercao();
                return result;
            }
        }

        public void InserFoodRestriction(UserRestriction restrictions)
        {

            using (var userDb = new UsersContext())
            {
                if (userDb.Users.Any(u => u.UserId == restrictions.User.UserId))
                {
                    var usuario = userDb.Users.First(user => user.UserId == restrictions.User.UserId);
                    userDb.UserRestrictions.Add(restrictions);
                    userDb.SaveChanges();
                }
                else
                {
                    throw new ArgumentException("usuário não existe");
                }
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            using (var dbContext = new UsersContext())
            {
                var userDb = dbContext.Users.FirstOrDefault(x => x.UserId.ToString().Equals(user.UserId.ToString(), StringComparison.InvariantCultureIgnoreCase));

                if (userDb == null)
                    return false;

                userDb.InProcessing = user.InProcessing;
                userDb.PersistedId = user.PersistedId;
                userDb.GuidReference = user.GuidReference;
                userDb.UserId = user.UserId;

                dbContext.Attach(userDb).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<User> GetUser(Expression<Func<User, bool>> expression)
        {
            using (var dbContext = new UsersContext())
            {
                return await dbContext.Users.FirstOrDefaultAsync(expression);
            }
        }

        public void InsertUser(User newUser)
        {
            using (var dbContext = new UsersContext())
            {
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
            }
        }
    }
}
