# Endava .NET Code Challenge

An ASP.NET Core MVC application that allows users to see the movies that are available and highlight which movie provider is streaming at the cheapest price.
The external API sharing the movie catalogue isn't reliable. To ensure the site can remain functional and be able to handle any instability we retry any failed API requests.

# Prerequisites
- .NET 6.0 - `brew install --cask dotnet-sdk`

# Libraries and Frameworks
- [ASP.NET Core MVC](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview) - Framework for building web apps and APIs using the Model-View-Controller design pattern
- [Polly](https://github.com/App-vNext/Polly) - Library that provides resilience and transient-fault handling capabilities. Used to apply Polly's Retry policy
- [XUnit](https://xunit.net/) - Framework for unit testing in .NET
- [Moq](https://github.com/moq/moq) - Framework for mocking
- [Bootstrap](https://getbootstrap.com/docs/5.1/getting-started/introduction/) - Framework for UI/UX

# Run
- `dotnet run --project Endava.PrincesTheatre` to start app (default port is 5000)
- `dotnet watch run --project Endava.PrincesTheatre` to run with hot-reload
- `dotnet test Endava.PrincesTheatre.Test` to run test suite


# High-level design
<img width="1333" alt="princes-theatre-dotnet" src="https://user-images.githubusercontent.com/29138219/201549588-beb30aec-33f6-488e-994f-8bddfc48cbee.png">

