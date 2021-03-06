CREATE DATABASE Bank
GO
USE Bank
GO
CREATE TABLE AccountHolders
(
Id INT NOT NULL,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
SSN CHAR(10) NOT NULL
CONSTRAINT PK_AccountHolders PRIMARY KEY (Id)
)

CREATE TABLE Accounts
(
Id INT NOT NULL,
AccountHolderId INT NOT NULL,
Balance MONEY DEFAULT 0
CONSTRAINT PK_Accounts PRIMARY KEY (Id)
CONSTRAINT FK_Accounts_AccountHolders FOREIGN KEY (AccountHolderId) REFERENCES AccountHolders(Id)
)

INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (1, 'Susan', 'Cane', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (2, 'Kim', 'Novac', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (3, 'Jimmy', 'Henderson', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (4, 'Steve', 'Stevenson', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (5, 'Bjorn', 'Sweden', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (6, 'Kiril', 'Petrov', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (7, 'Petar', 'Kirilov', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (8, 'Michka', 'Tsekova', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (9, 'Zlatina', 'Pateva', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (10, 'Monika', 'Miteva', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (11, 'Zlatko', 'Zlatyov', '1234567890');
INSERT INTO AccountHolders (Id, FirstName, LastName, SSN) VALUES (12, 'Petko', 'Petkov Junior', '1234567890');

INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (1, 1, 123.12);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (2, 3, 4354.23);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (3, 12, 6546543.23);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (4, 9, 15345.64);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (5, 11, 36521.20);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (6, 8, 5436.34);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (7, 10, 565649.20);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (8, 11, 999453.50);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (9, 1, 5349758.23);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (10, 2, 543.30);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (11, 3, 10.20);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (12, 7, 245656.23);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (13, 5, 5435.32);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (14, 4, 1.23);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (15, 6, 0.19);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (16, 2, 5345.34);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (17, 11, 76653.20);
INSERT INTO Accounts (Id, AccountHolderId, Balance) VALUES (18, 1, 235469.89);

GO

CREATE OR ALTER PROCEDURE usp_GetHoldersFullName 
AS
	SELECT a.FirstName + ' ' + a.LastName AS [Full Name]
	FROM AccountHolders a

GO

CREATE OR ALTER PROCEDURE usp_GetHoldersWithBalanceHigherThan(@amount DECIMAL(15, 2))
AS
	SELECT ah.FirstName, ah.LastName
	FROM AccountHolders ah
	JOIN Accounts a
	ON a.AccountHolderId = ah.Id
	GROUP BY ah.FirstName, ah.LastName
	HAVING SUM(a.Balance) > @amount
	ORDER BY ah.FirstName, ah.LastName

EXEC [dbo].usp_GetHoldersWithBalanceHigherThan 7000

GO
CREATE FUNCTION ufn_CalculateFutureValue(@sum DECIMAL(15, 2), @rate FLOAT, @years INT)
RETURNS DECIMAL(15, 4)
AS
BEGIN
	RETURN @sum * (POWER(1 + @rate, @years))
END

GO

CREATE PROCEDURE usp_CalculateFutureValueForAccount(@accountId INT, @rate FLOAT)
AS
BEGIN
	SELECT a.Id AS [Account Id],
	       ah.FirstName AS [First Name],
		   ah.LastName AS [Last Name],
		   a.Balance AS [Current Balance],
		   dbo.ufn_CalculateFutureValue(a.Balance, @rate, 5) AS [Balance in 5 years]
	FROM Accounts a
	JOIN AccountHolders ah 
	ON a.AccountHolderId = ah.Id
	WHERE a.Id = @accountId
END


CREATE TABLE Logs
(
	LogId INT PRIMARY KEY IDENTITY,
	AccountId INT,
	OldSum DECIMAL(15, 2),
	NewSum DECIMAL(15, 2)
)

GO

CREATE TRIGGER tr_UpdateBalance ON Accounts FOR UPDATE
AS
BEGIN
	DECLARE @newSum DECIMAL(15, 2) = (SELECT i.Balance FROM INSERTED AS i)
	DECLARE @oldSum DECIMAL(15, 2) = (SELECT d.Balance FROM DELETED AS d)
	DECLARE @accountId INT = (SELECT i.Id FROM INSERTED AS i)

	INSERT INTO Logs
	(
	    AccountId,
	    OldSum,
	    NewSum
	)
	VALUES
	(
		@accountId,
		@oldSum,
		@newSum
	)
END

CREATE TABLE NotificationEmails
(
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Recipient INT,
	[Subject] VARCHAR(500),
	Body VARCHAR(500)
)

GO

CREATE TRIGGER tr_AddNewEmail ON Logs FOR INSERT
AS
BEGIN
	DECLARE @recipient INT = 
		(
			SELECT i.AccountId
			FROM inserted i
		)
	DECLARE @oldSum DECIMAL(15, 2) = 
		(
			SELECT i.OldSum
			FROM inserted i
		)
	DECLARE @newSum DECIMAL(15, 2) = 
		(
			SELECT i.NewSum
			FROM inserted i
		)

	INSERT INTO NotificationEmails(Recipient, [Subject], Body) VALUES
	(	
		@recipient,
		'Balance change for account: ' + CAST(@recipient AS VARCHAR(15)),
	    'On ' + CAST(GETDATE() AS VARCHAR(50)) + ' your balance was changed from ' +
	    CAST(@oldSum AS VARCHAR(30)) + ' to ' +
	    CAST(@newSum AS VARCHAR(50)) + '.'
	)
END

GO

CREATE OR ALTER PROCEDURE usp_DepositMoney(@accountId INT, @moneyAmount DECIMAL(15, 4))
AS
BEGIN
	IF(@moneyAmount > 0)
	BEGIN
		UPDATE Accounts
		SET Balance += @moneyAmount
		WHERE Accounts.Id = @accountId
	END
END

GO

CREATE OR ALTER PROCEDURE usp_WithdrawMoney (@accountId INT, @moneyAmount DECIMAL(15, 4))
AS
BEGIN
	IF(@moneyAmount > 0)
	BEGIN
		UPDATE Accounts
		SET Balance -= @moneyAmount
		WHERE Accounts.Id = @accountId
	END
END

GO

CREATE OR ALTER PROCEDURE usp_TransferMoney(@senderId INT, @receiverId INT, @amount DECIMAL(15, 4))
AS
BEGIN
	IF(@amount > 0)
	BEGIN
		EXEC [dbo].usp_DepositMoney @receiverId, @amount
		EXEC [dbo].usp_WithdrawMoney @senderId, @amount
	END
END