CREATE DATABASE ReportService

USE ReportService

CREATE TABLE Users
(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	Username NVARCHAR(30) NOT NULL UNIQUE,
	[Password] NVARCHAR(50) NOT NULL,
	[Name] NVARCHAR(50),
	[Gender] VARCHAR(1) CHECK([Gender] = 'F' OR [Gender] = 'M'),
	BirthDate DATETIME,
	Age INT,
	Email NVARCHAR(50) NOT NULL
)

CREATE TABLE Departments
(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL
)

CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	FirstName NVARCHAR(25),
	LastName NVARCHAR(25),
	[Gender] VARCHAR(1) CHECK([Gender] = 'F' OR [Gender] = 'M'),
	BirthDate DATETIME,
	Age INT,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id) NOT NULL
)

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(50) NOT NULL,
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
)

CREATE TABLE [Status]
(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	Label NVARCHAR(30) NOT NULL
)

CREATE TABLE Reports
(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	StatusId INT FOREIGN KEY REFERENCES [Status](Id) NOT NULL,
	OpenDate DATETIME NOT NULL,
	CloseDate DATETIME,
	[Description] NVARCHAR(200),
	UserId INT FOREIGN KEY REFERENCES Users(Id) NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id)
)

GO

INSERT INTO Employees(Firstname,
                      Lastname,
                      Gender,
                      Birthdate,
                      DepartmentId)
VALUES
('Marlo', 'O�Malley', 'M', '09/21/1958', '1'),
('Niki', 'Stanaghan', 'F', '11/26/1969', '4'),
('Ayrton', 'Senna', 'M', '03/21/1960 ', '9'),
('Ronnie', 'Peterson', 'M', '02/14/1944', '9'),
('Giovanna', 'Amati', 'F', '07/20/1959', '5')

INSERT INTO Reports(CategoryId,
                    StatusId,
                    OpenDate,
                    CloseDate,
                    Description,
                    UserId,
                    EmployeeId)
VALUES
('1', '1', '04/13/2017', NULL, 'Stuck Road on Str.133', '6', '2'),
('6', '3', '09/05/2015', '12/06/2015', 'Charity trail running', '3', '5'),
('14', '2', '09/07/2015', NULL, 'Falling bricks on Str.58', '5', '2'),
('4', '3', '07/03/2017', '07/06/2017', 'Cut off streetlight on Str.11', '1', '1')



GO

UPDATE Reports
SET StatusId = 2
WHERE StatusId = 1 AND CategoryId = 4

GO

DELETE Reports
WHERE StatusId = 4