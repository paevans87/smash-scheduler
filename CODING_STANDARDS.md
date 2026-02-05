# Coding Standards & Preferences

## 1. Linguistics (UK English)
- **Rule**: All identifiers, strings, and documentation must use British English exclusively.
- **Good**: `optimise`, `colour`, `initialise`.
- **Bad**: `optimize`, `color`, `initialize`.

## 2. Zero-Comment Policy
- **Rule**: No inline, JSDoc, or block comments allowed. Code must be self-documenting through clear naming.

## 3. Structural Preferences: Mandatory Braces
- **Rule**: Always use curly braces for conditional and loop blocks, even for single-line statements.
- **Bad**:
  if (!user || !user.active) return false;
- **Good**:
  if (!user || !user.active) {
    return false;
  }

## 4. Guard Clauses & Early Returns
- **Rule**: Use the "guard clause" pattern to handle errors/exceptions at the top of functions.
- **Rule**: Every guard clause must follow the Mandatory Braces rule (multiline).
- **Good**:
  if (isMissingData) {
    return null;
  }

## 5. Critic Agent Enforcement
The **Critic** must reject code that:
1. Omits curly braces `{}` for any `if`, `else`, `for`, or `while` statement.
2. Uses US English spelling.
3. Contains any comments (except for explicit `TODO` items requested by Architect).

## 6. Architecture: SOLID Principles
- **Single Responsibility (SRP)**: Each class must have one reason to change. If a class handles logic and I/O, the Critic must reject it.
- **Dependency Inversion (DIP)**: High-level modules must not depend on low-level modules. Use interfaces for all dependencies.

## 7. Testing & Quality Assurance
- **Rule**: Every new feature requires a corresponding unit test file.
- **Edge Cases**: Tests must cover null inputs, empty strings, and out-of-range values.
- **Workflow**: The Coder is responsible for executing `dotnet test` and ensuring 100% pass rate before finishing.

## 8. C# Specifics: Records vs Classes
- **Rule**: Use `record` for Data Transfer Objects (DTOs), API responses, and immutable state.
- **Rule**: Use `class` only when managing complex behavior, internal state mutation, or where reference equality is required.
- **Example**: `public record UserDto(string FullName, string Email);`

## 9. API Design: RESTful Standards
- **Rule**: APIs must follow standard REST patterns (nouns for resources, plural paths, correct HTTP verbs).
- **Exceptions**: Only the Architect can override this (e.g., for GraphQL or WebSockets) in `specs/architecture.md`.

## 10. Security First
- **Rule**: NO code should introduce potential attack vectors.
- **Exceptions**: NONE

## 11. Primary Constructors (C# 12+)
- **Rule**: Prefer primary constructors for classes with simple constructor parameter to field/property assignment patterns.
- **Good**:
  ```csharp
  public class ClubService(IClubRepository clubRepository) : IClubService
  {
      public async Task<Club?> GetByIdAsync(Guid id)
      {
          return await clubRepository.GetByIdAsync(id);
      }
  }
  ```
- **Bad**:
  ```csharp
  public class ClubService : IClubService
  {
      private readonly IClubRepository _clubRepository;

      public ClubService(IClubRepository clubRepository)
      {
          _clubRepository = clubRepository;
      }
  }
  ```
- **Note**: Traditional constructors are acceptable when constructor logic is complex (validation, initialisation beyond simple assignment).

## 12. One Class Per File
- **Rule**: Each class, record, or interface must live in its own file. Multiple class definitions in a single .cs file are prohibited.
- **Exceptions**: Nested classes that are private implementation details of the parent class are permitted.
- **File Naming**: The filename must match the type name exactly (e.g., `ClubService.cs` contains `public class ClubService`).

## 13. Line Length
- **Rule**: Single lines of code may extend up to 180 characters before requiring a line break.
- **Rationale**: Modern displays support wider code viewing, and 180 characters provides a balance between readability and reducing vertical scrolling.
- **Exceptions**: If a line exceeds 180 characters, break it at logical points (method parameters, chained methods, operators).

## 14. LINQ Formatting
- **Rule**: LINQ queries with chained methods must line-break at each method chain.
- **Good**:
  ```csharp
  var activePlayers = players
      .Where(p => p.IsActive)
      .Select(p => p.Name)
      .ToList();
  ```
- **Bad**:
  ```csharp
  var activePlayers = players.Where(p => p.IsActive).Select(p => p.Name).ToList();
  ```
- **Rationale**: Line-breaking chained methods improves readability and makes debugging easier by isolating each transformation step.

## 15. Frontend - Components
- **Rule**: Code should be broken down into smaller atomic components to remove duplication, improve isolation and ease of maintenance.