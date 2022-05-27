namespace Equality.Http
{
    public class PaginationData
    {
        public PaginationData()
        {
        }

        /// <summary>
        /// The count of items per page.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The cursor for cursor pagination.
        /// </summary>
        public string Cursor { get; set; }
    }
}
