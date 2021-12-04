use master;


create database TheBureauWPF;
go
use TheBureauWPF;
go;

-- ����������
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

 -- ���� ���� ����������
create table Equipment (
	id char(2) primary key,
	type nvarchar(30) not null,
	mounting nvarchar(5) not null
)
go
create table Accessory ( -- �������������
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

-- ����
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
-- �������
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
-- ������ �� ������
create table Request (
	id int identity(1,1) primary key,
	clientId int not null foreign key references Client(id),
	addressId int not null foreign key references Address(id),
	stage int not null check (stage in (1,2,3)), --��������, ��������, ���
	status int not null check (status in (1,2,3)), --������� � ���������, � ��������, ���������
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
values ('RP', '��������', '���'),
	   ('RS', '��������', '�����'),
	   ('HP', '��������� ���������', '���'),
	   ('HS', '��������� ���������', '�����'),
	   ('VP', '������������� ���������', '���')
go
insert into Accessory (equipmentId, name, price, art) 
values ('RP', '����� ����������������� �16*2 HT, ������� (10�)', 50, '3C16020'),
	   ('RS', '����� ����������������� �16*2 HT, ������� (10�)', 50, '3C16020'),
	   ('HP', '����� ����������������� �16*2 HT, ������� (10�)', 50, '3C16020'),
	   ('HS', '����� ����������������� �16*2 HT, ������� (10�)', 50, '3C16020'),
	   ('VP', '����� ����������������� �16*2 HT, ������� (10�)', 50, '3C16020'),
	   ('RP', '����� ����������������� �20*2, ������� (6�)', 30, '3C20030'),
	   ('RS', '����� ����������������� �20*2, ������� (6�)', 30, '3C20030'),
	   ('HP', '����� ����������������� �20*2, ������� (6�)', 30, '3C20030'),
	   ('HS', '����� ����������������� �20*2, ������� (6�)', 30, '3C20030'),
	   ('VP', '����� ����������������� �20*2, ������� (6�)', 30, '3C20030'),
	   ('RP', '�����-����� 16�2-16�2, �� ������, ������� (2��)', 30, 'P701600'),
	   ('RS', '�����-����� 16�2-16�2, �� ������, ������� (2��)', 30, 'P701600'),
	   ('HP', '�����-����� 16�2-16�2, �� ������, ������� (2��)', 30, 'P701600'),
	   ('HS', '�����-����� 16�2-16�2, �� ������, ������� (2��)', 30, 'P701600'),
	   ('VP', '�����-����� 16�2-16�2, �� ������, ������� (2��)', 30, 'P701600'),
	   ('RP', '�����-����� 20�2-20�2, �� ������, ������� (2��)', 30, 'P702000'),
	   ('RS', '�����-����� 20�2-20�2, �� ������, ������� (2��)', 30, 'P702000'),
	   ('HP', '�����-����� 20�2-20�2, �� ������, ������� (2��)', 30, 'P702000'),
	   ('HS', '�����-����� 20�2-20�2, �� ������, ������� (2��)', 30, 'P702000'),
	   ('VP', '�����-����� 20�2-20�2, �� ������, ������� (2��)', 30, 'P702000'),

	   ('RP', '�����-�������� 16�2/90, �� ������, ������� (6��)', 80, 'P711600'),
	   ('RS', '�����-�������� 16�2/90, �� ������, ������� (6��)', 80, 'P711600'),
	   ('HP', '�����-�������� 16�2/90, �� ������, ������� (6��)', 80, 'P711600'),
	   ('HS', '�����-�������� 16�2/90, �� ������, ������� (6��)', 80, 'P711600'),
	   ('VP', '�����-�������� 16�2/90, �� ������, ������� (6��)', 80, 'P711600'),

	   ('RP', '�����-������� 16�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P721600'),
	   ('RS', '�����-������� 16�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P721600'),
	   ('HP', '�����-������� 16�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P721600'),
	   ('HS', '�����-������� 16�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P721600'),
	   ('VP', '�����-������� 16�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P721600'),

	   ('RP', '�����-������� 20�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P722003'),
	   ('RS', '�����-������� 20�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P722003'),
	   ('HP', '�����-������� 20�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P722003'),
	   ('HS', '�����-������� 20�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P722003'),
	   ('VP', '�����-������� 20�2-16�2-16�2, �� ������, ������� (2��)', 40, 'P722003'),

	   ('RP', '�����-������� 20�2-16�2-20�2, �� ������, ������� (2��)', 40, 'P722001'),
	   ('RS', '�����-������� 20�2-16�2-20�2, �� ������, ������� (2��)', 40, 'P722001'),
	   ('HP', '�����-������� 20�2-16�2-20�2, �� ������, ������� (2��)', 40, 'P722001'),
	   ('HS', '�����-������� 20�2-16�2-20�2, �� ������, ������� (2��)', 40, 'P722001'),
	   ('VP', '�����-������� 20�2-16�2-20�2, �� ������, ������� (2��)', 40, 'P722001'),


	   ('RP', '�����-���������� 16�2-1/2"� (�������� ������), �� ������, ������� (2��)', 30, 'P701611'),
	   ('RS', '�����-���������� 16�2-1/2"� (�������� ������), �� ������, ������� (2��)', 30, 'P701611'),
	   ('HP', '�����-���������� 16�2-1/2"� (�������� ������), �� ������, ������� (2��)', 30, 'P701611'),
	   ('HS', '�����-���������� 16�2-1/2"� (�������� ������), �� ������, ������� (2��)', 30, 'P701611'),
	   ('VP', '�����-���������� 16�2-1/2"� (�������� ������), �� ������, ������� (2��)', 30, 'P701611'),

	   ('RP', '��������� ������� ��� ������ ����, ��������� 3/4�-15 2��./�����', 22, '835915'),
	   ('RS', '��������� ������� ��� ������ ����, ��������� 3/4�-15 2��./�����', 22, '835915'),
	   ('RP', '������-������ �00�� 16�2 SkinPress new', 25, '7090LW162'),
	   ('RS', '������-������ �00�� 16�2 SkinPress new', 25, '7090LW162'),
	   ('HP', 'Inoflex ������ ������, 3/4"�1/2", DN20 (2 ��./�����), ��������', 80, 'ME46006'),
	   ('HP', '������� DN16 3/4" ���1/2" ��', 10, 'ME90652.1'),
	   ('HP', '���� ���������������� ����-TS-900V ������� ������ ����������� 1/2", ������, �������', 40, '1772867'), 
	   ('HP', '���������������� ������ ����-�S-90-� ������� ������ ����������� 1/2", �� ������', 45, '1772826'),
	   ('RS', '�-�������� ������ ��� ������� ����������� � ��������� ����������, ������� ��� ����������� �������, � ������� ������, �/� 50�� Female F1/2" 3/4"� (��)', 30, '965204-AF'),
	   ('VP', '������ ���������������� � �������������� Calypso Exact ���������, ������ Rp 1/2", DN15, Kvs=0,86�3/�, �N10, ����=120�, AMETAL, ��������', 35, '3452-02.000'),

	   ('VP', '������ ����������� �������-������������ R�dit�� ���������, ������ Rp1/2"�1/2", DN15, Kvs=1,36�3/�, �N10, ����=95�, ������, ������', 20, '0382-02.000'),
	   ('HP', '�����-���������� ������� �16�1/2�90', 18, '3AP1531216'),
	   ('HP', '�����-����� ���������� 20�2-16�2, ������', 15, 'P702001')
go
insert into Accessory (equipmentId, name, price)
values  
	    ('HP', '������ �������� ������������� �� �������/�����-25 (10�)', 10),
		('VP', '������ �������� ������������� �� �������/�����-25 (10�)', 10),
		('RP', '���� ������� L=80�� (5��)', 1),
		('RS', '���� ������� L=80�� (5��)', 1),
		('HP', '���� ������� L=80�� (5��)', 1),
		('HS', '���� ������� L=80�� (5��)', 1),
		('VP', '���� ������� L=80�� (5��)', 1),
		('RP', '����� (5��)', 5),
		('RS', '����� (5��)', 5),
		('HP', '����� (5��)', 5),
		('HS', '����� (5��)', 5),
		('VP', '����� (5��)', 5)
go



insert into Tool(name, stage) --1 �������� 2 �������� 3 ���
values 
('��������', 1), ('�������', 1), ('���������', 1), ('����������-��������', 1), ('�������� ������', 1), ('�������������� �����', 1),  ('����������', 1), ('�����', 1),
('����������', 2), ('�������� ������', 2), ('�������������� �����', 2),
('��������', 3), ('�������', 3), ('���������', 3), ('����������-��������', 3), ('�������� ������', 3), ('�������������� �����', 3),  ('����������', 3), ('�����', 3)



-- �������, ������
insert into Brigade default values

insert into Employee(firstname, patronymic, surname, email, contactNumber, brigadeId)
	values
	('����', '��������', '������', 'i@mail.by', '375115768798', 1),
	('������', '���������', '�������', 'a@mail.by', '375111736458', 1),
	('�������', '����������', '��������', 'l@mail.by', '375119875643', 1),
	('������', '���������', '�������', 's@mail.by', '375111239785', 2),
	('�����', '���������', '�������', 'd@mail.by', '375111736458', 2),
	('����', '��������', '������', 'e@mail.by', '375117896453', 2)
go

select * from Brigade join Employee on brigade.id = brigadeId

select * from request

select * from client
insert into Client values('���������', '��������', '�������', 'lizavetazinovich@gmail.com', 375445634337),
('������', '����������', '�����������', 'maxicids@yandex.by', 37525249572)
go

insert into Company default values
go

ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF
GO
