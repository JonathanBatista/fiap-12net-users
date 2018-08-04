using GeekBurger.Users.Core.Domains;
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
                if (!_usuarios.Exists(u => u.FaceBase64 == face))
                {
                    var novo = new User(face);
                    _usuarios.Add(novo);
                    return novo.AzureGuid;
                }
                else
                {
                    var usuario = _usuarios.First(u => u.FaceBase64 == face);
                    return usuario.AzureGuid;
                }
            }

            try
            {
                if (tries < 2)
                    throw new Exception();

                var result = executaInsercao();
                return result;
            }
            catch
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
            if (_usuarios.Exists(user => user.AzureGuid == restrictions.User.AzureGuid))
            {
                var usuario = _usuarios.First(user => user.AzureGuid == restrictions.User.AzureGuid);
                usuario.Restrictions.Add(restrictions);
            }
            else
            {
                throw new ArgumentException("usuário não existe");
            }
        }
    }
}
