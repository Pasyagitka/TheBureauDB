use TheBureau;
go

--Brigade
create view RequestBrigade as 
	select * from [dbo].[Request]
go

--Client 
--proceeds???
create or alter view RequestForClientView
		with schemabinding
		as  
		select [dbo].[Request].id 'requestId', [dbo].[Request].mountingDate, [dbo].[Request].comment, [dbo].[Request].stageId, [dbo].[Request].statusId,
		[dbo].[Request].brigadeId, [dbo].[Request].clientId, [dbo].[Request].addressId,
		[dbo].[Address].country, [dbo].[Address].city, [dbo].[Address].street, [dbo].[Address].house, [dbo].[Address].corpus, [dbo].[Address].flat,
		[dbo].[Client].firstname, [dbo].[Client].surname, [dbo].[Client].patronymic, [dbo].[Client].email,  [dbo].[Client].contactNumber
		from [dbo].[Request]
		join [dbo].[Client] on  [dbo].[Request].clientId = [dbo].[Client].id
		join [dbo].[Address] on [dbo].[Request].addressId = [dbo].[Address].id
go

--select * from RequestForClientView where email = 'liza@gmail.ru'


create or alter view BrigadeView
as 
	select [dbo].[Brigade].id, [dbo].[Brigade].userId, [dbo].[Brigade].brigadierId,
	[dbo].[Employee].firstname, [dbo].[Employee].surname, [dbo].[Employee].patronymic, [dbo].[Employee].email,  [dbo].[Employee].contactNumber
	from [dbo].[Brigade]
	left join [dbo].[Employee] on [dbo].[Employee].brigadeId = [dbo].[Brigade].id
go

select * from BrigadeView