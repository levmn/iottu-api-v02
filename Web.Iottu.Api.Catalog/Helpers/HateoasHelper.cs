namespace Web.Iottu.Api.Catalog.Helpers
{
    public static class HateoasHelper
    {
        public static object CreateLink(string routeName, object routeValues, string rel, string method)
        {
            return new
            {
                href = $"/{routeName}",
                rel,
                method
            };
        }

        public static object AddLinks<T>(T entity, Dictionary<string, string> links)
        {
            var props = entity!.GetType()
                .GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(entity));

            props["_links"] = links
                .Select(l => new { href = l.Value, rel = l.Key, method = GetHttpMethod(l.Key) })
                .ToArray();

            return props;
        }

        public static IEnumerable<object> CollectionLinks(string basePath, int page, int pageSize, int totalPages)
        {
            var links = new List<object>
            {
                new { href = $"{basePath}?page={page}&pageSize={pageSize}", rel = "self", method = "GET" }
            };

            if (page > 1)
            {
                links.Add(new { href = $"{basePath}?page={page - 1}&pageSize={pageSize}", rel = "prev", method = "GET" });
            }

            if (page < totalPages)
            {
                links.Add(new { href = $"{basePath}?page={page + 1}&pageSize={pageSize}", rel = "next", method = "GET" });
            }

            return links;
        }

        private static string GetHttpMethod(string rel)
        {
            return rel.ToLower() switch
            {
                "self" => "GET",
                "update" => "PUT",
                "delete" => "DELETE",
                _ => "GET"
            };
        }
    }
}
