CREATE DATABASE USERS
USE USERS

CREATE TABLE Users(
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL,
	Password VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY(MAX),
	CHECK (DATALENGTH(ProfilePicture) <= 921600),
	LastLoginTime DATETIME2,
	IsDeleted BIT
)

INSERT INTO Users(Username, Password, ProfilePicture, LastLoginTime, IsDeleted) VALUES
('Pesho', '1234', NULL, NULL, 0),
('Mimi','12345', NULL, NULL, 1),
('Gosho','123', NULL, NULL, 0),
('Stamat','1234', NULL, NULL, 1),
('Iva', '123', NULL, NULL, 0)