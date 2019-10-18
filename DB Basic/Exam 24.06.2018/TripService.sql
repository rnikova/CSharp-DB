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

SELECT r.Id, r.Price, h.[Name], c.[Name]
FROM Rooms r
JOIN Hotels h
ON h.Id = r.HotelId
JOIN Cities c
ON c.Id = h.CityId
WHERE r.[Type] = 'First Class'
ORDER BY r.Price DESC, r.Id

SELECT at.AccountId, 
	CONCAT(a.FirstName, ' ', a.LastName) AS [FullName],
	MAX(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate)) AS [LongestTrip],
	MIN(DATEDIFF(DAY, t.ArrivalDate, t.ReturnDate)) AS [ShortestTrip]
FROM AccountsTrips at
JOIN Accounts a
ON a.Id = at.AccountId
JOIN Trips t
ON t.Id = at.TripId
WHERE a.MiddleName IS NULL AND t.CancelDate IS NULL
GROUP BY at.AccountId, CONCAT(a.FirstName, ' ', a.LastName), a.Id
ORDER BY [LongestTrip] DESC, a.Id

SELECT TOP(5) c.Id, c.[Name], c.CountryCode AS [Country], COUNT(a.Id) AS [Accounts]
FROM Accounts a
JOIN Cities c
ON c.Id = a.CityId
GROUP BY c.Id, c.[Name], c.CountryCode
ORDER BY [Accounts] DESC

SELECT a.Id, a.Email, c.[Name] AS [City], COUNT(*) AS [Trips]
FROM Accounts a
JOIN AccountsTrips at 
ON a.Id = at.AccountId
JOIN Trips t 
ON at.TripId = t.Id
JOIN Rooms r 
ON t.RoomId = r.Id
JOIN Hotels h 
ON r.HotelId = h.Id
JOIN Cities c 
ON h.CityId = c.Id
WHERE a.CityId = h.CityId
GROUP BY a.Id, a.Email, a.CityId, c.[Name]
ORDER BY Trips DESC, a.Id

SELECT TOP(10) c.Id, c.[Name], SUM(h.BaseRate + r.Price) AS [Total Revenue], COUNT(*)  AS [Trips]
FROM Hotels h
JOIN Rooms r
ON r.HotelId = h.Id
JOIN Cities c
ON c.Id = h.CityId
JOIN Trips t
ON t.RoomId = r.Id
WHERE YEAR(t.BookDate) = 2016
GROUP BY c.Id, c.[Name]
ORDER BY [Total Revenue] DESC, [Trips] DESC


SELECT at.TripId, 
	   h.[Name] AS [HotelName], 
	   r.[Type] AS [RoomType], 
	   CASE WHEN t.CancelDate IS NULL THEN SUM(h.BaseRate + r.Price)
	   ELSE 0 END AS [Revenue]
FROM Hotels h
JOIN Rooms r
ON r.HotelId = h.Id
JOIN Trips t
ON t.RoomId = r.Id
JOIN AccountsTrips at
ON at.TripId = t.Id
GROUP BY at.TripId, h.[Name], r.[Type], t.CancelDate
ORDER BY r.[Type], at.TripId


SELECT AccountId, Email, CountryCode, Trips
FROM (SELECT
        a.Id AS AccountId,
        a.Email,
        c.CountryCode,
        COUNT(*) AS Trips,
        DENSE_RANK() OVER (PARTITION BY c.CountryCode ORDER BY COUNT(*) DESC, a.Id ) AS Rank
      FROM Accounts A
        JOIN AccountsTrips at 
		ON a.Id = at.AccountId
        JOIN Trips t 
		ON at.TripId = t.Id
        JOIN Rooms r 
		ON t.RoomId = r.Id
        JOIN Hotels h 
		ON r.HotelId = h.Id
        JOIN Cities c 
		ON h.CityId = c.Id
        GROUP BY c.CountryCode, a.Email, a.Id) AS RanksPerCountry
WHERE Rank = 1
ORDER BY Trips DESC, AccountId


SELECT TripId, 
	   SUM(Luggage) AS Luggage, 
	   '$' + CONVERT(VARCHAR(10), SUM(Luggage) * CASE WHEN SUM(Luggage) > 5 THEN 5 ELSE 0 END) AS Fee
FROM Trips
JOIN AccountsTrips AT on Trips.Id = AT.TripId
GROUP BY TripId
HAVING SUM(Luggage) > 0
ORDER BY Luggage DESC


SELECT
  t.Id,
  CONCAT(a.FirstName, ' ' + a.MiddleName, ' ', a.LastName) AS [Full Name],
  ac.Name AS [From],
  hc.Name AS [To],
  CASE WHEN CancelDate IS NOT NULL THEN 'Canceled'
  ELSE CONCAT(DATEDIFF(DAY, T.ArrivalDate, T.ReturnDate), ' days')
  END  AS Duration
FROM Trips AS t
JOIN AccountsTrips at 
ON t.Id = at.TripId
JOIN Accounts a 
ON at.AccountId = a.Id
JOIN Rooms r 
ON t.RoomId = r.Id
JOIN Hotels h 
ON r.HotelId = h.Id
JOIN Cities hc 
ON h.CityId = hc.Id
JOIN Cities ac 
ON a.CityId = ac.Id
ORDER BY [Full Name], T.Id

SELECT * FROM Rooms
GO
CREATE OR ALTER FUNCTION udf_GetAvailableRoom(@HotelId INT, @Date DATE, @People INT)
RETURNS NVARCHAR(MAX)
BEGIN
	DECLARE @BookedRooms TABLE(Id INT)

    INSERT INTO @BookedRooms
      SELECT DISTINCT R.Id
      FROM Rooms R
        LEFT JOIN Trips T on R.Id = T.RoomId
      WHERE R.HotelId = @HotelId AND @Date BETWEEN T.ArrivalDate AND T.ReturnDate AND T.CancelDate IS NULL

    DECLARE @Rooms TABLE(Id INT, Price DECIMAL(15, 2), [Type] NVARCHAR(20), Beds INT, TotalPrice DECIMAL(15, 2))
    INSERT INTO @Rooms
      SELECT TOP (1)
        r.Id,
        r.Price,
        r.[Type],
        r.Beds,
        @People * (h.BaseRate + r.Price) AS TotalPrice
      FROM Rooms r
        LEFT JOIN Hotels h ON r.HotelId = h.Id
      WHERE
        r.HotelId = @HotelId AND
        r.Beds >= @People AND
        r.Id NOT IN (SELECT Id
                     FROM @BookedRooms)
      ORDER BY TotalPrice DESC

    DECLARE @RoomCount INT = (SELECT COUNT(*) FROM @Rooms)

    IF (@RoomCount < 1)
      BEGIN
        RETURN 'No rooms available'
      END

    DECLARE @Result VARCHAR(MAX) = (SELECT CONCAT('Room ', Id, ': ', Type, ' (', Beds, ' beds) - ', '$', TotalPrice)
                                    FROM @Rooms)

    RETURN @Result
END

GO

CREATE OR ALTER PROC usp_SwitchRoom(@TripId INT, @TargetRoomId INT)
AS
BEGIN
    DECLARE @CurrentHotelId INT = (SELECT h.Id
                                   FROM Hotels h
                                   JOIN Rooms r ON h.Id = r.HotelId
                                   JOIN Trips t ON r.Id = t.RoomId
                                  WHERE t.Id = @TripId)

    DECLARE @TargetHotelId INT = (SELECT h.Id
                                  FROM Hotels h
                                  JOIN Rooms r ON h.Id = r.HotelId
                                  WHERE r.Id = @TargetRoomId)

    IF (@CurrentHotelId <> @TargetHotelId)
      THROW 50013, 'Target room is in another hotel!', 1

    DECLARE @PeopleCount INT = (SELECT COUNT(*)
                                FROM AccountsTrips
                                WHERE TripId = @TripId)

    DECLARE @TargetRoomBeds INT = (SELECT Beds
                                   FROM Rooms
                                   WHERE Id = @TargetRoomId)

    IF (@PeopleCount > @TargetRoomBeds)
      THROW 50013, 'Not enough beds in target room!', 1

    UPDATE Trips
    SET RoomId = @TargetRoomId
    WHERE Id = @TripId
END

GO

CREATE TRIGGER T_CancelTrip
ON Trips
INSTEAD OF DELETE
AS
BEGIN
      UPDATE Trips
      SET CancelDate = GETDATE()
      WHERE Id IN (SELECT Id
                   FROM deleted
                   WHERE CancelDate IS NULL)
END