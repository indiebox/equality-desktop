﻿using System;
using System.Collections.Generic;
using System.Text;

using Catel.IoC;

namespace Equality.Http
{
    public class QueryParameters
    {
        public Field[] Fields { get; set; } = new Field[] { };

        public string[] Includes { get; set; } = new string[] { };

        public Sort[] Sorts { get; set; } = new Sort[] { };

        public Filter[] Filters { get; set; } = new Filter[] { };

        public Dictionary<string, string> Additional { get; set; }

        public virtual Uri Parse(string uri)
        {
            Additional ??= new();

            var result = Additional;
            ParseFields(result);
            ParseIncludes(result);
            ParseSorts(result);
            ParseFilters(result);

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
    }
}
