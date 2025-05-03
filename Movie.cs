public enum Genre { Drama, Adventure, Family, Action, SciFi, Comedy, Animated, Thriller, Other }
public enum Classification { G, PG, M15, MA15 }

public class Movie
{
    public string Title { get; }
    public Genre Genre { get; }
    public Classification Classification { get; }
    public int Duration { get; }
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }
    public int BorrowCount { get; private set; }

    public Movie(string title, Genre genre, Classification classification, int duration, int copies)
    {
        Title = title;
        Genre = genre;
        Classification = classification;
        Duration = duration;
        TotalCopies = copies;
        AvailableCopies = copies;
        BorrowCount = 0;
    }

    public bool Borrow()
    {
        if (AvailableCopies > 0)
        {
            AvailableCopies--;
            BorrowCount++;
            return true;
        }
        return false;
    }

    public void Return()
    {
        if (AvailableCopies < TotalCopies)
            AvailableCopies++;
    }

    public void AddCopies(int num)
    {
        TotalCopies += num;
        AvailableCopies += num; // 同步更新可用副本
    }

    public bool RemoveCopies(int num)
    {
        if (num <= TotalCopies)
        {
            TotalCopies -= num;
            AvailableCopies = Math.Min(AvailableCopies, TotalCopies);
            return true;
        }
        return false;
    }
}