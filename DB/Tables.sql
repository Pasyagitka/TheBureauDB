use TheBureau;
go

create table [User] (
	id int identity(1,1) primary key,
	[login] nvarchar(20) not null,
	[password] nchar(70) not null
)
go

--Auxiliary
create table [Status](
	id int identity(1,1) primary key,
	[status] nvarchar(12) not null--Принято в обработку, В процессе, Завершено
)
go
create table Stage(
	id int identity(1,1) primary key,
	stage nvarchar(12) not null --Черновая, Чистовая, Обе
)
go
create table Mounting(
	id int identity(1,1) primary key,
	mounting nvarchar(12) not null --Стена, Пол
)
go

--Storage
create table Tool (
	id int identity(1,1) primary key,
	[name] nvarchar(30) not null,
	stageId int not null foreign key references Stage(id)
)
go
create table Equipment (
	id nvarchar(3) primary key,
	[type] nvarchar(30) not null,
	mountingId int not null foreign key references Mounting(id)
)
go
create table Accessory ( -- Комплектующие
	id int identity(1,1) primary key,
	art nvarchar(15) null,
	equipmentId nvarchar(3) not null foreign key references Equipment(id),
	[name] nvarchar(150) not null,
	price decimal(6,2) not null default 0
)
go


--Brigades
create table Brigade (
	id int identity(1,1) primary key,
	userId int foreign key references [User](id) null,
	brigadierId int foreign key references [User](id) null,
	creationDate date not null
)
go
create table Employee (
	id int identity(1,1) primary key,
	firstname nvarchar(20) not null,
	patronymic nvarchar(20) not null,
	surname nvarchar(20) not null,
	email nvarchar(255) null,
	contactNumber nvarchar(12) null,
	brigadeId int null foreign key references Brigade(id)
)
go


--Client
create table Client (
	id int identity(1,1) primary key,
	firstname nvarchar(20) not null,
	patronymic nvarchar(20) not null,
	surname nvarchar(20) not null,
	email nvarchar(255) not null,
	contactNumber nvarchar(12) not null 
)
go
create table Address (
	id int identity(1,1) primary key,
	country nvarchar(30) default 'Belarus',
	city nvarchar(30) not null,
	street nvarchar(30) not null,
	house int not null,
	corpus nvarchar(10) null,
	flat int null
)
go


--For request
create table Request (
	id int identity(1,1) primary key,
	clientId int not null foreign key references Client(id),
	addressId int not null foreign key references Address(id),
	stageId int not null foreign key references Stage(id),
	statusId int not null foreign key references [Status](id), 
	registerDate date not null,
	mountingDate date not null,
	comment nvarchar(200) null,
	brigadeId int null foreign key references Brigade(id),
	proceeds decimal(6,2) null
)
go
create table RequestEquipment (
	id int identity(1,1) primary key,
	requestId int not null foreign key references Request(id),
	equipmentId nvarchar(3) not null foreign key references Equipment(id),
	quantity int
)
go
create table RequestTool (
	id int identity(1,1) primary key,
	requestId int not null foreign key references Request(id),
	toolId int not null foreign key references Tool(id)
)
go
create table RequestAccessory (
	id int identity(1,1) primary key,
	requestId int not null foreign key references Request(id),
	accessoryId int not null foreign key references Accessory(id),
	quantity int not null default (1)
)
go

--Sheldule
create table Schedule (
	id int identity(1,1) primary key,
	employeeId int not null foreign key references Employee(id),
	requestId int not null foreign key references Request(id),
	attend bit not null default (0)
)
go