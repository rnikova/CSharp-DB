create database Bitbucket

create table Users(
	Id int primary key identity(1, 1),
	Username nvarchar(30) not null,
	[Password] nvarchar(30) not null,
	Email nvarchar(50) not null
)

create table Repositories(
	Id int primary key identity(1, 1),
	[Name] nvarchar(50) not null
)

create table RepositoriesContributors(
	RepositoryId int foreign key references Repositories(Id) not null,
	ContributorId int foreign key references Users(Id) not null,

	primary key(RepositoryId, ContributorId)
)

CREATE TABLE Issues(
	Id int primary key identity(1, 1),
	Title nvarchar(255) not null,
	IssueStatus char(6) not null,
	RepositoryId int foreign key references Repositories(Id) not null,
	AssigneeId int foreign key references Users(Id) not null
)

CREATE TABLE Commits(
	Id int primary key identity(1, 1),
	[Message] nvarchar(255) not null,
	IssueId int foreign key references Issues(Id),
	RepositoryId int foreign key references Repositories(Id) not null,
	ContributorId int foreign key references Users(Id) not null
)

CREATE TABLE Files(
	Id int primary key identity(1, 1),
	[Name] nvarchar(100) not null,
	Size decimal(15, 2) not null,
	ParentId int foreign key references Files(Id),
	CommitId int foreign key references Commits(Id) not null
)

insert into Files([Name], Size, ParentId, CommitId) values
('Trade.idk', 2598.0, 1, 1),
('menu.net', 9238.31, 2, 2),
('Administrate.soshy', 1246.93, 3, 3),
('Controller.php', 7353.15, 4, 4),
('Find.java', 9957.86, 5, 5),
('Controller.json', 14034.87, 3, 6),
('Operate.xix', 7662.92, 7, 7)

insert into Issues(Title, IssueStatus, RepositoryId, AssigneeId) values
('Critical Problem with HomeController.cs file', 'open', 1, 4),
('Typo fix in Judge.html', 'open', 4, 3),
('Implement documentation for UsersService.cs', 'closed', 8, 2),
('Unreachable code in Index.cs', 'open', 9, 8)

update Issues
set IssueStatus = 'closed'
where AssigneeId = 6

delete from RepositoriesContributors 
where RepositoryId = 3

delete from Issues 
where RepositoryId = 3

select c.Id, c.Message, c.RepositoryId, c.ContributorId
from Commits c
order by c.Id, c.Message, c.RepositoryId, c.ContributorId

select f.Id, f.Name, f.Size 
from Files f
where f.Size > 1000 and f.Name like '%html%'
order by f.Size desc, f.Id, f.Name

select i.Id, u.Username + ' : ' + i.Title as [IssueAssignee]
from Issues i
join Users u on i.AssigneeId = u.Id
order by i.Id desc, i.AssigneeId

select f.Id, f.Name, CAST(f.Size as nvarchar(20)) + 'KB' as [Size]
from Files f
left join Files cf
on f.Id = cf.ParentId
where cf.Id is null
order by f.Id, f.Name, f.Size

select top 5 r.Id, r.Name, count(c.Id) as [Commits]
from Repositories r
join RepositoriesContributors rc
on rc.RepositoryId = r.Id
join Commits c
on c.RepositoryId = r.Id
group by r.Id, r.Name
order by [Commits] desc, r.Id, r.Name

select u.Username, avg(f.Size) as [Size]
from Users u
join Commits c
on c.ContributorId = u.Id
join Files f
on f.CommitId = c.Id
group by u.Username
order by [Size] desc, u.Username

go

create or alter function udf_UserTotalCommits(@username nvarchar(50))
returns int
begin
	declare @count int = (select count(*) from Commits c join Users u on u.Id = c.ContributorId where u.Username = @username)

	return @count
end

go

create or alter procedure usp_FindByExtension(@extension nvarchar(100))
as
	select f.Id, f.Name, cast(f.Size as nvarchar) + 'KB' as [Size]
	from Files f
	where f.Name like ('%' + @extension)
	order by f.Id, f.Name, f.Size