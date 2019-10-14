CREATE DATABASE School

USE School

CREATE TABLE Students(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	FirstName NVARCHAR(30) NOT NULL,
	MiddleName NVARCHAR(25),
	LastName NVARCHAR(30) NOT NULL,
	Age INT CHECK(Age > 5 AND Age < 100),
	[Address] NVARCHAR(50),
	Phone NCHAR(10)
)

CREATE TABLE Subjects(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(20) NOT NULL,
	Lessons INT CHECK(Lessons > 0) NOT NULL
)

CREATE TABLE StudentsSubjects(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NULL,
	Grade DECIMAL(15, 2) CHECK (Grade >= 2.00 AND Grade <= 6.00) NOT NULL
)

CREATE TABLE Exams(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	[Date] DATETIME,
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NULL
)

CREATE TABLE StudentsExams(
	StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
	ExamId INT FOREIGN KEY REFERENCES Exams(Id) NOT NULL,
	Grade DECIMAL(15, 2) CHECK (Grade >= 2.00 AND Grade <= 6.00) NOT NULL,

	PRIMARY KEY(StudentId, ExamId)
)

CREATE TABLE Teachers(
	Id INT PRIMARY KEY IDENTITY(1, 1),
	FirstName NVARCHAR(20) NOT NULL,
	LastName NVARCHAR(20) NOT NULL,
	[Address] NVARCHAR(20) NOT NULL,
	Phone NCHAR(10),
	SubjectId INT FOREIGN KEY REFERENCES Subjects(Id) NOT NULL
)

CREATE TABLE StudentsTeachers(
	StudentId INT FOREIGN KEY REFERENCES Students(Id) NOT NULL,
	TeacherId INT FOREIGN KEY REFERENCES Teachers(Id) NOT NULL,

	PRIMARY KEY(StudentId, TeacherId)
)


INSERT INTO Teachers(FirstName, LastName, [Address], Phone, SubjectId) VALUES
('Ruthanne', 'Bamb', '84948 Mesta Junction', '3105500146', 6),
('Gerrard', 'Lowin', '370 Talisman Plaza', '3324874824', 2),
('Merrile', 'Lambdin', '81 Dahle Plaza', '4373065154', 5),
('Bert', 'Ivie', '2 Gateway Circle', '4409584510', 4)

INSERT INTO Subjects([Name], Lessons) VALUES
('Geometry', 12),
('Health', 10),
('Drama', 7),
('Sports', 9)

UPDATE StudentsSubjects
SET Grade = 6.00
WHERE SubjectId IN (1, 2) AND Grade >= 5.50

DELETE FROM StudentsTeachers
WHERE TeacherId IN (SELECT Id FROM Teachers WHERE Phone LIKE '%72%')

DELETE FROM Teachers
WHERE Phone LIKE '%72%'

SELECT s.FirstName, s.LastName, s.Age
FROM Students s
WHERE s.Age >= 12
ORDER BY s.FirstName, s.LastName

SELECT s.FirstName, s.LastName, COUNT(st.TeacherId) AS [TeachersCount]
FROM Students s
JOIN StudentsTeachers st
ON st.StudentId = s.Id
GROUP BY s.FirstName, s.LastName

SELECT s.FirstName + ' ' + s.LastName AS [Full Name]
FROM Students s
LEFT JOIN StudentsExams se
ON se.StudentId = s.Id
WHERE se.StudentId IS NULL
ORDER BY [Full Name]

SELECT TOP(10) s.FirstName, s.LastName, CAST(AVG(se.Grade) AS DECIMAL(15, 2)) AS [Grade]
FROM Students s
JOIN StudentsExams se
ON se.StudentId = s.Id
GROUP BY s.FirstName, s.LastName
ORDER BY [Grade] DESC, s.FirstName, s.LastName

SELECT s.FirstName + ISNULL(' ' + MiddleName, '') + ' ' + s.LastName AS [Full Name]
FROM Students s
LEFT JOIN StudentsSubjects ss
ON ss.StudentId = s.Id
WHERE ss.SubjectId IS NULL
ORDER BY [Full Name]

SELECT s.[Name], AVG(ss.Grade) AS [Average Grade]
FROM Subjects s
JOIN StudentsSubjects ss
ON ss.SubjectId = s.Id
GROUP BY s.[Name], s.Id
ORDER BY s.Id

GO

CREATE OR ALTER FUNCTION udf_ExamGradesToUpdate(@studentId INT, @grade DECIMAL(15, 2))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @student INT = (SELECT TOP(1) s.Id 
							FROM Students s 
							WHERE s.Id = @studentId)

	IF (@student IS NULL)
	BEGIN
		RETURN 'The student with provided id does not exist in the school!'
	END

	IF (@grade > 6.00)
	BEGIN
		RETURN 'Grade cannot be above 6.00!'
	END

	DECLARE @studentName NVARCHAR(30) = (SELECT s.FirstName
											FROM Students s
											WHERE s.Id = @studentId)

	DECLARE @result INT = (SELECT COUNT(se.Grade)
							FROM StudentsExams se
							WHERE se.StudentId = @studentId
							AND se.Grade BETWEEN @grade AND @grade + 0.5)

	RETURN ('You have to update ' + CAST(@result AS NVARCHAR) + ' grades for the student ' + CAST(@studentName AS NVARCHAR))
END

GO

CREATE OR ALTER PROCEDURE usp_ExcludeFromSchool(@StudentId INT)
AS
	DECLARE @Student INT = (SELECT TOP(1) s.Id 
							FROM Students s 
							WHERE s.Id = @StudentId)

	IF (@Student IS NULL)
	BEGIN
		RAISERROR ('This school has no student with the provided id!', 16, 1)
		RETURN
	END

	DELETE FROM StudentsSubjects
	WHERE @StudentId = StudentsSubjects.StudentId

	DELETE FROM StudentsTeachers
	WHERE @StudentId = StudentsTeachers.StudentId

	DELETE FROM StudentsExams
	WHERE @StudentId = StudentsExams.StudentId

	DELETE FROM Students 
	WHERE @StudentId = Students.Id
