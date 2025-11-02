Answers to Technical Questions

1. How much time did you spend on this task?

Approximately 6–8 hours were spent designing and implementing the API, services, models, web view with Bootstrap, and integrating with the OpenWeather API.

2. If you had more time, what improvements or additions would you make?
- Allow continent selection and display countries accordingly.
- Provide a list of countries and the ability to browse them.
- Display historical weather changes for countries and cities.
- Add a comprehensive list of all cities in the world for search.
- Introduce premium features, such as alerts for bad weather or extreme conditions.

3. The most useful feature recently added to your favorite programming language

In C# 11, features like list patterns and raw string literals are very useful.

Example: Raw string literal
```csharp
string jsonTemplate = """
{
    "name": "Ahmad",
    "role": "Developer"
}
""";
Console.WriteLine(jsonTemplate);
```

Example: List pattern matching
```csharp
int[] numbers = { 1, 2, 3 };
if (numbers is [1, 2, 3])
{
    Console.WriteLine("Exact match!");
}
```

4. How do you identify and diagnose a performance issue in production? Have you done this before?

Methods:
- Usually, I use diagnostic tools to monitor how resource usage (CPU, memory, disk, network) changes under different scenarios.
- Check for slow database queries or external service calls.
- Review application logs to see which parts of the system are taking longer than expected.
- Investigate the system to find root causes and optimize performance.

Experience: Yes, I have used this approach to identify performance issues in production and apply improvements.

5. Last technical book or conference you attended and what did you learn?

Courses: 
- "Qt Framework for C++ Developers"
  - Learned how to build cross-platform GUI applications using Qt.
  - Practiced signals and slots, widgets, layouts, and event handling.

- "Object-Oriented Programming in C++"
  - Strengthened understanding of classes, inheritance, polymorphism, and templates.
  - Learned best practices for designing maintainable and extensible C++ applications.

6. Your opinion about this technical test

This test is less stressful compared to live interviews:
- Gives enough time for the candidate to think and plan their solution.
- Suitable for those who may perform poorly in traditional interview settings.
- The test is most effective when the same questions are not repeated in the live interview, and the interview mainly focuses on personality and general questions.

7. Describe yourself in JSON format:

```json
{
  "name": "Ahmad Hatami",
  "age": 33,
  "role": "Software Developer",
  "experienceYears": 8,
  "skills": [
    "C#",
    ".NET Core",
    "ASP.NET MVC",
    "Web API",
    "Entity Framework",
    "JavaScript",
    "Bootstrap",
    "SQL",
    "Software Reverse Engineering",
    "REST API Design"
  ],
  "favoriteLanguage": "C#",
  "learningInterest": ["BI", "Microservices", "AI", "DevOps", "Dapper"]
}
```