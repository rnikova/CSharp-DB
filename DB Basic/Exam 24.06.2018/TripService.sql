CREATE DATABASE TripService

USE TripService

CREATE TABLE Cities(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(20) NOT NULL,
	CountryCode CHAR(2) NOT NULL
)

CREATE TABLE Hotels(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(30) NOT NULL,
	CityId INT FOREIGN KEY REFERENCES Cities(Id) NOT NULL,
	EmployeeCount INT NOT NULL,
	BaseRate DECIMAL(15, 2)
)

CREATE TABLE Rooms(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	Price DECIMAL(15, 2) NOT NULL,
	[Type] NVARCHAR(20) NOT NULL,
	Beds INT NOT NULL,
	HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL
)

CREATE TABLE Trips(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	RoomId INT FOREIGN KEY REFERENCES Rooms(Id) NOT NULL,
	BookDate DATE NOT NULL,
	ArrivalDate DATE NOT NULL,
	ReturnDate DATE NOT NULL,
	CancelDate DATE,

	CONSTRAINT CH_BookDate_ArrivalDate CHECK(BookDate < ArrivalDate),
	CONSTRAINT CH_ArrivalDate_ReturnDate CHECK(ArrivalDate < ReturnDate)
)

CREATE TABLE Accounts(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(20),
	LastName NVARCHAR(50) NOT NULL,
	CityId INT FOREIGN KEY REFERENCES Cities(Id) NOT NULL,
	BirthDate DATE NOT NULL,
	Email VARCHAR(100) NOT NULL UNIQUE
)

CREATE TABLE AccountsTrips(
	AccountId INT FOREIGN KEY REFERENCES Accounts(Id) NOT NULL,
	TripId INT FOREIGN KEY REFERENCES Trips(Id) NOT NULL,
	Luggage INT NOT NULL CHECK (Luggage >= 0)

	PRIMARY KEY (AccountId, TripId)
)

INSERT INTO Accounts(FirstName, MiddleName, LastName, CityId, BirthDate, Email) VALUES
('John', 'Smith', 'Smith', 34, '1975-07-21', 'j_smith@gmail.com'),
('Gosho', NULL, 'Petrov', 11, '1978-05-16', 'g_petrov@gmail.com'),
('Ivan', 'Petrovich', 'Pavlov', 59, '1849-09-26', 'i_pavlov@softuni.bg'),
('Friedrich', 'Wilhelm', 'Nietzsche', 2, '1844-10-15', 'f_nietzsche@softuni.bg')

INSERT INTO Trips (RoomId, BookDate, ArrivalDate, ReturnDate, CancelDate) VALUES
(101, '2015-04-12', '2015-04-14', '2015-04-20', '2015-02-02'),
(102, '2015-07-07', '2015-07-15', '2015-07-22', '2015-04-29'),
(103, '2013-07-17',	'2013-07-23', '2013-07-24',	NULL),
(104, '2012-03-17', '2012-03-31', '2012-04-01',	'2012-01-10'),
(109, '2017-08-07', '2017-08-28', '2017-08-29', NULL)

UPDATE Rooms
SET Price *= 1.14
WHERE HotelId IN (5, 7, 9)

DELETE FROM AccountsTrips
WHERE AccountId = 47

DELETE FROM Accounts
WHERE Id = 47

SELECT c.Id, c.[Name]
FROM Cities c
WHERE c.CountryCode = 'BG'
ORDER BY c.[Name]

SELECT CONCAT(a.FirstName, ' ' ,a.MiddleName + ' ', a.LastName) AS [Full Name],
	   YEAR(a.BirthDate) AS [BirthYear]
FROM Accounts a
WHERE YEAR(a.BirthDate) > 1991
ORDER BY [BirthYear] DESC, a.FirstName

SELECT a.FirstName, a.LastName, FORMAT(a.BirthDate, 'MM-dd-yyyy') AS [Hometown],c.[Name], a.Email
FROM Accounts a
JOIN Cities c
ON c.Id = a.CityId
WHERE a.Email LIKE 'e%'
ORDER BY c.[Name] DESC

SELECT c.[Name] AS [Town], COUNT(h.CityId) AS [Hotels]
FROM Cities c
LEFT JOIN Hotels h
ON h.CityId = c.Id
GROUP BY c.[Name]
ORDER BY [Hotels] DESC, c.[Name]