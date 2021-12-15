use TheBureau;
go

													--Storage
--Accessory-----------------------------------------------------------------
create or alter procedure AddAccessory (@art nvarchar(15), @equipmentId nvarchar(3), @name nvarchar(150), @price decimal(6,2))
as begin
	begin try
		insert into [dbo].[AccessoryView] (art, equipmentId, [name], price)
		values(@art, @equipmentId, @name, @price);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure UpdateAccessory (@id int, @art nvarchar(15), @equipmentId nvarchar(3), @name nvarchar(150), @price decimal(6,2))
as begin
	begin try
		update [dbo].[AccessoryView] set art = @art, equipmentId= @equipmentId, [name]= @name, price = @price
		where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create or alter procedure DeleteAccessory (@accessoryId int)
as begin
	begin try
		begin tran
		--пометить как удаленные комплектующие в зависимых заявках
		update [dbo].[RequestAccessory]  set isDeleted = 1 where [accessoryId] = @accessoryId;
		--пометить как удаленные комплектующее
		update [dbo].[Accessory]  set isDeleted = 1 where [id] = @accessoryId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure GetAccessoryTotalPrice(@requestId int, @totalPrice decimal(6,2) output)
as begin
	begin try
		select @totalPrice = (select sum(price * quantity) as TotalPrice 
		from [dbo].[AccessoryView] join [dbo].[RequestAccessoryView]
		on [dbo].[AccessoryView].id = [dbo].[RequestAccessoryView].accessoryId where requestId = @requestId);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Equipment-----------------------------------------------------------------
create or alter procedure AddEquipment(@id nvarchar(3), @type nvarchar(30), @mountingId int)
as begin
	begin try
		insert into [dbo].[EquipmentView] (id, [type], mountingId) values(@id, @type, @mountingId);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure UpdateEquipment(@id nvarchar(3), @type nvarchar(30), @mountingId int)
as begin
	begin try
		update [dbo].[EquipmentView] set [type] = @type, mountingId = @mountingId where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure DeleteEquipment(@equipmentId nvarchar(3))
as begin
	begin try
		begin tran

		declare @accessoryId int;
		declare cur_acc cursor local for select id from [dbo].[AccessoryView] where [equipmentId] = @equipmentId;
		
		open cur_acc;
		fetch cur_acc into @accessoryId;
		while @@fetch_status = 0
			begin
				exec DeleteAccessory @accessoryId;
				fetch cur_acc into @accessoryId;
			end
		close cur_acc;
		
		update [dbo].[RequestEquipment] set isDeleted = 1 where [equipmentId] = @equipmentId;
		update [dbo].[Equipment] set isDeleted = 1 where [id] = @equipmentId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Tool-------------------------------------------------------------------
create or alter procedure AddTool (@name nvarchar(30), @stageId int)
as begin
	begin try
		insert into [dbo].[ToolView] ([name], stageId)
		values(@name, @stageId);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure UpdateTool (@id int, @name nvarchar(30), @stageId int)
as begin
	begin try
		update [dbo].[ToolView] set [name] = @name, stageId = @stageId where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure DeleteTool (@toolId int)
as begin
	begin try
		begin tran
		update [dbo].[RequestTool] set isDeleted = 1 where [toolId] = @toolId;
		update [dbo].[Tool]  set isDeleted = 1 where [id] = @toolId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Brigade-------------------------------------------------------------------
create or alter procedure AddBrigade(@password nchar(70))
as begin
	begin try
		begin tran
			--добавить новую бригаду, изначально без пользователя для входа и без бригадира
			--установить для бригады время создания - сейчас
			insert into [dbo].[BrigadeView] (userId, brigadierId, creationDate)	values(NULL, NULL, getdate());
			declare @brigadeId int = SCOPE_IDENTITY();	--получить id созданной бригады =ident_current('Brigade')
			declare @login nvarchar(20) = concat('brigade', @brigadeId);	--создать имя пользователя для бригады как brigade+id

			--добавить нового пользователя с переданным как параметр процедуры паролем
			insert into [dbo].[UserView] ([login], [password]) values(@login, @password);	
			declare @userId int = SCOPE_IDENTITY();  --получить id созданного пользователя
			--обновить данные о пользователе для бригады
			update [dbo].[BrigadeView] set userId = @userId where id = @brigadeId;
			select * from [dbo].[BrigadeView] where id = @brigadeId; --вернуть id бригады
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure DeleteBrigade(@id int)
as begin
	begin try
		begin tran
			declare @userId int;
			set @userId = (select userId from [dbo].[BrigadeView] where id = @id);
			update [dbo].[EmployeeView] set brigadeId = null where brigadeId = @id; 
			update [dbo].[RequestView] set brigadeId = null where brigadeId = @id; 
			update [dbo].[Brigade] set isDeleted = 1 where id = @id;
			update [dbo].[User] set isDeleted = 1 where id = @userId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure SetBrigadier(@brigadeId int, @employeeId int)
as begin
	begin try
		update [dbo].[BrigadeView] set [BrigadeView].brigadierId = @employeeId where [BrigadeView].id = @brigadeId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Employee-----------------------------------------------------------------
create or alter procedure AddEmployee (@firstname nvarchar(20), @patronymic nvarchar(20), @surname nvarchar(20), @email nvarchar(255), @contactNumber nvarchar(12), @brigadeId int)
as begin
	begin try
		insert into [dbo].[EmployeeView] (firstname, patronymic, surname, email, contactNumber, brigadeId)
		values(@firstname, @patronymic, @surname, @email, @contactNumber, @brigadeId);
		select * from [dbo].[EmployeeView] where id = SCOPE_IDENTITY(); -- ident_current('Employee')
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure UpdateEmployee (@id int, @firstname nvarchar(20), @patronymic nvarchar(20), @surname nvarchar(20), @email nvarchar(255), @contactNumber nvarchar(12), @brigadeId int)
as begin
	begin try
		update [dbo].[EmployeeView] set firstname = @firstname, patronymic = @patronymic, surname = @surname, email = @email, contactNumber = @contactNumber, brigadeId = @brigadeId
		where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure DeleteEmployee (@id int)
as begin
	begin try
		begin tran
			update [dbo].[BrigadeView] set brigadierId = null where brigadierId = @id;
			update [dbo].[Schedule] set isDeleted = 1 where [employeeId] = @id;
			update [dbo].[Employee] set isDeleted = 1 where [id] = @id;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Client--------------------------------------------------------------------
create or alter procedure UpdateClient (@id int, @firstname nvarchar(20), @patronymic nvarchar(20), @surname nvarchar(20), @email nvarchar(255),  @contactNumber nvarchar(12))
as begin
	begin try
		update [dbo].[ClientView] set firstname = @firstname, patronymic = @patronymic, surname = @surname, email = @email, contactNumber = @contactNumber
		where id = @id;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure DeleteClient (@clientId int) 
as begin
	begin try
		begin tran  --удалить клиента -> удалить все его заявки - отключить клиента, не показывать заявки
			declare @requestId int;
			declare cur_req cursor local for select id from [dbo].[RequestView] where clientId = @clientId;
			
			open cur_req;
			fetch cur_req into @requestId;
			while @@fetch_status = 0
				begin
					exec DeleteRequest @requestId;
					fetch cur_req into @requestId;
				end
			close cur_req;

			update [dbo].[Client] set isDeleted = 1 where id = @clientId;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Request-------------------------------------------------------------------
create or alter procedure LeaveRequest (
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
			if exists(select id as existingClientId from [dbo].[ClientView] where @client_firstname = firstname and @client_patronymic = patronymic and @client_surname = surname and @client_email = email and  @client_contactNumber = contactNumber)
				begin
					set @clientId = (select id as existingClientId from [dbo].[ClientView] where @client_firstname = firstname and @client_patronymic = patronymic and @client_surname = surname and @client_email = email and  @client_contactNumber = contactNumber);
				end
			else
				begin
					insert into [dbo].[ClientView] (firstname, patronymic, surname, email, contactNumber)
					values(@client_firstname, @client_patronymic, @client_surname, @client_email,  @client_contactNumber);
					set @clientId = SCOPE_IDENTITY(); 
				end;

			insert into [dbo].[AddressView] (country, city, street, house, corpus, flat)
			values(@address_country, @address_city, @address_street, @address_house,  @address_corpus, @address_flat);
			declare @addressId int =  SCOPE_IDENTITY(); 			

			declare @registerDate date = getdate(); 			
			insert into [dbo].[Request] (clientId, addressId, stageId, statusId, registerDate, mountingDate, comment, brigadeId, proceeds)
			values(@clientId, @addressId, @stageId, @statusId, @registerDate, @mountingDate, @comment, null, null);
	
			declare @requestId int = SCOPE_IDENTITY(); 	--select ident_current('Request') as id;
			select @requestId as id;
		commit;
	end try
	begin catch
		if @@trancount > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--(uses GetTotalAccessoryPrice procedure to update request proceeds field)
create or alter procedure UpdateRequestSetProceeds(@requestId int)
as begin
	begin try
		declare @proceeds decimal(6,2);
		exec GetAccessoryTotalPrice @requestid  = @requestId, @totalPrice = @proceeds output;
		update [dbo].[RequestView] set proceeds = @proceeds where id = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

-- Update request status, brigade (by admin)
create or alter procedure UpdateRequestByAdmin(@id int, @statusId int, @brigadeId int)
as begin
	begin try
		begin tran
		insert into [dbo].[ScheduleView](employeeId, requestId, modified)
		select [dbo].[EmployeeView].id, @id, getdate() from [dbo].[EmployeeView] where brigadeId = @brigadeId
		
		update [dbo].[RequestView]  set statusId = @statusId, brigadeId = @brigadeId where id = @id
		commit;
	end try
	begin catch
		if @@TRANCOUNT > 0 rollback;
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

-- Update request status (by brigade)
create or alter procedure UpdateRequestByBrigade(@id int, @statusId int)
as begin
	begin try
		update [dbo].[RequestView] set statusId = @statusId where id = @id
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


create or alter procedure DeleteRequest(@requestId int)
as begin
	begin try
		begin tran
			update [dbo].[Address] set isDeleted = 1 where id = (select addressId from  [dbo].[Request] where id = @requestId);	--address <--> request
			update [dbo].[Request] set isDeleted = 1 where id = @requestId;
			update [dbo].[RequestEquipment] set isDeleted = 1 where requestId = @requestId;
			update [dbo].[RequestAccessory] set isDeleted = 1 where requestId = @requestId;
			update [dbo].[RequestTool] set isDeleted = 1 where requestId = @requestId;
			update [dbo].[Schedule] set isDeleted = 1 where requestId = @requestId;
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


--RequestTool---------------------------------------------------------------
create or alter procedure AddRequestTool (@requestId int, @toolId int)
as begin
	begin try
		insert into [dbo].[RequestToolView] (requestId, toolId) values(@requestId, @toolId);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--RequestAccessory----------------------------------------------------------
create or alter procedure AddRequestAccessory (@requestId int, @accessoryId int, @quantity int)
as begin
	begin try
		insert into [dbo].[RequestAccessoryView] (requestId, accessoryId, quantity) values(@requestId, @accessoryId, @quantity);
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Schedule------------------------------------------------------------------
create or alter procedure ClearRequestSchedule(@requestId int)
as begin
	begin try
		update [dbo].[Schedule] set isDeleted = 1 where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

create or alter procedure SetScheduleRecord(@employeeId int, @requestId int)
as begin
	begin try
		insert into [dbo].[ScheduleView] (employeeId, requestId, modified) values(@employeeId, @requestId, getdate());
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
