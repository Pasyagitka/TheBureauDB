use TheBureau;
go

--Brigade
create view RequestBrigade as 
	select * from [dbo].[Request]
go

--Client
create view RequestClient as 
	select 
		clientId, addressId, stageId, statusId, registerDate, mountingDate, comment, brigadeId 
	from [dbo].[Request]
go