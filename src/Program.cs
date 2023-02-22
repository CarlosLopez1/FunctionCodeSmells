/*
    Clean Code - Chapter 17: Smells and Heuristics - Functions

    F1: Too Many Arguments
    F2: Output Arguments
    F3: Flag Arguments
    F4: Dead Function
*/

public class Program {
    public static void Main(string[] args) {
        DisplayMessage("*** A very sophisticated student grades dashboard ***", centered: true, ConsoleColor.Green);

        DisplayGrades("Cleo", "Strong", new (string, int)[] { ("Coding", new Random().Next(5, 11)), ("Math", new Random().Next(5, 11)), ("Science", new Random().Next(5, 11)) }, lastNameFirst: true, showLetterGrade: false);
        DisplayGrades("Olivia", "Allen", new (string, int)[] { ("Coding", new Random().Next(5, 11)), ("Math", new Random().Next(5, 11)), ("Science", new Random().Next(5, 11)) }, lastNameFirst: false, showLetterGrade: false);
        DisplayGrades("Fred", "Cisneros", new (string, int)[] { ("Coding", new Random().Next(5, 11)), ("Math", new Random().Next(5, 11)), ("Science", new Random().Next(5, 11)) }, lastNameFirst: true, showLetterGrade: true);
        DisplayGrades("Julia", "Pacheco", new (string, int)[] { ("Coding", new Random().Next(5, 11)), ("Math", new Random().Next(5, 11)), ("Science", new Random().Next(5, 11)) }, lastNameFirst: false, showLetterGrade: false);
        DisplayGrades("Gene", "Dixon", new (string, int)[] { ("Coding", new Random().Next(5, 11)), ("Math", new Random().Next(5, 11)), ("Science", new Random().Next(5, 11)) }, lastNameFirst: true, showLetterGrade: true);

        Console.Write("\n\n\nPress any key to close...");
        Console.ReadKey();
    }

    private static void DisplayGrades(string firstName, string lastName, (string, int)[] grades, bool lastNameFirst, bool showLetterGrade) {
        string message;
        
        string formattedName = lastNameFirst ? $"{lastName}, {firstName}" : $"{firstName} {lastName}";
        message = $"Student: { formattedName }\n";

        var maxSubjectLength = grades.Select(grade => grade.Item1)
            .ToList()
            .Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur)
            .Length;

        foreach (var grade in grades)
        {
            string formattedGrade = grade.Item2.ToString();

            if (showLetterGrade && !GetLetterGrade(grade.Item2, out formattedGrade))
                formattedGrade = $"!INVALID";
            
            var padding = new string(' ', maxSubjectLength - grade.Item1.Length);

            message += $"\tSubject: {grade.Item1}{padding} - Grade: {formattedGrade}\n";
        }

        DisplayMessage(message);
    }

    private static bool GetLetterGrade(int value, out string formattedGrade) {
        switch (value) {
            case 10: formattedGrade = "A"; break;
            case 9: formattedGrade = "B"; break;
            case 8: formattedGrade = "C"; break;
            case 7: formattedGrade = "D"; break;
            case 6: formattedGrade = "E"; break;
            default: formattedGrade = "F"; break;
        }
        
        return value >= 5 && value <= 10;
    }

    private static void DisplayMessage(string message, bool centered = false, ConsoleColor color = ConsoleColor.White) {
        if (centered) {
            int padding = (Console.WindowWidth + message.Length) / 2;
            message = message.PadLeft(padding);
        }

        if(color != ConsoleColor.White)
            Console.ForegroundColor = color;

        Console.WriteLine(message);   

        if(color != ConsoleColor.White)
            Console.ForegroundColor = ConsoleColor.White;
    }

    #region Mocks
    private static Grade[] GetMockgrades() {
        return new List<Student>() {
            new Student("Cleo", "Strong"),
            new Student("Olivia", "Allen"),
            new Student("Fred", "Cisneros"),
            new Student("Julia", "Pacheco"),
            new Student("Gene", "Dixon")
        }
        .Select(student => {
            return Enum.GetValues(typeof(Subject))
                .Cast<Subject>()
                .Select(subject => new Grade(student, subject, new Random().Next(5, 11)))
                .ToArray();
        })
        .SelectMany(x => x)
        .ToArray();
    }
    #endregion
}

#region types
public enum Subject {
    Math,
    Coding,
    Science
}

public enum LetterGrade {
    A = 10,
    B = 9,
    C = 8,
    D = 7,
    E = 6,
    F = 5
}

public class Student {
    public Student(string firstName, string lastname) {
        FirstName = firstName;
        LastName = lastname;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
}

public class Grade {
    public Grade(Student student, Subject subject, int value) {
        Student = student;
        Subject = subject;
        Value = value;
    }

    public Student Student { get; private set; }
    public Subject Subject { get; private set; }
    public int Value { get; private set; }
}
#endregion