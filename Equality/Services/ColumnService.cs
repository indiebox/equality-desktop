using System;
using System.Collections.Generic;
using System.Text;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    internal class ColumnService : ColumnServiceBase<Column, Board>, IColumnService
    {
        public ColumnService(IApiClient apiClient, ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {

        }
    }
}
