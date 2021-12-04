use master;


create database TheBureauWPF;
go
use TheBureauWPF;
go;

-- Приложение
create table Company (
	email nvarchar(255) null default ('thebureaunotificationcenter@gmail.com'),
	password nchar(70) null default ('thebureau')
)
go
create table [Role] (
	id int identity(1,1) primary key,
	role int not null unique,
	name nvarchar(10) not null unique
)
go
create table [User] (
	id int identity(1,1) primary key,
	login nvarchar(20) not null,
	password nchar(70) not null,
	role int not null foreign key references Role(id)
)
go

 -- База Бюро Монтажника
create table Equipment (
	id char(2) primary key,
	type nvarchar(30) not null,
	mounting nvarchar(5) not null
)
go
create table Accessory ( -- Комплектующие
	id int identity(1,1) primary key,
	art nvarchar(15) null,
	equipmentId char(2) not null foreign key references Equipment(id),
	name nvarchar(150) not null,
	price decimal(6,2) not null default 0
)
go
create table Tool (
	id int identity(1,1) primary key,
	name nvarchar(30) not null,
	stage int not null check (stage in (1,2,3))
)
go

-- Люди
create table Client (
	id int identity(1,1) primary key,
	firstname nvarchar(20) not null,
	patronymic nvarchar(20) not null,
	surname nvarchar(20) not null,
	email nvarchar(255) not null,
	contactNumber decimal(12,0) not null 
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
-- Бригады
create table Brigade (
	id int identity(1,1) primary key,
	userId int foreign key references  [User](id)
)
go
create table Employee (
	id int identity(1,1) primary key,
	firstname nvarchar(20) not null,
	patronymic nvarchar(20) not null,
	surname nvarchar(20) not null,
	email nvarchar(255) null,
	contactNumber decimal(12,0) null,
	brigadeId int not null foreign key references Brigade(id)
)
go
-- Заявка на монтаж
create table Request (
	id int identity(1,1) primary key,
	clientId int not null foreign key references Client(id),
	addressId int not null foreign key references Address(id),
	stage int not null check (stage in (1,2,3)), --Черновая, Чистовая, Обе
	status int not null check (status in (1,2,3)), --Принято в обработку, В процессе, Завершено
	mountingDate date not null,
	comment nvarchar(200) null,
	brigadeId int null foreign key references Brigade(id)
)
go
create table RequestEquipment (
	id int identity(1,1) primary key,
	requestId int not null foreign key references Request(id),
	equipmentId char(2) not null foreign key references Equipment(id),
	quantity int
)
go



insert into Role(role, name)
values (0, 'admin'),
	   (1, 'brigade'),
	   (2, 'client')
go
select * from role
insert into [User](login, password, role)
values ('admin', 'admin', 1),
 ('brigade1', '1111', 2),
 ('brigade2', '2222', 2)
go
select * from  [User]


select * from Accessory
select * from Equipment

insert into Equipment(id, type, mounting) 
values ('RP', 'радиатор', 'пол'),
	   ('RS', 'радиатор', 'стена'),
	   ('HP', 'напольный конвектор', 'пол'),
	   ('HS', 'напольный конвектор', 'стена'),
	   ('VP', 'внутрипольный конвектор', 'пол')
go
insert into Accessory (equipmentId, name, price, art) 
values ('RP', 'Труба металлополимерная ф16*2 HT, Австрия (10м)', 50, '3C16020'),
	   ('RS', 'Труба металлополимерная ф16*2 HT, Австрия (10м)', 50, '3C16020'),
	   ('HP', 'Труба металлополимерная ф16*2 HT, Австрия (10м)', 50, '3C16020'),
	   ('HS', 'Труба металлополимерная ф16*2 HT, Австрия (10м)', 50, '3C16020'),
	   ('VP', 'Труба металлополимерная ф16*2 HT, Австрия (10м)', 50, '3C16020'),
	   ('RP', 'Труба металлополимерная ф20*2, Австрия (6м)', 30, '3C20030'),
	   ('RS', 'Труба металлополимерная ф20*2, Австрия (6м)', 30, '3C20030'),
	   ('HP', 'Труба металлополимерная ф20*2, Австрия (6м)', 30, '3C20030'),
	   ('HS', 'Труба металлополимерная ф20*2, Австрия (6м)', 30, '3C20030'),
	   ('VP', 'Труба металлополимерная ф20*2, Австрия (6м)', 30, '3C20030'),
	   ('RP', 'Пресс-муфта 16х2-16х2, из латуни, Австрия (2шт)', 30, 'P701600'),
	   ('RS', 'Пресс-муфта 16х2-16х2, из латуни, Австрия (2шт)', 30, 'P701600'),
	   ('HP', 'Пресс-муфта 16х2-16х2, из латуни, Австрия (2шт)', 30, 'P701600'),
	   ('HS', 'Пресс-муфта 16х2-16х2, из латуни, Австрия (2шт)', 30, 'P701600'),
	   ('VP', 'Пресс-муфта 16х2-16х2, из латуни, Австрия (2шт)', 30, 'P701600'),
	   ('RP', 'Пресс-муфта 20х2-20х2, из латуни, Австрия (2шт)', 30, 'P702000'),
	   ('RS', 'Пресс-муфта 20х2-20х2, из латуни, Австрия (2шт)', 30, 'P702000'),
	   ('HP', 'Пресс-муфта 20х2-20х2, из латуни, Австрия (2шт)', 30, 'P702000'),
	   ('HS', 'Пресс-муфта 20х2-20х2, из латуни, Австрия (2шт)', 30, 'P702000'),
	   ('VP', 'Пресс-муфта 20х2-20х2, из латуни, Австрия (2шт)', 30, 'P702000'),

	   ('RP', 'Пресс-угольник 16х2/90, из латуни, Австрия (6шт)', 80, 'P711600'),
	   ('RS', 'Пресс-угольник 16х2/90, из латуни, Австрия (6шт)', 80, 'P711600'),
	   ('HP', 'Пресс-угольник 16х2/90, из латуни, Австрия (6шт)', 80, 'P711600'),
	   ('HS', 'Пресс-угольник 16х2/90, из латуни, Австрия (6шт)', 80, 'P711600'),
	   ('VP', 'Пресс-угольник 16х2/90, из латуни, Австрия (6шт)', 80, 'P711600'),

	   ('RP', 'Пресс-тройник 16х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P721600'),
	   ('RS', 'Пресс-тройник 16х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P721600'),
	   ('HP', 'Пресс-тройник 16х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P721600'),
	   ('HS', 'Пресс-тройник 16х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P721600'),
	   ('VP', 'Пресс-тройник 16х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P721600'),

	   ('RP', 'Пресс-тройник 20х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P722003'),
	   ('RS', 'Пресс-тройник 20х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P722003'),
	   ('HP', 'Пресс-тройник 20х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P722003'),
	   ('HS', 'Пресс-тройник 20х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P722003'),
	   ('VP', 'Пресс-тройник 20х2-16х2-16х2, из латуни, Австрия (2шт)', 40, 'P722003'),

	   ('RP', 'Пресс-тройник 20х2-16х2-20х2, из латуни, Австрия (2шт)', 40, 'P722001'),
	   ('RS', 'Пресс-тройник 20х2-16х2-20х2, из латуни, Австрия (2шт)', 40, 'P722001'),
	   ('HP', 'Пресс-тройник 20х2-16х2-20х2, из латуни, Австрия (2шт)', 40, 'P722001'),
	   ('HS', 'Пресс-тройник 20х2-16х2-20х2, из латуни, Австрия (2шт)', 40, 'P722001'),
	   ('VP', 'Пресс-тройник 20х2-16х2-20х2, из латуни, Австрия (2шт)', 40, 'P722001'),


	   ('RP', 'Пресс-переходник 16х2-1/2"н (наружная резьба), из латуни, Австрия (2шт)', 30, 'P701611'),
	   ('RS', 'Пресс-переходник 16х2-1/2"н (наружная резьба), из латуни, Австрия (2шт)', 30, 'P701611'),
	   ('HP', 'Пресс-переходник 16х2-1/2"н (наружная резьба), из латуни, Австрия (2шт)', 30, 'P701611'),
	   ('HS', 'Пресс-переходник 16х2-1/2"н (наружная резьба), из латуни, Австрия (2шт)', 30, 'P701611'),
	   ('VP', 'Пресс-переходник 16х2-1/2"н (наружная резьба), из латуни, Австрия (2шт)', 30, 'P701611'),

	   ('RP', 'Резьбовой адаптер для медных труб, Евроконус 3/4Е-15 2шт./компл', 22, '835915'),
	   ('RS', 'Резьбовой адаптер для медных труб, Евроконус 3/4Е-15 2шт./компл', 22, '835915'),
	   ('RP', 'Трубка-Уголок З00мм 16х2 SkinPress new', 25, '7090LW162'),
	   ('RS', 'Трубка-Уголок З00мм 16х2 SkinPress new', 25, '7090LW162'),
	   ('HP', 'Inoflex гибкая трубка, 3/4"х1/2", DN20 (2 шт./компл), Германия', 80, 'ME46006'),
	   ('HP', 'Футорка DN16 3/4" НРх1/2" ВР', 10, 'ME90652.1'),
	   ('HP', 'Клан термостатический ГЕРЦ-TS-900V угловой осевой специальный 1/2", латунь, Австрия', 40, '1772867'), 
	   ('HP', 'Термостатический клапан ГЕРЦ-ТS-90-Н угловой осевой специальный 1/2", из латуни', 45, '1772826'),
	   ('RS', 'Н-образный модуль для нижнего подключения к панельным радиаторам, угловой для двухтрубной системы, с шаровым краном, с/с 50мм Female F1/2" 3/4"Е (ФР)', 30, '965204-AF'),
	   ('VP', 'Клапан термостатический с преднастройкой Calypso Exact проходной, резьба Rp 1/2", DN15, Kvs=0,86м3/ч, РN10, Тмах=120С, AMETAL, Германия', 35, '3452-02.000'),

	   ('VP', 'Клапан радиаторный запорно-регулирующий Rаditес проходной, резьба Rp1/2"х1/2", DN15, Kvs=1,36м3/ч, РN10, Тмах=95С, латунь, Италия', 20, '0382-02.000'),
	   ('HP', 'Пресс-переходник угловой ф16х1/2х90', 18, '3AP1531216'),
	   ('HP', 'Пресс-муфта переходная 20х2-16х2, латунь', 15, 'P702001')
go
insert into Accessory (equipmentId, name, price)
values  
	    ('HP', 'Трубка защитная гофрированная ПЭ красная/синяя-25 (10м)', 10),
		('VP', 'Трубка защитная гофрированная ПЭ красная/синяя-25 (10м)', 10),
		('RP', 'Крюк двойной L=80мм (5шт)', 1),
		('RS', 'Крюк двойной L=80мм (5шт)', 1),
		('HP', 'Крюк двойной L=80мм (5шт)', 1),
		('HS', 'Крюк двойной L=80мм (5шт)', 1),
		('VP', 'Крюк двойной L=80мм (5шт)', 1),
		('RP', 'Мешки (5шт)', 5),
		('RS', 'Мешки (5шт)', 5),
		('HP', 'Мешки (5шт)', 5),
		('HS', 'Мешки (5шт)', 5),
		('VP', 'Мешки (5шт)', 5)
go



insert into Tool(name, stage) --1 черновая 2 чистовая 3 обе
values 
('болгарка', 1), ('пылесос', 1), ('штроборез', 1), ('перфоратор-отбойник', 1), ('комплект ключей', 1), ('гидравлический пресс', 1),  ('калибратор', 1), ('клещи', 1),
('перфоратор', 2), ('комплект ключей', 2), ('гидравлический пресс', 2),
('болгарка', 3), ('пылесос', 3), ('штроборез', 3), ('перфоратор-отбойник', 3), ('комплект ключей', 3), ('гидравлический пресс', 3),  ('калибратор', 3), ('клещи', 3)



-- Бригады, Емплои
insert into Brigade default values

insert into Employee(firstname, patronymic, surname, email, contactNumber, brigadeId)
	values
	('Иван', 'Иванович', 'Иванов', 'i@mail.by', '375115768798', 1),
	('Андрей', 'Андреевич', 'Андреев', 'a@mail.by', '375111736458', 1),
	('Алексей', 'Алексеевич', 'Алексеев', 'l@mail.by', '375119875643', 1),
	('Сергей', 'Сергеевич', 'Сергеев', 's@mail.by', '375111239785', 2),
	('Денис', 'Денисович', 'Денисов', 'd@mail.by', '375111736458', 2),
	('Егор', 'Егорович', 'Егоров', 'e@mail.by', '375117896453', 2)
go

select * from Brigade join Employee on brigade.id = brigadeId

select * from request

select * from client
insert into Client values('Елизавета', 'Игоревна', 'Зинович', 'lizavetazinovich@gmail.com', 375445634337),
('Максим', 'Витальевич', 'Малиновский', 'maxicids@yandex.by', 37525249572)
go

insert into Company default values
go

ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF
GO
