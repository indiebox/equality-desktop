namespace Equality.Http
{
    public class Filter
    {
        public Filter(string name, params string[] values)
        {
            Name = name;
            Values = values;
        }

        public string Name { get; protected set; }

        public string[] Values { get; protected set; }
    }
}
