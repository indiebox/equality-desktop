namespace Equality.Http
{
    public class Field
    {
        public Field(string sourceName, string field) : this(sourceName, new string[] { field })
        {
        }

        public Field(string sourceName, string[] fields)
        {
            SourceName = sourceName;
            Fields = fields;
        }

        public string SourceName { get; protected set; }

        public string[] Fields { get; protected set; }
    }
}
