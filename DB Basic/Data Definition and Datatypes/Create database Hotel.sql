CREATE DATABASE Hotel

USE Hotel

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(20),
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Customers (
	AccountNumber INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	PhoneNumber INT,
	EmergencyName NVARCHAR(MAX),
	EmergencyNumber INT,
	Notes NVARCHAR(MAX) 
)

CREATE TABLE RoomStatus(
	RoomStatus NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE RoomTypes(
	RoomType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE BedTypes(
	BedType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Rooms (
	RoomNumber INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	RoomType NVARCHAR(50) FOREIGN KEY REFERENCES RoomTypes(RoomType) NOT NULL,
	BedType NVARCHAR(50) FOREIGN KEY REFERENCES BedTypes(BedType) NOT NULL,
	Rate INT,
	RoomStatus NVARCHAR(50) FOREIGN KEY REFERENCES RoomStatus(RoomStatus) NOT NULL,
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Payments  (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	PaymentDate DATETIME,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	FirstDateOccupied DATETIME,
	LastDateOccupied DATETIME,
	TotalDays INT,
	AmountCharged DECIMAL(10,2),
	TaxRate INT,
	TaxAmount DECIMAL(10,2),
	PaymentTotal DECIMAL(10,2),
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Occupancies   (
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	DateOccupied  DATETIME,
	AccountNumber INT FOREIGN KEY REFERENCES Customers(AccountNumber) NOT NULL,
	RoomNumber INT FOREIGN KEY REFERENCES Rooms(RoomNumber) NOT NULL,
	RateApplied INT,
	PhoneCharge INT,
	Notes NVARCHAR(MAX) 
)

INSERT INTO Employees(FirstName, LastName) VALUES
('FIRST', 'NAME'),
('SECOND', 'NAME'),
('THIRD', 'NAME')

INSERT INTO Customers(FirstName, LastName) VALUES
('FIRST', 'NAME'),
('SECOND', 'NAME'),
('THIRD', 'NAME')

INSERT INTO RoomStatus(RoomStatus) VALUES
('STATUS1'),
('STATUS2'),
('STATUS3')

INSERT INTO RoomTypes(RoomType) VALUES
('Type1'),
('Type2'),
('Type3')

INSERT INTO BedTypes(BedType) VALUES
('Type1'),
('Type2'),
('Type3')

INSERT INTO Rooms(RoomType, BedType, RoomStatus) VALUES
('Type1', 'Type1', 'STATUS1'),
('Type2', 'Type2', 'STATUS2'),
('Type3', 'Type3', 'STATUS3')

INSERT INTO Payments(EmployeeId, AccountNumber) VALUES
(1, 1),
(2, 2),
(3, 3)

INSERT INTO Occupancies(EmployeeId, AccountNumber, RoomNumber) VALUES
(1, 1, 2),
(2, 2, 2),
(3, 3, 3)

UPDATE Payments
SET TaxRate *= 0.97

SELECT TaxRate FROM Payments

TRUNCATE TABLE Occupancies