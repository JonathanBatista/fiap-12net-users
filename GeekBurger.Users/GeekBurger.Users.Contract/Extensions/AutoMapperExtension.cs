using AutoMapper;
using GeekBurger.Users.Core.Domains;
using System.Collections.Generic;

namespace GeekBurger.Users.Contract.Extensions
{
    public static class AutoMapperExtension
    {
        public static TDestination MapTo<TDestination>(this DomainEntity entity)
        {
            return Mapper.Map<TDestination>(entity);
        }

        public static List<TDestination> MapToList<TDestination, TSource>(this ICollection<TSource> list)
        {
            return Mapper.Map<List<TDestination>>(list);
        }
    }
}
