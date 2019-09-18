CREATE DATABASE PEOPLE
USE PEOPLE

CREATE TABLE PEOPLE(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	[Name] NVARCHAR(200) NOT NULL,
	[Picture] IMAGE,
	[Height] DECIMAL(10,2),
	[Weight] DECIMAL(10,2),
	[Gender] VARCHAR(1) NOT NULL CHECK([Gender] = 'f' OR [Gender] = 'm'),
	[Birthdate] DATE NOT NULL,
	[Biography] NVARCHAR(MAX)
)

INSERT INTO PEOPLE([Name], [Picture], [Height], [Weight], [Gender], [Birthdate], [Biography]) VALUES
('Pesho', NULL, 1.77, 76, 'm', '1989/03/03', 'My name is Pesho'),
('Mimi', NULL, 1.55, 46, 'f', '1984/07/03', 'My name is Mimi'),
('Gosho', NULL, 1.85, 96, 'm', '1993/03/17', 'My name is Gosho'),
('Stamat', NULL, 1.74, 86, 'm', '1985/09/03', NULL),
('Iva', NULL, 1.6, 66, 'f', '1991/05/03', NULL)