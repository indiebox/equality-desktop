namespace Equality.Http
{
    public class Sort
    {
        public Sort(string name, bool desc = false)
        {
            Name = name;
            Descending = desc;
        }

        public string Name { get; protected set; }

        public bool Descending { get; protected set; }
    }
}
