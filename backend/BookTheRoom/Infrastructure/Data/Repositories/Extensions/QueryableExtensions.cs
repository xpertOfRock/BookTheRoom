
namespace Infrastructure.Data.Repositories.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate,
            bool condition)
        {
            return condition ? queryable.Where(predicate) : queryable;
        }
    }
}


//.WhereIf(h => 
//    h.Name.ToLower().Contains(request.Search.ToLower()) ||
//    h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
//    h.Address.Country.ToLower().Contains(request.Search.ToLower()) ||
//    h.Address.Country.ToLower().Contains(request.Search.ToLower())
//    , string.IsNullOrWhiteSpace(request.Search))