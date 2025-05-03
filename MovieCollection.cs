public class MovieCollection
{
    private class Node
    {
        public string Key { get; }
        public Movie Value { get; set; }
        public Node Next { get; set; }
        public Node(string key, Movie value) => (Key, Value) = (key, value);
    }

    private Node[] buckets;
    private const int InitialCapacity = 101;
    private int count = 0;

    public MovieCollection() => buckets = new Node[InitialCapacity];

    private int GetHash(string key)
    {
        int hash = 0;
        foreach (char c in key)
            hash = (hash * 31 + c) % buckets.Length;
        return hash;
    }

    public void Add(string title, Movie movie)
    {
        int index = GetHash(title);
        Node node = buckets[index];
        while (node != null)
        {
            if (node.Key.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                node.Value.AddCopies(movie.TotalCopies);
                return;
            }
            node = node.Next;
        }
        buckets[index] = new Node(title, movie) { Next = buckets[index] };
        count++;
    }

    public Movie Get(string title)
    {
        int index = GetHash(title);
        Node node = buckets[index];
        while (node != null)
        {
            if (node.Key.Equals(title, StringComparison.OrdinalIgnoreCase))
                return node.Value;
            node = node.Next;
        }
        return null;
    }

    public bool Remove(string title)
    {
        int index = GetHash(title);
        Node prev = null;
        Node current = buckets[index];
        while (current != null)
        {
            if (current.Key.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                if (prev == null)
                    buckets[index] = current.Next;
                else
                    prev.Next = current.Next;
                count--;
                return true;
            }
            prev = current;
            current = current.Next;
        }
        return false;
    }

    public List<Movie> GetAllMovies()
    {
        List<Movie> allMovies = new List<Movie>();
        foreach (var bucket in buckets)
        {
            Node current = bucket;
            while (current != null)
            {
                allMovies.Add(current.Value);
                current = current.Next;
            }
        }
        return allMovies;
    }
}