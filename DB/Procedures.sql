use TheBureau;
go

													--Storage
--Accessory-----------------------------------------------------------------
create procedure AddAccessory (@art nvarchar(15), @equipmentId nvarchar(3), @name nvarchar(150), @price decimal(6,2))
as begin
	begin try
		insert into [dbo].[Accessory] (art, equipmentId, [name], price)
		values(@art, @equipmentId, @name, @price);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure UpdateAccessory (@id int, @art nvarchar(15), @equipmentId nvarchar(3), @name nvarchar(150), @price decimal(6,2))
as begin
	begin try
		update [dbo].[Accessory] set art = @art, equipmentId= @equipmentId, [name]= @name, price = @price
		where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteAccessory (@accessoryId int)
as begin
	begin try
		begin tran
		delete from [dbo].[RequestAccessory] where [accessoryId] = @accessoryId;
		delete from [dbo].[Accessory] where [id] = @accessoryId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
alter procedure GetAccessoryTotalPrice(@requestId int, @totalPrice decimal(6,2) output)
as begin
	begin try
		select @totalPrice = (select sum(price * quantity) as TotalPrice from [dbo].[Accessory] join [dbo].[RequestAccessory]
				on [dbo].[Accessory].id = [dbo].[RequestAccessory].accessoryId where requestId = @requestId);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Equipment-----------------------------------------------------------------
create procedure AddEquipment(@type nvarchar(30), @mountingId int)
as begin
	begin try
		insert into [dbo].[Equipment] ([type], mountingId) values(@type, @mountingId);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure UpdateEquipment(@id int, @type nvarchar(30), @mountingId int)
as begin
	begin try
		update [dbo].[Equipment] set [type] = @type, mountingId = @mountingId where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
alter procedure DeleteEquipment(@equipmentId int) --
as begin
	begin try
		begin tran

		declare @accessoryId int;
		declare cur_acc cursor local for select id from [dbo].[Accessory] where [equipmentId] = @equipmentId;
		
		open cur_acc;
		fetch cur_acc into @accessoryId;
		while @@fetch_status = 0
			begin
				exec DeleteAccessory @accessoryId;
				fetch cur_acc into @accessoryId;
			end
		close cur_acc;
		
		delete from [dbo].[RequestEquipment] where [equipmentId] = @equipmentId;
		delete from [dbo].[Equipment] where [id] = @equipmentId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Tool-------------------------------------------------------------------
create procedure AddTool (@name nvarchar(30), @stageId int)
as begin
	begin try
		insert into [dbo].[Tool] ([name], stageId)	values(@name, @stageId);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure UpdateTool (@id int, @name nvarchar(30), @stageId int)
as begin
	begin try
		update [dbo].[Tool] set [name] = @name, stageId = @stageId where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteTool (@toolId int)
as begin
	begin try
		begin tran
		delete from [dbo].[RequestTool] where [toolId] = @toolId;
		delete from [dbo].[Tool] where [id] = @toolId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Brigade-------------------------------------------------------------------
alter procedure AddBrigade(@password nchar(70))
as begin
	begin try
		begin tran
			insert into [dbo].[Brigade] (userId, brigadierId, creationDate)	values(NULL, NULL, getdate());
			declare @brigadeId int = ident_current('Brigade'); 
			declare @login nvarchar(20) = concat('brigade', @brigadeId);

			insert into [dbo].[User] ([login], [password]) values(@login, @password);
			declare @userId int = ident_current('User'); 

			update [dbo].[Brigade] set userId = @userId where id = @brigadeId;
			select * from [dbo].[Brigade] where id = ident_current('Brigade');
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
alter procedure DeleteBrigade(@id int)
as begin
	begin try
		begin tran
			declare @userId int;
			set @userId = (select userId from [dbo].[Brigade] where id = @id);
			update [dbo].[Employee] set brigadeId = null where brigadeId = @id; 
			delete from [dbo].[Brigade] where id = @id;
			delete from [dbo].[User] where id = @userId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Employee-----------------------------------------------------------------
create or alter procedure AddEmployee (@firstname nvarchar(20), @patronymic nvarchar(20), @surname nvarchar(20), 
									@email nvarchar(255), @contactNumber nvarchar(12), @brigadeId int)
as begin
	begin try
		insert into [dbo].[Employee] (firstname, patronymic, surname, email, contactNumber, brigadeId)
		values(@firstname, @patronymic, @surname, @email, @contactNumber, @brigadeId);
		select * from [dbo].[Employee] where id = ident_current('Employee');
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure UpdateEmployee (@id int, @firstname nvarchar(20), @patronymic nvarchar(20), @surname nvarchar(20),
												@email nvarchar(255), @contactNumber nvarchar(12), @brigadeId int)
as begin
	begin try
		update [dbo].[Employee] set firstname = @firstname, patronymic = @patronymic, surname = @surname, email = @email, contactNumber = @contactNumber, brigadeId = @brigadeId
		where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteEmployee (@id int)
as begin
	begin try
		begin tran
			update [dbo].[Brigade] set brigadierId = null where brigadierId = @id;
			delete from [dbo].[Schedule] where [employeeId] = @id;
			delete from [dbo].[Employee] where [id] = @id;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Client--------------------------------------------------------------------
alter procedure UpdateClient (@id int, @firstname nvarchar(20), @patronymic nvarchar(20), 
				@surname nvarchar(20), @email nvarchar(255),  @contactNumber nvarchar(12))
as begin
	begin try
		update [dbo].[Client] set firstname = @firstname, patronymic = @patronymic, surname = @surname, email = @email, contactNumber = @contactNumber
		where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteClient (@clientId int) --
as begin
	begin try
		begin tran 
			--удалить клиента -> удалить все его заявки - отключить клиента, не показывать заявки
			declare @requestId int;
			declare cur_req cursor local for select id from [dbo].[Request] where clientId = @clientId;
			
			open cur_req;
			fetch cur_req into @requestId;
			while @@fetch_status = 0
				begin
					exec DeleteRequest @requestId;
					fetch cur_req into @requestId;
				end
			close cur_req;

			delete from [dbo].[Client] where id = @clientId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--exec LeaveRequest 'ааа', 'ааа', 'ааа', 'ааа@gg.com', '37544676384', 'Беларусь', 'Минск', 'Улица', 3, '', 1, 1, 1, '2021-12-09', 'comm'

--Request-------------------------------------------------------------------
alter procedure LeaveRequest (
			@client_firstname nvarchar(20),
			@client_patronymic nvarchar(20),
			@client_surname nvarchar(20),
			@client_email nvarchar(255),
			@client_contactNumber nvarchar(12),

			@address_country nvarchar(30),
			@address_city nvarchar(30),
			@address_street nvarchar(30),
			@address_house int,
			@address_corpus nvarchar(10),
			@address_flat int,

			@stageId int, 
			@statusId int, 
			@mountingDate date, 
			@comment nvarchar(200)
		)
as begin
	begin try
		begin tran
			declare @clientId int;
			--клиент не дублируется, если до этого был в базе
			if exists(select id as existingClientId from [dbo].[Client] where @client_firstname = firstname and @client_patronymic = patronymic and @client_surname = surname and @client_email = email and  @client_contactNumber = contactNumber)
				begin
					set @clientId = (select id as existingClientId from [dbo].[Client] where @client_firstname = firstname and @client_patronymic = patronymic and @client_surname = surname and @client_email = email and  @client_contactNumber = contactNumber);
				end
			else
				begin
					insert into [dbo].[Client] (firstname, patronymic, surname, email, contactNumber)
					values(@client_firstname, @client_patronymic, @client_surname, @client_email,  @client_contactNumber);
					set @clientId = ident_current('Client'); 
				end;
			print @clientId;

			insert into [dbo].[Address] (country, city, street, house, corpus, flat)
			values(@address_country, @address_city, @address_street, @address_house,  @address_corpus, @address_flat);
			declare @addressId int = ident_current('Address'); 			

			declare @registerDate date = getdate(); 			
			insert into [dbo].[Request] (clientId, addressId, stageId, statusId, registerDate, mountingDate, comment, brigadeId, proceeds)
			values(@clientId, @addressId, @stageId, @statusId, @registerDate, @mountingDate, @comment, null, null);
	
			declare @requestId int = ident_current('Request'); 		
			select ident_current('Request') as id;	
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--update request set proceeds (GetTotalAccessoryPrice for request) - вычисляемое
alter procedure UpdateRequestSetProceeds(@requestId int)
as begin
	begin try
		declare @proceeds decimal(6,2);
		exec GetAccessoryTotalPrice @requestid  = @requestId, @totalPrice = @proceeds output;
		update [dbo].[Request] set proceeds = @proceeds where id = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--select * from Request;
--exec UpdateRequestSetProceeds @requestId = 32



--Admin: Change request status, brigade
alter procedure UpdateRequestByAdmin(@id int, @statusId int, @brigadeId int)
as begin
	begin try
		update [dbo].[Request]  set statusId = @statusId, brigadeId = @brigadeId where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Brigade: Change request status
alter procedure UpdateRequestByBrigade(@id int, @statusId int)
as begin
	begin try
		update [dbo].[Request]  set statusId = @statusId where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
alter procedure DeleteRequest(@requestId int) --
as begin
	begin try
		begin tran
			delete from [dbo].[Address] where id = (select addressId from  [dbo].[Request] where id = @requestId);	--address <--> request
			delete from [dbo].[Request] where id = @requestId;
			delete from [dbo].[RequestEquipment] where requestId = @requestId;
			delete from [dbo].[RequestAccessory] where requestId = @requestId;
			delete from [dbo].[RequestTool] where requestId = @requestId;
			delete from [dbo].[Schedule] where requestId = @requestId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--RequestEquipment----------------------------------------------------------
create or alter procedure AddRequestEquipment (@requestId int, @equipmentId nvarchar(3), @quantity int)
as begin
	begin try
		insert into [dbo].[RequestEquipment] (requestId, equipmentId, quantity)
		values(@requestId, @equipmentId, @quantity);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteRequestEquipment (@requestEquipmentId int)
as begin
	begin try
		delete from [dbo].[RequestEquipment] where id = @requestEquipmentId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteRequestEquipmentByRequestId (@requestId int)
as begin
	begin try
		delete from [dbo].[RequestEquipment] where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--RequestTool---------------------------------------------------------------
create procedure AddRequestTool (@requestId int, @toolId int)
as begin
	begin try
		insert into [dbo].[RequestTool] (requestId, toolId) values(@requestId, @toolId);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteRequestTool (@requestToolId int)
as begin
	begin try
		delete from [dbo].[RequestTool] where id = @requestToolId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteRequestToolByRequestId (@requestId int)
as begin
	begin try
		delete from [dbo].[RequestTool] where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--RequestAccessory----------------------------------------------------------
create procedure AddRequestAccessory (@requestId int, @accessoryId int, @quantity int)
as begin
	begin try
		insert into [dbo].[RequestAccessory] (requestId, accessoryId, quantity) values(@requestId, @accessoryId, @quantity);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteRequestAccessory (@requestAccessoryId int)
as begin
	begin try
		delete from [dbo].[RequestAccessory] where id = @requestAccessoryId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteRequestAccessoryByRequestId (@requestId int)
as begin
	begin try
		delete from [dbo].[RequestAccessory] where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Schedule------------------------------------------------------------------
create procedure AddScheduleRecord (@employeeId int, @requestId int, @attend bit)
as begin
	begin try
		insert into [dbo].[Schedule] (employeeId, requestId, attend) values(@employeeId, @requestId, @attend);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure UpdateScheduleRecord (@id int, @employeeId int, @requestId int, @attend bit)
as begin
	begin try
		update [dbo].[Schedule] set employeeId = @employeeId, requestId = @requestId, attend = @attend
		where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create procedure DeleteScheduleRecord (@recordId int)
as begin
	begin try
		delete from [dbo].[Schedule] where id = @recordId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
