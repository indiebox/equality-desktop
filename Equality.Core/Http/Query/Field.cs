namespace Equality.Http
{
    public class Field
    {
        public Field(string sourceName, params string[] fields)
        {
            SourceName = sourceName;
            Fields = fields;
        }

        public string SourceName { get; protected set; }

        public string[] Fields { get; protected set; }
    }
}
