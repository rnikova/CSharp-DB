CREATE DATABASE CarRental

USE CarRental

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate INT,
	WeeklyRate INT,
	MonthlyRate INT,
	WeekendRate INT
)

CREATE TABLE Cars(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	PlateNumber INT NOT NULL,
	Manufacturer NVARCHAR(50) NOT NULL,
	Model NVARCHAR(30) NOT NULL,
	CarYear INT NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Doors INT NOT NULL,
	Picture IMAGE,
	Condition NVARCHAR(MAX),
	Available VARCHAR(5) NOT NULL CHECK(Available = 'Yes' Or Available = 'No')
)

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(20),
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Customers (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	DriverLicenceNumber NVARCHAR(30) NOT NULL,
	FullName NVARCHAR(MAX) NOT NULL,
	[Address] NVARCHAR(MAX),
	City NVARCHAR(30),
	ZIPCode NVARCHAR(20),
	Notes NVARCHAR(MAX) 
)

CREATE TABLE RentalOrders (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
	CarId INT FOREIGN KEY REFERENCES Cars(Id) NOT NULL,
	TankLevel INT,
	KilometrageStart DECIMAL,
	KilometrageEnd DECIMAL,
	TotalKilometrage DECIMAL,
	StartDate DATETIME,
	EndDate DATETIME,
	TotalDays INT,
	RateApplied INT,
	TaxRate INT,
	OrderStatus NVARCHAR(MAX),
	Notes NVARCHAR(MAX) 
)

INSERT INTO Categories(CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate) VALUES
('VALUE1', NULL, NULL, NULL, NULL),
('VALUE2', NULL, NULL, NULL, NULL),
('VALUE3', NULL, NULL, NULL, NULL)

INSERT INTO Cars(PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available) VALUES
(1234, 'VALUE1', 'VALUE1', 2000, 1, 2, NULL, NULL, 'YES'),
(12345, 'VALUE2', 'VALUE2', 2002, 2, 5, NULL, NULL, 'NO'),
(123456, 'VALUE3', 'VALUE3', 2004, 3, 2, NULL, NULL, 'YES')

INSERT INTO Employees(FirstName, LastName, Title, Notes) VALUES
('First', 'Name', NULL, NULL),
('Second', 'Name', NULL, NULL),
('Third', 'Name', NULL, NULL)

INSERT INTO Customers(DriverLicenceNumber, FullName, [Address], City, ZIPCode, Notes) VALUES
('VALUE1', 'NAME', NULL, NULL, NULL, NULL),
('VALUE2', 'NAME', NULL, NULL, NULL, NULL),
('VALUE3', 'NAME', NULL, NULL, NULL, NULL)

INSERT INTO RentalOrders(EmployeeId, CustomerId, CarId) VALUES
(1, 1, 1),
(2, 2, 2),
(3, 3, 3)