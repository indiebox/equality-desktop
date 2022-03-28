namespace Equality.Http
{
    public class Filter
    {
        public Filter(string name, string value) : this(name, new string[] { value })
        {
        }

        public Filter(string name, string[] values)
        {
            Name = name;
            Values = values;
        }

        public string Name { get; protected set; }

        public string[] Values { get; protected set; }
    }
}
