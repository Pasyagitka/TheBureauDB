use TheBureau;
go

--Mounting----------------------------------------------------------------------------------
create or alter procedure GetAllMountings
as begin
	begin try
		select id, mounting from [dbo].[Mounting]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Status------------------------------------------------------------------------------------
create or alter procedure GetStatusIdByName(@status nvarchar(12))
as begin
	begin try
		select id from [dbo].[Status] where [status] = @status;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Stage--------------------------------------------------------------------------------------
create or alter procedure GetAllStages
as begin
	begin try
		select id, stage from [dbo].[Stage]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetStageIdByName(@stage nvarchar(12))
as begin
	begin try
		select id from [dbo].[Stage] where stage = @stage;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Client--------------------------------------------------------------------------------------
create or alter procedure GetAllClients
as begin
	begin try
		select id, firstname, patronymic, surname, email, contactNumber from [dbo].[ClientView]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetClient(@id int)
as begin
	begin try
		select id, firstname, patronymic, surname, email, contactNumber from [dbo].[ClientView] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Accessory--------------------------------------------------------------------------------------
create or alter procedure GetAccessory(@id int)
as begin
	begin try
		select id, art, equipmentId, [name], price from [dbo].[AccessoryView] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetAllAccessories
as begin
	begin try
		select id, art, equipmentId, [name], price from [dbo].[AccessoryView]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Equipment--------------------------------------------------------------------------------------
create or alter procedure GetAllEquipment
as begin
	begin try
		select id, [type], mountingId from [dbo].[EquipmentView]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetEquipment(@id nvarchar(3))
as begin
	begin try
		select id, [type], mountingId from [dbo].[EquipmentView] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Brigade--------------------------------------------------------------------------------------
create or alter procedure GetAllBrigades
as begin
	begin try
		select id, userId, brigadierId, creationDate from [dbo].[BrigadeView]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetBrigade(@id int)
as begin
	begin try
		select id, userId, brigadierId, creationDate from [dbo].[BrigadeView] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetBrigadeByUserId(@userId int) 
as begin 
	begin try
		select id, userId, brigadierId, firstname, surname, patronymic, email, contactNumber from [dbo].[ForBrigadeView] where userId = @userId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Employee--------------------------------------------------------------------------------------
create or alter procedure GetAllEmployees
as begin
	begin try
		select id, firstname, patronymic, surname, email, contactNumber, brigadeId from [dbo].[EmployeeView]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetEmployeesForBrigade(@brigadeId int)
as begin
	begin try
		select id, firstname, patronymic, surname, email, contactNumber, brigadeId from [dbo].[EmployeeView] where brigadeId = @brigadeId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetEmployee(@id int)
as begin
	begin try
		select id, firstname, patronymic, surname, email, contactNumber, brigadeId from [dbo].[EmployeeView] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
		if error_procedure() is not null print 'in procedure : ' + error_procedure();
	end catch
end;
go




--Request--------------------------------------------------------------------------------------
create or alter procedure GetAllRequests
as begin
	begin try
		select id, clientId, addressId, stageId, statusId, registerDate, mountingDate, comment, brigadeId, proceeds from [dbo].[RequestView]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetRequest (@id int)
as begin
	begin try
		select id, clientId, addressId, stageId, statusId, registerDate, mountingDate, comment, brigadeId, proceeds from [dbo].[RequestView] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetUpdatedRequest(@id int) --Get updated request data (status, brigadeId) by id
as begin
	begin try
		select [statusId], brigadeId from [dbo].[RequestView] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetAllRequestsForRequestView
as begin
	begin try
		select [dbo].[RequestView].id 'id', clientId, addressId, stageId, statusId, registerDate, mountingDate, comment, brigadeId, proceeds,
				firstname, patronymic, surname, email, contactNumber,
				country, city, street, house, corpus, flat
		from [dbo].[RequestView] 
		join [dbo].[ClientView] on [dbo].[RequestView].clientId = [dbo].[ClientView].id 
		join [dbo].[AddressView] on [dbo].[RequestView].addressId = [dbo].[AddressView].id
		order by [dbo].[RequestView].id desc;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetAllRequestsForBrigade(@brigadeId int)
as begin
	begin try
		select [dbo].[RequestView].id 'id', clientId, addressId, stageId, statusId, registerDate, mountingDate, comment, brigadeId, proceeds,
				firstname, patronymic, surname, email, contactNumber,
				country, city, street, house, corpus, flat 
		from [dbo].[RequestView] 
		join [dbo].[ClientView] on [dbo].[RequestView].clientId = [dbo].[ClientView].id 
		join [dbo].[AddressView] on [dbo].[RequestView].addressId = [dbo].[AddressView].id
		where brigadeId = @brigadeId 
		order by [dbo].[RequestView].id desc;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetRequestsForClient(@clientId int)
as begin
	begin try
		select requestId 'id', brigadeId, clientId, addressId, mountingDate, comment, stageId,  statusId,
		country, city, street, house, corpus, flat from RequestForClientView
		where clientId = @clientId order by id desc;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetRequestsForClientById(@requestId int) --client role
as begin
	begin try
		select * from RequestForClientView where requestId = @requestId order by requestId desc;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetRequestEquipmentByRequestId(@requestId int) --AdminWindow\Request, BrigadeWindow, REPORT
as begin
	begin try
		select quantity, equipmentId, [type], mounting from RequestEquipmentForReportView
		where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetRequestsCountByStatusId(@statusId int)
as begin
	begin try
		select count(id) as rqcount from [dbo].[RequestView] where statusId = @statusId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Tool--------------------------------------------------------------------------------------
create or alter procedure GetAllTools
as begin
	begin try
		select id, [name], stageId from [dbo].[ToolView]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetTool(@id int)
as begin
	begin try
		select id, [name], stageId from [dbo].[ToolView] where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--User--------------------------------------------------------------------------------------
--пароль в приложение
create or alter procedure LoginUser(@login nvarchar(20))
as begin
	begin try
		select id, [login], [password] from [dbo].[UserView] where [login] = @login
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
