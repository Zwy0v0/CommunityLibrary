using System;
using System.Collections.Generic;

public class Program
{
    private static MovieCollection movies = new MovieCollection();
    private static MemberCollection members = new MemberCollection();
    private static Member currentMember = null;

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("COMMUNITY LIBRARY MOVIE DVD MANAGEMENT SYSTEM");
            Console.WriteLine("Main Menu");
            Console.WriteLine("1. Staff");
            Console.WriteLine("2. Member");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice ==> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StaffLogin();
                    break;
                case "2":
                    MemberLogin();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void StaffLogin()
    {
        Console.Write("Username: ");
        string user = Console.ReadLine();
        Console.Write("Password: ");
        string pass = ReadPassword();
        if (user == "staff" && pass == "today123")
            StaffMenu();
        else
        {
            Console.WriteLine("Login failed. Press any key to continue.");
            Console.ReadKey();
        }
    }

    static void MemberLogin()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Password: ");
        string pass = ReadPassword();
        Member member = members.Find(firstName, lastName);
        if (member != null && member.Password == pass)
        {
            currentMember = member;
            MemberMenu();
        }
        else
        {
            Console.WriteLine("Login failed. Press any key to continue.");
            Console.ReadKey();
        }
    }

    static string ReadPassword()
    {
        string pass = "";
        ConsoleKeyInfo key;
        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Enter)
            {
                pass += key.KeyChar;
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);
        Console.WriteLine();
        return pass;
    }

    static void StaffMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("\nStaff Menu");
            Console.WriteLine("1. Add DVDs");
            Console.WriteLine("2. Remove DVDs");
            Console.WriteLine("3. Register Member");
            Console.WriteLine("4. Remove Member");
            Console.WriteLine("5. Find Member Phone");
            Console.WriteLine("6. Find Members Renting Movie");
            Console.WriteLine("0. Return to Main Menu");
            Console.Write("Enter your choice ==> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddDVDs();
                    break;
                case "2":
                    RemoveDVDs();
                    break;
                case "3":
                    RegisterMember();
                    break;
                case "4":
                    RemoveMember();
                    break;
                case "5":
                    FindMemberPhone();
                    break;
                case "6":
                    FindMembersRentingMovie();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AddDVDs()
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();
        Movie existing = movies.Get(title);
        if (existing != null)
        {
            Console.Write("Number of copies to add: ");
            if (int.TryParse(Console.ReadLine(), out int copies) && copies > 0)
            {
                existing.AddCopies(copies);
                Console.WriteLine($"{copies} copies added. Press any key to continue.");
            }
            else
                Console.WriteLine("Invalid number. Press any key to continue.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Enter genre (Drama, Adventure, Family, Action, SciFi, Comedy, Animated, Thriller, Other): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out Genre genre))
            {
                Console.WriteLine("Invalid genre. Press any key to continue.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter classification (G, PG, M15, MA15): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out Classification classification))
            {
                Console.WriteLine("Invalid classification. Press any key to continue.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter duration (minutes): ");
            if (!int.TryParse(Console.ReadLine(), out int duration) || duration <= 0)
            {
                Console.WriteLine("Invalid duration. Press any key to continue.");
                Console.ReadKey();
                return;
            }

            Console.Write("Enter number of copies: ");
            if (!int.TryParse(Console.ReadLine(), out int copies) || copies <= 0)
            {
                Console.WriteLine("Invalid copies. Press any key to continue.");
                Console.ReadKey();
                return;
            }

            Movie newMovie = new Movie(title, genre, classification, duration, copies);
            movies.Add(title, newMovie);
            Console.WriteLine($"Movie '{title}' added. Press any key to continue.");
            Console.ReadKey();
        }
    }

    static void RemoveDVDs()
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();
        Movie movie = movies.Get(title);
        if (movie == null)
        {
            Console.WriteLine("Movie not found. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Console.Write($"Current copies: {movie.TotalCopies}. Enter number to remove: ");
        if (int.TryParse(Console.ReadLine(), out int num) && num > 0)
        {
            if (movie.RemoveCopies(num))
            {
                Console.WriteLine($"{num} copies removed. Press any key to continue.");
                if (movie.TotalCopies == 0)
                    movies.Remove(title);
            }
            else
                Console.WriteLine("Cannot remove more than available. Press any key to continue.");
        }
        else
            Console.WriteLine("Invalid number. Press any key to continue.");
        Console.ReadKey();
    }

    static void RegisterMember()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();
        if (members.Find(firstName, lastName) != null)
        {
            Console.WriteLine("Member already exists. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Console.Write("Phone Number: ");
        string phone = Console.ReadLine();
        Console.Write("Set 4-digit Password: ");
        string password = Console.ReadLine();
        if (password.Length != 4 || !int.TryParse(password, out _))
        {
            Console.WriteLine("Password must be 4 digits. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Member newMember = new Member(firstName, lastName, phone, password);
        members.Add(newMember);
        Console.WriteLine("Member registered. Press any key to continue.");
        Console.ReadKey();
    }

    static void RemoveMember()
    {
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();
        if (members.Remove(firstName, lastName))
            Console.WriteLine("Member removed. Press any key to continue.");
        else
            Console.WriteLine("Member not found or has unreturned DVDs. Press any key to continue.");
        Console.ReadKey();
    }

    static void FindMemberPhone()
    {
        Console.Write("Enter first name: ");
        string firstName = Console.ReadLine();
        Console.Write("Enter last name: ");
        string lastName = Console.ReadLine();
        Member member = members.Find(firstName, lastName);
        if (member != null)
            Console.WriteLine($"Phone: {member.PhoneNumber}");
        else
            Console.WriteLine("Member not found.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    static void FindMembersRentingMovie()
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();
        bool found = false;
        for (int i = 0; i < members.Count; i++)
        {
            if (members[i].BorrowedMovies.Contains(title))
            {
                Console.WriteLine($"{members[i].FirstName} {members[i].LastName}");
                found = true;
            }
        }
        if (!found)
            Console.WriteLine("No members renting this movie.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    static void MemberMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"\nMember Menu (Logged in as: {currentMember.FirstName} {currentMember.LastName})");
            Console.WriteLine("1. Browse All Movies");
            Console.WriteLine("2. Display Movie Details");
            Console.WriteLine("3. Borrow Movie");
            Console.WriteLine("4. Return Movie");
            Console.WriteLine("5. List My Borrowed Movies");
            Console.WriteLine("6. Display Top 3 Movies");
            Console.WriteLine("0. Logout");
            Console.Write("Enter your choice ==> ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    BrowseAllMovies();
                    break;
                case "2":
                    DisplayMovieDetails();
                    break;
                case "3":
                    BorrowMovie();
                    break;
                case "4":
                    ReturnMovie();
                    break;
                case "5":
                    ListBorrowedMovies();
                    break;
                case "6":
                    DisplayTop3Movies();
                    break;
                case "0":
                    currentMember = null;
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void BrowseAllMovies()
    {
        Console.WriteLine("All Movies (Sorted by Title):");
        var allMovies = movies.GetAllMovies();
        allMovies.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase));
        foreach (var movie in allMovies)
            Console.WriteLine($"{movie.Title} | Genre: {movie.Genre} | Available: {movie.AvailableCopies}/{movie.TotalCopies}");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    static void DisplayMovieDetails()
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();
        Movie movie = movies.Get(title);
        if (movie != null)
        {
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Genre: {movie.Genre}");
            Console.WriteLine($"Classification: {movie.Classification}");
            Console.WriteLine($"Duration: {movie.Duration} minutes");
            Console.WriteLine($"Available Copies: {movie.AvailableCopies}/{movie.TotalCopies}");
            Console.WriteLine($"Times Borrowed: {movie.BorrowCount}");
        }
        else
            Console.WriteLine("Movie not found.");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    static void BorrowMovie()
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();
        Movie movie = movies.Get(title);
        if (movie == null)
        {
            Console.WriteLine("Movie not found. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        if (currentMember.BorrowedMovies.Count >= 5)
        {
            Console.WriteLine("You have reached the maximum borrowing limit (5). Press any key to continue.");
            Console.ReadKey();
            return;
        }

        if (currentMember.BorrowedMovies.Contains(title))
        {
            Console.WriteLine("You have already borrowed this movie. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        if (movie.Borrow())
        {
            currentMember.BorrowMovie(title);
            Console.WriteLine($"Movie '{title}' borrowed successfully. Press any key to continue.");
        }
        else
            Console.WriteLine("No available copies. Press any key to continue.");
        Console.ReadKey();
    }

    static void ReturnMovie()
    {
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();
        if (!currentMember.BorrowedMovies.Contains(title))
        {
            Console.WriteLine("You did not borrow this movie. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Movie movie = movies.Get(title);
        if (movie != null)
        {
            movie.Return();
            currentMember.ReturnMovie(title);
            Console.WriteLine($"Movie '{title}' returned. Press any key to continue.");
        }
        else
            Console.WriteLine("Movie not found in library. Press any key to continue.");
        Console.ReadKey();
    }

    static void ListBorrowedMovies()
    {
        if (currentMember.BorrowedMovies.Count == 0)
        {
            Console.WriteLine("You have no borrowed movies. Press any key to continue.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Your Borrowed Movies:");
        foreach (string title in currentMember.BorrowedMovies)
            Console.WriteLine(title);
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    static void DisplayTop3Movies()
    {
        var allMovies = movies.GetAllMovies();
        allMovies.Sort((a, b) => b.BorrowCount.CompareTo(a.BorrowCount));
        int topCount = Math.Min(3, allMovies.Count);
        Console.WriteLine("Top 3 Most Borrowed Movies:");
        for (int i = 0; i < topCount; i++)
            Console.WriteLine($"{i + 1}. {allMovies[i].Title} (Borrowed {allMovies[i].BorrowCount} times)");
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }
}