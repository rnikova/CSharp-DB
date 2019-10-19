CREATE DATABASE Supermarket

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(30) NOT NULL
)

CREATE TABLE Items(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(30) NOT NULL,
	Price DECIMAL(15, 2) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL
)

CREATE TABLE Employees(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Phone CHAR(12) NOT NULL,
	Salary DECIMAL(15, 2) NOT NULL
)

CREATE TABLE Orders(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[DateTime] DATETIME NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL
)

CREATE TABLE OrderItems(
	OrderId INT FOREIGN KEY REFERENCES Orders(Id) NOT NULL,
	ItemId INT FOREIGN KEY REFERENCES Items(Id) NOT NULL,
	Quantity INT NOT NULL CHECK(Quantity >= 1)

	PRIMARY KEY(OrderId, ItemId)
)

CREATE TABLE Shifts(
	Id INT IDENTITY(1, 1),
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	CheckIn DATETIME NOT NULL,
	CheckOut DATETIME NOT NULL

	PRIMARY KEY(Id, EmployeeId)
)

ALTER TABLE Shifts 
ADD CONSTRAINT CHK_CheckDates CHECK(CheckIn < CheckOut)

INSERT INTO Employees (FirstName, LastName, Phone, Salary) VALUES
  ('Stoyan',	'Petrov',	'888-785-8573',	500.25),
  ('Stamat',	'Nikolov',	'789-613-1122',	999995.25),
  ('Evgeni',	'Petkov',	'645-369-9517',	1234.51),
  ('Krasimir',	'Vidolov',	'321-471-9982',	50.25)

INSERT INTO Items (Name, Price, CategoryId) VALUES
  ('Tesla battery',154.25	,8),
  ('Chess',	30.25,	8),
  ('Juice',	5.32,1),
  ('Glasses',10,	8),
  ('Bottle of water',	1,	1)

UPDATE Items
SET Price *= 1.27
WHERE CategoryId IN (1, 2, 3)

DELETE FROM OrderItems
WHERE OrderID = 48

SELECT Id,FirstName
FROM Employees
WHERE Salary > 6500
ORDER BY FirstName, Id

SELECT FirstName, LastName, COUNT(o.Id) AS [Count]
FROM Employees AS e
JOIN Orders AS o 
ON o.EmployeeId = e.Id
GROUP BY FirstName, LastName
ORDER BY [Count] DESC, FirstName

SELECT FirstName, LastName, AVG(DATEDIFF(HOUR, s.CheckIn, s.CheckOut)) AS [Work Hours]
FROM Employees AS e
JOIN Shifts AS s 
ON s.EmployeeId = e.Id
GROUP BY FirstName, LastName, e.Id
HAVING AVG(DATEDIFF(HOUR, s.CheckIn, s.CheckOut)) > 7
ORDER BY [Work Hours] DESC, e.Id

SELECT TOP (1) oi.OrderId, SUM(i.Price * oi.Quantity) AS [TotalPrice]
FROM Items i
JOIN OrderItems oi
ON oi.ItemId = i.Id
JOIN Orders o
ON o.Id = oi.OrderId
GROUP BY oi.OrderId
ORDER BY [TotalPrice] DESC

SELECT TOP (10) o.Id, MAX(i.Price) AS [ExpensivePrice], MIN(i.Price) [CheapPrice]
FROM Orders AS o
JOIN OrderItems AS oi 
ON oi.OrderId = o.Id
JOIN Items AS i 
ON i.Id = oi.ItemId
GROUP BY o.Id
ORDER BY [ExpensivePrice] DESC, o.Id ASC

SELECT DISTINCT e.Id, e.FirstName, e.LastName
FROM Employees e
RIGHT JOIN Orders o
ON o.EmployeeId = e.Id
ORDER BY e.Id

SELECT DISTINCT e.Id,  e.FirstName + ' ' + e.LastName AS [Full Name]
FROM Shifts s
JOIN Employees e
ON e.Id = s.EmployeeId
WHERE DATEDIFF(HOUR, s.CheckIn, s.CheckOut) < 4
ORDER BY e.Id

SELECT e.FirstName + ' ' + e.LastName AS [Full Name],
	   SUM(oi.Quantity * i.Price) AS [Total Price],
	   SUM(oi.Quantity) AS [Items]
FROM Employees e
JOIN Orders o
ON o.EmployeeId = e.Id
JOIN OrderItems oi
ON oi.OrderId = o.Id
JOIN Items i
ON i.Id = oi.ItemId
WHERE o.[DateTime] < '2018-06-15'
GROUP BY e.FirstName + ' ' + e.LastName
ORDER BY [Total Price] DESC, [Items]

SELECT e.FirstName + ' ' + e.LastName AS [Full Name],
	   DATENAME(WEEKDAY, s.CheckIn) AS [Day of week]
FROM Orders o
FULL JOIN Employees e
ON e.Id = o.EmployeeId
JOIN Shifts s
ON s.EmployeeId = e.Id
WHERE o.Id IS NULL AND DATEDIFF(HOUR, s.CheckIn, s.CheckOut) > 12
ORDER BY e.Id

SELECT DATEPART(DAY, o.DateTime)  AS [DayOfMonth], CAST(AVG(i.Price * oi.Quantity)  AS decimal(15, 2)) AS TotalPrice
FROM Orders AS o
JOIN OrderItems AS oi 
ON oi.OrderId = o.Id
JOIN Items AS i 
ON i.Id = oi.ItemId
GROUP BY DATEPART(DAY, o.DateTime)
ORDER BY DayOfMonth ASC

