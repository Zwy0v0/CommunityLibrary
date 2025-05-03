public class MemberCollection
{
    private Member[] members;
    private int capacity;
    private int count = 0;

    public int Count => count;

    public MemberCollection(int initialCapacity = 10)
    {
        capacity = initialCapacity;
        members = new Member[capacity];
    }

    private void Resize()
    {
        capacity *= 2;
        Member[] newArray = new Member[capacity];
        Array.Copy(members, newArray, count);
        members = newArray;
    }

    public bool Add(Member member)
    {
        if (Find(member.FirstName, member.LastName) != null)
            return false;
        if (count == capacity)
            Resize();
        members[count++] = member;
        return true;
    }

    public Member Find(string firstName, string lastName)
    {
        for (int i = 0; i < count; i++)
        {
            if (members[i].FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                members[i].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                return members[i];
        }
        return null;
    }

    public bool Remove(string firstName, string lastName)
    {
        for (int i = 0; i < count; i++)
        {
            if (members[i].FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) &&
                members[i].LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
            {
                if (members[i].BorrowedMovies.Count > 0)
                    return false;
                Array.Copy(members, i + 1, members, i, count - i - 1);
                count--;
                return true;
            }
        }
        return false;
    }

    public Member this[int index] => (index >= 0 && index < count) ? members[index] : throw new IndexOutOfRangeException();
}