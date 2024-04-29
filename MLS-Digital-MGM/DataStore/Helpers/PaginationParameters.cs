using System.Linq.Expressions;
namespace DataStore.Helpers;
public class PagingParameters<T>
{

    public Expression<Func<T, bool>> Predicate { get; set;} = c => true;
    public string SearchTerm { get; set;}
    public string SortColumn { get; set;}
    public string SortDirection { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set;}
    public Expression<Func<T, object>>[] Includes { get; set; } = new Expression<Func<T, object>>[] {};
    public string? CreatedById { get; set; } = null;

}