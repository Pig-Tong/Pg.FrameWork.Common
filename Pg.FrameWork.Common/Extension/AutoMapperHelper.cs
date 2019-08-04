using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;

namespace Pg.FrameWork.Common.Extension
{
    /// <summary>
    /// 自动映射器
    /// </summary>
    public static class AutoMapperHelper
    {
        public static T MapTo<T>(this object obj)
        {
            if (obj == null)
            {
                return default(T);
            }
            Mapper.Initialize(x => x.CreateMap(obj.GetType(), typeof(T)));
            return Mapper.Map<T>(obj);
        }

        public static List<TDestination> MapToList<TDestination>(this IEnumerable source)
        {
            if (source == null)
            {
                return null;
            }
            IEnumerator enumerator = source.GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    if (current != null)
                    {
                        Type type = current.GetType();
                        Mapper.Initialize(x => x.CreateMap(type, typeof(TDestination)));
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            return Mapper.Map<List<TDestination>>(source);
        }

        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            Mapper.Initialize(x => x.CreateMap<TSource, TDestination>());
            return Mapper.Map<List<TDestination>>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
        {
            if (source == null)
            {
                return destination;
            }
            Mapper.Initialize(x => x.CreateMap<TSource, TDestination>());
            return Mapper.Map(source, destination);
        }
    }
}
