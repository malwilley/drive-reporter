# Drive Reporter Code Sample

## Running/Testing

Requires .NET Core 2.0 SDK

```
dotnet run  --project DriveReporter [inputFilePath]
```

```
dotnet test UnitTests
```

## Thought Process

### Dependency injection

I chose to structure this app using dependency injection so that my components would be loosely coupled and more easily tested in isolation. This pattern works particularly well with mocking frameworks which I made use of in unit testing. Dependencies can be mocked and asserted on which allows us to test **only** the logic within the class being tested. This is particularly important when class relationships become more complicated or when dependencies cause side-effects (such as reading a file in the case of this app).

### Use of interfaces

I made liberal use of interfaces (ICommand, ICommandFactory, IDriveReport, etc) in making this app. Interfaces are necessary for dependency injection, but also allow us other benefits. When designing against interfaces, we don't need to (or rather, can't) be concerned about how classes are implemented. In more robust architectures, these interfaces could be extracted to an inner layer and be implemented in outer layers.

### File structure

Instead of organizing files by type (Interfaces, Models, Services, etc), I have organized them by related function (Commands, Inputs, Reporting). It is a minor detail but I find that this type of folder structure can be more productive.

### Other design decisions

#### DriveReport data structure

I chose to implement the IDriveReport interface with a Dictionary map of driver names to associated data. With this setup inserts and lookups are O(1), while sorting is O(N*LogN). I found this a good compromise since sorting only occurs once at the end, while inserts and lookups can happen many times per run. This could also be implemented with a BST or other sorted structure if the end sorting time was unacceptable.

#### Mostly-immutable

When desiging any bit of code I always try to keep state mutation to a minimum. In places where mutation is necessary for performance or convenience it is at least isolated. In this application, DriveReport is implemented as a mutable data structure, but still returns itself after operations as if it were immutable. This way we can still map/filter/reduce on it easily without having to allocate space for new objects. However, this does come at the cost of having to keep in mind that these operations do in fact mutate the object.