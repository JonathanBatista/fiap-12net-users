using GeekBurger.Users.Core.Domains;
using GeekBurger.Users.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekBurger.Users.Data
{

    public class UserRepository
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

        public void UpdateUser(User user)
        {

        }
    }
}
