using System;
using System.Collections.Generic;
using System.Text;

using Catel.IoC;

namespace Equality.Http
{
    public class QueryParameters
    {
        /// <summary>
        /// List of fields to include in response.
        /// </summary>
        public Field[] Fields { get; set; } = new Field[] { };

        /// <summary>
        /// Comma-separated list of relations to include in response.
        /// </summary>
        public string[] Includes { get; set; } = new string[] { };

        /// <summary>
        /// List of sorts.
        /// </summary>
        public Sort[] Sorts { get; set; } = new Sort[] { };

        /// <summary>
        /// List of filters.
        /// </summary>
        public Filter[] Filters { get; set; } = new Filter[] { };

        /// <summary>
        /// The pagination data.
        /// </summary>
        public PaginationData PaginationData { get; set; } = new PaginationData();

        /// <summary>
        /// List of additional query parameters.
        /// </summary>
        public Dictionary<string, string> Additional { get; set; }

        /// <summary>
        /// Parse query parameters to the <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The relative path(without domen).</param>
        /// <returns>Returns the <see cref="Uri"/> with query parameters.</returns>
        public virtual Uri Parse(string uri)
        {
            Additional ??= new();

            var result = Additional;
            ParseFields(result);
            ParseIncludes(result);
            ParseSorts(result);
            ParseFilters(result);
            ParsePaginationData(result);

            return this.GetDependencyResolver().Resolve<ApiClient>().BuildUri(uri, result);
        }

        protected void ParseFields(Dictionary<string, string> result)
        {
            foreach (var field in Fields) {
                result.TryAdd($"fields[{field.SourceName}]", string.Join(',', field.Fields));
            }
        }

        protected void ParseIncludes(Dictionary<string, string> result)
        {
            result.TryAdd("include", string.Join(',', Includes));
        }

        protected void ParseSorts(Dictionary<string, string> result)
        {
            var sorts = new StringBuilder();

            foreach (var sort in Sorts) {

                if (sort.Descending) {
                    sorts.Append('-');
                }

                sorts.Append(sort.Name);
                sorts.Append(',');
            }

            result.TryAdd("sort", sorts.ToString().Trim(','));
        }

        protected void ParseFilters(Dictionary<string, string> result)
        {
            foreach (var filter in Filters) {
                result.TryAdd($"filters[{filter.Name}]", string.Join(',', filter.Values));
            }
        }

        protected void ParsePaginationData(Dictionary<string, string> result)
        {
            if (PaginationData.Count > 0) {
                result.TryAdd($"page[count]", PaginationData.Count.ToString());
            }

            if (!string.IsNullOrWhiteSpace(PaginationData.Cursor)) {
                result.TryAdd($"cursor", PaginationData.Cursor);
            }
        }
    }
}
