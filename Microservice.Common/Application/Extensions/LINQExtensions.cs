namespace Microservice.Common.Application.Extensions;

public static class LINQExtensions
{
    #region ForEachThen

    public static IEnumerable<T> ForEachThen<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        return enumeration.Select(i => { action(i); return i; }).ToList();
    }

    public static Task<IEnumerable<T>> ForEachThenAsync<T>(this IEnumerable<T> enumeration, Func<T, Task> action)
    {
        return Task.FromResult(enumeration.ForEachThen(async t => await action(t)));
    }

    #endregion

    #region Mapping

    public static async Task<IEnumerable<T1>> SelectAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<T1>> func)
    {
        return await Task.WhenAll(enumeration.Select(func));
    }

    public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<T> enumeration, Func<T, Task<IEnumerable<T1>>> func)
    {
        return (await enumeration.SelectAsync(func)).SelectMany(s => s);
    }

    public static IEnumerable<(T1, T2)> With<T1, T2>(this IEnumerable<T1> enumeration, Func<T1, T2> func)
    {
        return enumeration.Select(e => (Entity: e, Mapped: func(e)));
    }

    public static async Task<IEnumerable<T1>> SelectAsync<T, T1>(this IAsyncEnumerable<T> enumeration, Func<T, T1> func)
    {
        List<T1> result = new();
        await foreach (var e in enumeration)
        {
            result.Add(func(e));
        }
        return result;
    }

    #endregion
}
