use TheBureau;
go


--Stage--------------------------------------------------------------------------------------
create procedure GetAllStages
as begin
	begin try
		select * from [dbo].[Stage]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Client--------------------------------------------------------------------------------------
create procedure GetAllClients
as begin
	begin try
		select * from [dbo].[Client]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetClient(@id int)
as begin
	begin try
		select * from [dbo].[Client] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Accessory--------------------------------------------------------------------------------------
create procedure GetAllAccessories
as begin
	begin try
		select * from [dbo].[Accessory]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetAccessoriesIdByEquipmentId(@id int)
as begin
	begin try
		select * from [dbo].[Accessory] where equipmentId = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Address--------------------------------------------------------------------------------------

--create procedure GetAllAddresses
--as begin
--	begin try
--		select * from [dbo].[Address]
--	end try
--	begin catch
--		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
--	end catch
--end;
go

--Brigade--------------------------------------------------------------------------------------

create procedure GetAllBrigades
as begin
	begin try
		select * from [dbo].[Brigade]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetBrigade(@id int)
as begin
	begin try
		select * from [dbo].[Brigade] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetBrigadeByUserId(@userId int) 
as begin 
	begin try
		select * from [dbo].[BrigadeView] where userId = @userId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--alter procedure GetBrigadeWithEmployees(@id int) 
--as begin 
--	begin try
--		select brigadeId, brigadierId, firstname, patronymic, surname, email, contactNumber from [dbo].[Brigade] join [dbo].[Employee] on [dbo].[Employee].brigadeId = [dbo].[Brigade].id
--		where brigadeId = @id;
--	end try
--	begin catch
--		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
--	end catch
--end;
--go

--Employee--------------------------------------------------------------------------------------

create procedure GetAllEmployees
as begin
	begin try
		select * from [dbo].[Employee]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetEmployeesForBrigade(@brigadeId int)
as begin
	begin try
		select * from [dbo].[Employee] where brigadeId = @brigadeId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetEmployee(@id int)
as begin
	begin try
		select * from [dbo].[Employee] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
		if error_procedure() is not null print 'in procedure : ' + error_procedure();
	end catch
end;
go

--create procedure FindEmployeesByCriteria(@criteria nvarchar(255))
--as begin
--	begin try
--		select id from [dbo].[Employee] where firstname=@criteria or patronymic = @criteria or surname = @criteria or email = @criteria or contactNumber = @criteria;
--	end try
--	begin catch
--		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
--	end catch
--end;
--go


--Equipment--------------------------------------------------------------------------------------
create procedure GetAllEquipment
as begin
	begin try
		select * from [dbo].[Equipment]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Request--------------------------------------------------------------------------------------
create procedure GetAllRequests
as begin
	begin try
		select * from [dbo].[Request]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetRequest (@id int)
as begin
	begin try
		select * from [dbo].[Request] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

alter procedure GetAllRequestsForRequestView
as begin
	begin try
		select * from [dbo].[Request] join [dbo].[Client] on [dbo].[Request].clientId = [dbo].[Client].id 
		join [dbo].[Address] on [dbo].[Request].addressId = [dbo].[Address].id
		left join [dbo].[RequestEquipment] on [dbo].[Request].id = [dbo].[RequestEquipment].requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

alter procedure GetAllRequestsForBrigade(@brigadeId int)
as begin
	begin try
		select * from [dbo].[Request] join [dbo].[Client] on [dbo].[Request].clientId = [dbo].[Client].id 
									  join [dbo].[Address] on [dbo].[Request].addressId = [dbo].[Address].id
		where brigadeId = @brigadeId order by  [dbo].[Request].id desc;

		--select [dbo].[RequestEquipment].id, equipmentId, quantity from [dbo].[Request]
		--join [dbo].[RequestEquipment] on  [dbo].[Request].id = requestId  where brigadeId = @brigadeId


		--select [type], mounting, equipmentId, quantity 
		--		from [dbo].[RequestEquipment] join [dbo].[Equipment] on RequestEquipment.equipmentId = Equipment.id
		--		join Mounting on mountingId = Mounting.id
		--where requestId = @brigadeId;
		--select accessoryId, quantity from [dbo].[RequestAccessory] where requestId = @brigadeId;
		--select toolId from [dbo].[RequestTool] where requestId = @brigadeId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

alter procedure GetRequestsByBrigadeId(@brigadeId int)
as begin
	begin try
		select * from [dbo].[Request] where brigadeId = @brigadeId order by id desc;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

alter procedure GetRequestsForClient(@clientId int)
as begin
	begin try
		select * from [dbo].[Request] join [dbo].[Address] on [dbo].[Request].addressId = [dbo].[Address].id
		where clientId = @clientId order by  [dbo].[Request].id desc;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Client side
create or alter procedure GetRequestsForClientById(@requestId int) 
as begin
	begin try
		select * from RequestForClientView where requestId = @requestId order by requestId desc;
		--select * from RequestForClientView left join [dbo].[RequestEquipment] on [dbo].[Request].id = [dbo].[RequestEquipment].requestId
		--select * from RequestForClientView left join [dbo].[RequestAccessory] on [dbo].[Request].id = [dbo].[RequestAccessory].requestId
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


create procedure GetRequestAccessoryByRequestId(@requestId int)
as begin
	begin try
		select * from [dbo].[RequestAccessory] where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

alter procedure GetRequestEquipmentByRequestId(@requestId int)
as begin
	begin try
		select quantity, equipmentId, [type], mounting
		from [dbo].[RequestEquipment] join [dbo].[Equipment] 
		on [dbo].[RequestEquipment].equipmentId = [dbo].[Equipment].id
		join [dbo].[Mounting] on mountingId = [dbo].[Mounting].id
		where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetRequestToolByRequestId(@requestId int)
as begin
	begin try
		select * from [dbo].[RequestTool] where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetRequestsCountByStatusId(@statusId int)
as begin
	begin try
		select count(*) as rqcount from [dbo].[Request] where statusId = @statusId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetToDoRequests
as begin
	begin try
		select * from [dbo].[Request] where statusId in('InProcessing', 'InProgress') order by id Desc;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Tool--------------------------------------------------------------------------------------
create procedure GetAllTools
as begin
	begin try
		select * from [dbo].[Tool]
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetTool(@id int)
as begin
	begin try
		select * from [dbo].[Tool] where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


create procedure GetToolsByStage(@stageid int)
as begin
	begin try
		select * from [dbo].[Tool] where stageId = @stageid;
	end try
begin catch
	print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
end catch
end;
go


--User--------------------------------------------------------------------admin + brigade
--create procedure AddUser(@login nvarchar(20), @password nchar(70))
--as begin
--	begin try
--		insert into [dbo].[User] ([login], [password])
--		values(@login, @password);
--	end try
--	begin catch
--		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
--		if error_procedure() is not null print 'in procedure : ' + error_procedure();
--	end catch
--end;
--go
--create procedure DeleteUser(@userId int)
--as begin
--	begin try
--		delete from [dbo].[User] where [id] = @userId;
--	end try
--	begin catch
--		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
--		if error_procedure() is not null print 'in procedure : ' + error_procedure();
--	end catch
--end;
go

--пароль в приложение
alter procedure LoginUser(@login nvarchar(20))
as begin
	begin try
		select * from [dbo].[User] where [login] = @login
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create procedure GetUser(@id int)
as begin
	begin try
		select * from [dbo].[User] where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
