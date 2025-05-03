public class Member
{
    public string FirstName { get; }
    public string LastName { get; }
    public string PhoneNumber { get; }
    public string Password { get; }
    public List<string> BorrowedMovies { get; } = new List<string>();

    public Member(string firstName, string lastName, string phone, string password)
    {
        if (password.Length != 4 || !int.TryParse(password, out _))
            throw new ArgumentException("Password must be 4 digits.");
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phone;
        Password = password;
    }

    public bool BorrowMovie(string title)
    {
        if (BorrowedMovies.Count >= 5 || BorrowedMovies.Contains(title))
            return false;
        BorrowedMovies.Add(title);
        return true;
    }

    public bool ReturnMovie(string title) => BorrowedMovies.Remove(title);
}