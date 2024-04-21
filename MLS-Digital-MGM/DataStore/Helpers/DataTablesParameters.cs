using Microsoft.AspNetCore.Http;

namespace MLS_Digital_MGM.DataStore.Helpers
{
 public class DataTablesParameters
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public string SortColumn { get; set; }
    public string SortColumnAscDesc { get; set; }
    public string SearchValue { get; set; }

    public bool LoadFromRequest(IHttpContextAccessor httpContextAccessor)
    {
        var request = httpContextAccessor.HttpContext.Request;
        if (request.Query.ContainsKey("draw") &&
            request.Query.ContainsKey("start") &&
            request.Query.ContainsKey("length") &&
            request.Query.ContainsKey("columns[" + request.Query["order[0][column]"].FirstOrDefault() + "][name]") &&
            request.Query.ContainsKey("order[0][dir]") &&
            request.Query.ContainsKey("search[value]"))
        {
            var draw = request.Query["draw"].FirstOrDefault();
            var start = request.Query["start"].FirstOrDefault();
            var length = request.Query["length"].FirstOrDefault();

            SortColumn = request.Query["columns[" + request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            SortColumnAscDesc = request.Query["order[0][dir]"].FirstOrDefault();
            SearchValue = request.Query["search[value]"].FirstOrDefault();

            PageSize = length != null ? Convert.ToInt32(length) : 0;
            PageNumber = start != null ? Convert.ToInt32(start) / PageSize + 1 : 1;

            return true;
        }
        return false;
    }
}
}
