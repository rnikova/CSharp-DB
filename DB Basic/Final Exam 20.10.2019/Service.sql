create database Service

use Service

create table Users(
	Id int primary key identity(1, 1),
	Username varchar(30) not null unique,
	Password varchar(50) not null,
	Name varchar(50) not null,
	Birthdate datetime,
	Age int check(Age >= 14 and Age <= 110),
	Email varchar(50) not null
)

create table Departments(
	Id int primary key identity(1, 1),
	Name varchar(50) not null
)

create table Employees(
	Id int primary key identity(1, 1),
	FirstName varchar(25) not null,
	LastName varchar(25) not null,
	Birthdate datetime,
	Age int check(Age >= 18 and Age <= 110),
	DepartmentId int foreign key references Departments(Id)
)

create table Categories(
	Id int primary key identity(1, 1),
	Name varchar(50) not null,
	DepartmentId int foreign key references Departments(Id) not null
)

create table Status(
	Id int primary key identity(1, 1),
	Label varchar(30) not null
)

create table Reports(
	Id int primary key identity(1, 1),
	CategoryId int foreign key references Categories(Id) not null,
	StatusId int foreign key references Status(Id) not null,
	OpenDate datetime not null,
	CloseDate datetime,
	Description varchar(200) not null,
	UserId int foreign key references Users(Id) not null,
	EmployeeId int foreign key references Employees(Id)
)

insert into Employees(FirstName, LastName, Birthdate, DepartmentId) values
('Marlo',	'O''Malley', '1958-9-21',	1),
('Niki',	'Stanaghan',	'1969-11-26',	4),
('Ayrton',	'Senna',	'1960-03-21',	9),
('Ronnie',	'Peterson',	'1944-02-14',	9),
('Giovanna',	'Amati',	'1959-07-20',	5)

insert into Reports(CategoryId, StatusId, OpenDate, CloseDate, Description, UserId, EmployeeId) values
(1,	1,	'2017-04-13', null,	'Stuck Road on Str.133', 6,	2),
(6,	3,	'2015-09-05', '2015-12-06',	'Charity trail running', 3,	5),
(14, 2,	'2015-09-07', null,	'Falling bricks on Str.58',	5,	2),
(4,	3,	'2017-07-03', '2017-07-06',	'Cut off streetlight on Str.11', 1,	1)

update Reports
set CloseDate = GETDATE()
where CloseDate is null

delete from Reports
where StatusId = 4

select r.Description, FORMAT(r.OpenDate, 'dd-MM-yyyy') as [OpenDate]
from Reports r
where r.EmployeeId is null
order by r.OpenDate, r.Description

select r.Description, c.Name
from Reports r
join Categories c
on c.Id = r.CategoryId
order by r.Description, c.Name

select top 5 c.Name as [CategoryName], count(r.Id) as [ReportsNumber] 
from Categories c
join Reports r
on r.CategoryId = c.Id
group by c.Name
order by [ReportsNumber] desc, c.Name


select u.Username, c.Name as [CategoryName]
from Users u
right join Reports r
on r.UserId = u.Id
join Categories c
on c.Id = r.CategoryId
where DATEPART(MONTH, r.OpenDate) = DATEPART(MONTH, u.Birthdate)
	and DATEPART(DAY, r.OpenDate) = DATEPART(DAY, u.Birthdate)
	order by u.Username, c.Name


select e.FirstName + ' ' + e.LastName as [FullName], COUNT(u.Id) as [UsersCount]
from Employees e
left join Reports r
on r.EmployeeId = e.Id
left join Users u
on u.Id = r.UserId
group by e.FirstName + ' ' + e.LastName
order by [UsersCount] desc, [FullName]


select ISNULL(e.FirstName + ' ' + e.LastName, 'None') as [Employee],
		ISNULL(d.Name, 'None') as [Department],
		c.Name as [Category],
		r.Description,
		FORMAT(r.OpenDate, 'dd.MM.yyyy') as [OpenDate],
		s.Label as [Status],
		u.Name as [User]
from Employees e
full join Reports r
on e.Id = r.EmployeeId
full join Categories c
on c.Id = r.CategoryId
full join Departments d
on d.Id = e.DepartmentId
right join Status s
on s.Id = r.StatusId
join Users u
on u.Id = r.UserId
order by e.FirstName desc, e.LastName desc, [Department], [Category], r.Description, [OpenDate], [Status], [User]


go

create or alter function udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
returns int
begin
	if(@StartDate is null or @StartDate = 0)
	begin
		return 0
	end

	if(@EndDate is null or @EndDate = 0)
	begin
		return 0
	end

	declare @result int = datediff(hour, @StartDate, @EndDate)

	return @result
end

go

create or alter procedure usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
as
	declare @employeeDept int = (select e.DepartmentId from Employees e where e.Id = @EmployeeId)
	declare @reportDept int = (select c.DepartmentId from Reports r 
								join  Categories c on c.Id = r.CategoryId 
								join Departments d on d.Id = c.DepartmentId
								where r.Id = @ReportId)

	if(@employeeDept <> @reportDept)
	begin
		RAISERROR ('Employee doesn''t belong to the appropriate department!', 16, 1)
	end

	update Reports 
	set EmployeeId = @EmployeeId
	where Id = @ReportId