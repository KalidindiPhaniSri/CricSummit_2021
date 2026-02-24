# CricSummit_2021
A console application built using .NET Core.

## Description
CricSummit 2021 is a .NET console application that predicts cricket scores and generates commentary using rule-based logic based on batting and bowling types. 
It simulates match progress, handles Super Over scenarios, and determines the winning team.

## Features
- Predict match score and commentary
- Handle Super Over scenarios
- Generate ball-by-ball commentary
- Determine match winner in Super Over.

## Installation
Follow the steps below to set up and run this project locally.

### Prerequisites
Make sure the following tools are installed on your system:

- **.NET 8 SDK** 
This project targets .NET 8.
Download and install from : https://dotnet.microsoft.com/en-us/download/dotnet/8.0

- **Git**
Download Git from : https://git-scm.com/install/

### Clone the repository
git clone https://github.com/KalidindiPhaniSri/CricSummit_2021

Navigate into the project folder
```bash 
cd CricSummit.Console 
```

Restore dependencies 
```bash  
dotnet restore 
```

Build the project 
```bash  
 dotnet build
 ```
 
 Run the project
 ```bash
 dotnet run
 ```

## Usage
Provide input through the console. Currently, only a Console Provider is implemented.

### Input Format
Follow this format for each input
bowlingtype_ battingtype_timing

To test multiple combinations, separate them by a space
Ex: fast_defensive_late spin_aggressive_early

## Technologies used
- .NET 8
- C#
- LINQ
- Domain Driven Design (DDD)
- XUnit (for unit testing)

## Project Structure

### CricSummit.Console
This layer interacts directly with the user. It validates input, invokes the relevant application services, and displays the output.

### CricSummit.Application
This layer handles orchestration logic. It coordinates domain services and works with domain objects to execute business use cases.

### CricSummit.Domain
This layer contains all core business logic and domain behavior. It is implemented using Domain-Driven Design principles.

### CricSummit.Tests
This project contains unit tests written using xUnit to ensure correctness and reliability of the application.
