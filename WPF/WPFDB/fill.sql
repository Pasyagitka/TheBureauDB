insert into Address(city, street, house, flat) values ('Minsk', 'Sverdlova', 5, 1)
select * from Address

insert into Client(firstname, patronymic, surname, email, contactNumber)
values ('Анастасия', 'Алексеевна', 'Перкаль', 'liza_zinovich@mail.ru', 375445634336)

select * from Client

insert into Request(clientId, addressId, stage, status, mountingDate) 
values (1, 1, 3, 1, getdate())
select * from Request

insert into Request(clientId, addressId, stage, status, mountingDate)
values (1, 1, 2, 2, getdate())

insert into Request(clientId, addressId, stage, status, mountingDate, brigadeId)
values (1, 1, 1, 1, getdate(), 1)


insert into Employee(firstname, patronymic, surname, email, contactNumber)
	values
	('Иван', 'Иванович', 'Иванов', 'i@mail.by', '375115768798'),
	('Андрей', 'Андреевич', 'Андреев', 'a@mail.by', '375111736458'),
	('Алексей', 'Алексеевич', 'Алексеев', 'l@mail.by', '375119875643'),
	('Сергей', 'Сергеевич', 'Сергеев', 's@mail.by', '375111239785'),
	('Денис', 'Денисович', 'Денисов', 'd@mail.by', '375111736458'),
	('Егор', 'Егорович', 'Егоров', 'e@mail.by', '375117896453')
go

select * from Request
select * from Brigade
delete from Request 
delete from Brigade
delete from Employee

ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = OFF
GO

select * from [User];;