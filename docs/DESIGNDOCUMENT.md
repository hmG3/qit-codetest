# Code Test

> This is a simple code test application to showcase my skills and maturity in software design and development.

## Software tools

- IDE:
  - Visual Studio 2017
  - Visual Studio Code
- RESTful service:
  - C#
  - .NET Core 2.0
  - ASP.NET Core 2.0 WebAPI
- ORM:
  - Entity Framework Core 2.0
- RDBMS:
  - SQL Server 2014 LocalDB or later
- TDD:
  - NUnit 3
- Client app:
  - Angular 4
  - Bootstrap 4
  - HTML 5

## Installing

### Requirements

- At least [.NET Core SDK 2.0](https://www.microsoft.com/net/download/core)
- Installed [SQL Server LocalDB](https://www.microsoft.com/en-us/download/details.aspx?id=54284)
- At least [Node.js 6.9](https://nodejs.org/en/download/) and [Yarn](https://yarnpkg.com/en/docs/install) or npm 4

### Building

Clone a repository:

```Shell
git clone https://github.com/hmG3/qit-codetest.git
```

#### Visual Studio

- Make sure configuration for external tools is correct: _Tools_ > _Options_ > _Projects and Solutions_ > _Web Package Management_ > _External Web Tools_:

```ini
.\node_modules\.bin
$(PATH)
$(VSINSTALLDIR)\Web\External
...
```

- Wait for packages restoring and build the solution

#### Command line & .NET Core CLI

- Go to the `src\QITCodeTest.App` project folder and run either:

```Shell
yarn install
```

or

```Shell
npm install
```

- Go to the `qit-codetest` root folder and run:

```Shell
dotnet restore
dotnet build
```

Wait for package restoring and building.

## Running

- You should start both service and app applications:

- From withing Visual Studio
- Or go to each folders `src\QITCodeTest.Service`, `src\QITCodeTest.App` and run:

```Shell
dotnet run
```

- Endpoint urls already configured to listen on 8080 and 8081 ports for client and service respectively.
- Service database will be automatically created during first run and populated with test data.

## Application

Client applicaion built with Angular framework and uses JiT compilation with source maps and Hot Module Replacement. It consist of two main components:

1. Classes component with child modal form component.
1. Students component with child modal form component.

Home page shows Classes and Students grids where you can permorm basic CRUD operations.

### Classes component

Basic functions:

1. List – listing of all existing Classes.
1. Add – create new Class.
1. Edit – edit an existing Class.
1. Delete – delete a Class.

Validations for Class information:

1. All fields are required.
1. The Teacher Name field must starts with a salutation like: Mr. Robertson, Ms. Sanderson.

#### Students component

Basic functions:

1. List – listing of all existing Students, filtered by selected Class. Students with a GPA higher than 3.2 highlight in red. DOB displays as Age.
1. Add – create new Student.
1. Edit – edit an existing Student.
1. Delete – delete a Student.

Validations for Student information:

1. All fields except GPA are required.
1. The birthday must be not more than 80 years of age.
1. GPA must be in a range 1.0 to 5.0;
1. Student surname must be unique within all students.

## Hosting

- To change endpoint listen address edit in _hosting.json_:

Client:

```ini
"server.urls": "http://*:8080"
```

Service:

```ini
"server.urls": "http://*:8081"
```

- To change connection to service API from client app edit in _ClientApp/app/app.shared.ts_:

```TS
{ provide: 'WEBAPI_URL', useValue: 'http://localhost:8081/api' }
```

## Testing

The solution does not include integration tests.

To perform tests on service API, from the `tests\QITCodeTest.Service.Tests` project folder run:

```Shell
dotnet test
```

## Changing DB

NOTE: For simplicity, this app uses a SQL Server LocalDB database. But this can be easily converted to a full SQL Server database or SQLite using the EF Code-First migration.

To use another database:

- Edit `ConnectionStrings` in _appsettings.json_.
- Edit in _Startup.cs_:

```C#
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
```

## The REST API

### Endpoints

1. For managing Classes: `http://<api-host>/api/classes`
1. For managing Students: `http://<api-host>/api/students`

### Help pages using Swagger

- Navigate to `http://<api-host>/swagger/v1/swagger.json` to see the document generated that describes the endpoints
- Swagger UI can be viewed by navigating to `http://<api-host>/swagger`

To use another Swagger endpoint edit in _Startup.cs_:

```C#
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QIT Code Test API v0.1"));
```
