CREATE TABLE Directors(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	DirectorName NVARCHAR(MAX) NOT NULL,
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Genres(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	GenresName NVARCHAR(MAX) NOT NULL,
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Categories(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	CategoryName NVARCHAR(MAX) NOT NULL,
	Notes NVARCHAR(MAX) 
)

CREATE TABLE Movies(
	Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	Title NVARCHAR(MAX) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id) NOT NULL,
	CopyrightYear INT NOT NULL,
	[Length] INT NOT NULL,
	GenresId INT FOREIGN KEY REFERENCES Genres(Id) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	Raiting INT NOT NULL,
	Notes NVARCHAR(MAX) 
)

INSERT INTO Directors(DirectorName, Notes) VALUES
('Pesho','My name is Pesho'),
('Mimi', 'My name is Mimi'),
('Gosho', 'My name is Gosho'),
('Stamat', NULL),
('Iva', NULL)

INSERT INTO Genres(GenresName, Notes) VALUES
('Horror', NULL),
('Comedy', 'COMEDY'),
('Action', 'ACTION'),
('Fantasy', NULL),
('Series Movie', NULL)

INSERT INTO Categories(CategoryName, Notes) VALUES
('Horror', NULL),
('Comedy', 'COMEDY'),
('Action', 'ACTION'),
('Fantasy', NULL),
('Series Movie', NULL)

INSERT INTO Movies(Title, DirectorId, CopyrightYear, [Length], GenresId, CategoryId, Raiting, Notes) VALUES
('The Big Bang Theory', 2, 2010, 4, 2, 5, 9, NULL),
('Horror', 5, '2016', 2.43, 1, 1, 7, NULL),
('Action', 3, '2018', 2.35, 3, 3, 8, NULL),
('Comedy', 1, '2013', 1.55, 4, 2, 7, NULL),
('Fantasy', 4, '2019', 2.2, 5, 4, 6, NULL)