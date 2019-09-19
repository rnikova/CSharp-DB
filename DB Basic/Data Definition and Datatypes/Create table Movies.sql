CREATE DATABASE MOVIES

USE Movies

CREATE TABLE Directors(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	DirectorName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(MAX)
)

CREATE TABLE Genres(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	GenresName NVARCHAR(30) NOT NULL,
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY(1,1),
	CategoryName NVARCHAR(30),
	Notes NVARCHAR(MAX)
)

CREATE TABLE Movies(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id) NOT NULL,
	CopyrightYear INT NOT NULL,
	[Length] DECIMAL NOT NULL,
	GenresId INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Rating DECIMAL NOT NULL,
	Notes NVARCHAR(MAX) NOT NULL 
)

INSERT INTO Directors(DirectorName, Notes) VALUES
('Pesho','My name is Pesho'),
('Mimi', 'My name is Mimi'),
('Gosho', 'My name is Gosho'),
('Stamat', NULL),
('Iva', NULL);

INSERT INTO Genres(GenresName, Notes) VALUES
('Horror', NULL),
('Comedy', 'COMEDY'),
('Action', 'ACTION'),
('Fantasy', NULL),
('Series Movie', NULL);

INSERT INTO Categories(CategoryName, Notes) VALUES
('Horror', NULL),
('Comedy', 'COMEDY'),
('Action', 'ACTION'),
('Fantasy', NULL),
('Series Movie', NULL);

INSERT INTO Movies(Title, DirectorId, CopyrightYear, [Length], GenresId, CategoryId, Rating, Notes) VALUES
('The Big Bang Theory', 2, 2010, 0.43, 2, 5, 9.2, NULL),
('Horror', 5, 2016, 2.43, 1, 1, 7.8, NULL),
('Action', 3, 2018, 2.35, 3, 3, 8.1, NULL),
('Comedy', 1, 2013, 1.55, 4, 2, 7.3, NULL),
('Fantasy', 4, 2019, 2.2, 5, 4, 6.5, NULL);