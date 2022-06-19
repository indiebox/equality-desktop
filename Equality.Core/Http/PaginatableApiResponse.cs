using System;
using System.Threading.Tasks;

using Catel.IoC;

using Equality.Data;

namespace Equality.Http
{
    /// <summary>
    /// This ApiResponse class responsible for paginatable responses.
    /// </summary>
    /// <typeparam name="TPaginatableObject">The paginatable object.</typeparam>
    public class PaginatableApiResponse<TPaginatableObject> : ApiResponseMessage<TPaginatableObject[]>
        where TPaginatableObject : class, new()
    {
        protected IApiClient ApiClient;

        protected ITokenResolver TokenResolver;

        public PaginatableApiResponse(TPaginatableObject[] obj, ApiResponseMessage response) : base(obj, response)
        {
            ApiClient = this.GetDependencyResolver().Resolve<IApiClient>();
            TokenResolver = this.GetDependencyResolver().Resolve<ITokenResolver>();

            InitializeResponse();
        }

        /// <summary>
        /// Does the response has previous page.
        /// </summary>
        public bool HasPrevPage => !string.IsNullOrWhiteSpace(PrevPageLink);

        /// <summary>
        /// Does the response has next page.
        /// </summary>
        public bool HasNextPage => !string.IsNullOrWhiteSpace(NextPageLink);

        /// <summary>
        /// The link to the previous page.
        /// </summary>
        public string PrevPageLink { get; protected set; }

        /// <summary>
        /// The link to the next page.
        /// </summary>
        public string NextPageLink { get; protected set; }

        /// <summary>
        /// The count of items per page.
        /// </summary>
        public int PerPage { get; protected set; }

        /// <summary>
        /// Base url for response(without query parameters).
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// Sends the get next page request to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="IndexOutOfRangeException">Next page doesn't exists.</exception>
        public async Task<PaginatableApiResponse<TPaginatableObject>> NextPageAsync()
        {
            if (!HasNextPage) {
                throw new IndexOutOfRangeException("Next page doesn't exists.");
            }

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(NextPageLink);

            var data = Json.Deserialize<TPaginatableObject[]>(response.Content["data"]);

            return new(data, response);
        }

        /// <summary>
        /// Sends the get previous page request to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="IndexOutOfRangeException">Previous page doesn't exists.</exception>
        public async Task<PaginatableApiResponse<TPaginatableObject>> PrevPageAsync()
        {
            if (!HasPrevPage) {
                throw new IndexOutOfRangeException("Previous page doesn't exists.");
            }

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(PrevPageLink);

            var data = Json.Deserialize<TPaginatableObject[]>(response.Content["data"]);

            return new(data, response);
        }

        private void InitializeResponse()
        {
            NextPageLink = Content["links"]["next"].ToString();
            PrevPageLink = Content["links"]["prev"].ToString();

            Path = Content["meta"]["path"].ToString();
            PerPage = int.Parse(Content["meta"]["per_page"].ToString());
        }
    }
}
