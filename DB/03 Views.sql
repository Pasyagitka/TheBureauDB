use TheBureau;
go

---Primary views for data manipulating ----------------------------------------------------------------------------------------------------------------------------------
create or alter view [UserView] 
with schemabinding
as select id, [login], [password] from [dbo].[User] where isDeleted = 0; 
go
create or alter view [ToolView] 
with schemabinding
as select id, [name], stageId  from [dbo].[Tool] where isDeleted = 0; 
go
create or alter view [EquipmentView] 
with schemabinding
as select id, [type], mountingId from [dbo].[Equipment] where isDeleted = 0; 
go
create or alter view [AccessoryView] 
with schemabinding
as select id, art, equipmentId, [name], price from [dbo].[Accessory] where isDeleted = 0; 
go
create or alter view [BrigadeView] 
with schemabinding
as select id, userId, brigadierId, creationDate from [dbo].[Brigade] where isDeleted = 0; --если не "удалено" из таблицы 
go
create or alter view [EmployeeView] 
with schemabinding
as select id, firstname, patronymic, surname, email, contactNumber, brigadeId from [dbo].[Employee] where isDeleted = 0; 
go
create or alter view [ClientView]
with schemabinding
as select id, firstname, patronymic, surname, email, contactNumber from [dbo].[Client] where isDeleted = 0; 
go
create or alter view [AddressView]
with schemabinding
as select id, country, city, street, house, corpus, flat from [dbo].[Address] where isDeleted = 0; 
go
create or alter view [RequestView] 
with schemabinding
as select id, clientId, addressId, stageId, statusId, registerDate, mountingDate, comment, brigadeId, proceeds  from [dbo].[Request] where isDeleted = 0; 
go
create or alter view [RequestEquipmentView] 
with schemabinding
as select id, requestId, equipmentId, quantity from [dbo].[RequestEquipment] where isDeleted = 0; 
go
create or alter view [RequestToolView]
with schemabinding
as select id, requestId, toolId from [dbo].[RequestTool] where isDeleted = 0; 
go
create or alter view [RequestAccessoryView]
with schemabinding
as select id, requestId, accessoryId, quantity from [dbo].[RequestAccessory] where isDeleted = 0; 
go
create or alter view [ScheduleView]
as select id, employeeId, requestId, modified from [dbo].[Schedule] where isDeleted = 0; 
go


------------------------------------------------------------------------------------------------------------------------------------------------------
-- Display a request for client (Request, Client, Address data) --  used by GetRequestsForClientById procedure
create or alter view RequestForClientView
as  
		select [dbo].[RequestView].id 'requestId', [dbo].[RequestView].mountingDate, [dbo].[RequestView].comment, [dbo].[RequestView].stageId, [dbo].[RequestView].statusId,
		[dbo].[RequestView].brigadeId, [dbo].[RequestView].clientId, [dbo].[RequestView].addressId,
		[dbo].[AddressView].country, [dbo].[AddressView].city, [dbo].[AddressView].street, [dbo].[AddressView].house, [dbo].[AddressView].corpus, [dbo].[AddressView].flat,
		[dbo].[ClientView].firstname, [dbo].[ClientView].surname, [dbo].[ClientView].patronymic, [dbo].[ClientView].email,  [dbo].[ClientView].contactNumber
		from [dbo].[RequestView]
		join [dbo].[ClientView] on  [dbo].[RequestView].clientId = [dbo].[ClientView].id
		join [dbo].[AddressView] on [dbo].[RequestView].addressId = [dbo].[AddressView].id
go

-- Display a brigade with its Employees - used by GetBrigadeByUserId procedure
--отобразить бригаду вместе с работниками
create or alter view ForBrigadeView
as 
	select [dbo].[BrigadeView].id, [dbo].[BrigadeView].userId, [dbo].[BrigadeView].brigadierId, --информация о бригаде
	[dbo].[EmployeeView].firstname, [dbo].[EmployeeView].surname, [dbo].[EmployeeView].patronymic, [dbo].[EmployeeView].email,  [dbo].[EmployeeView].contactNumber
	--информация о работниках
	from [dbo].[BrigadeView] left join  -- в бригаде может не быть работников
	[dbo].[EmployeeView] on [dbo].[EmployeeView].brigadeId = [dbo].[BrigadeView].id
go

---SSRS ------------------ REPORTS----------------------------------------------------------------------------------------------------------------------------------------------

-- Display Request (Request, Address, Client, Stage, Status) -- used by RequestReport procedure
create or alter view RequestForReportView 
as select [dbo].[RequestView].id as 'requestId',  [dbo].[RequestView].registerDate, [dbo].[RequestView].mountingDate, [dbo].[RequestView].comment, 
		[dbo].[Stage].stage, [dbo].[Status].[status], [dbo].[RequestView].proceeds,

		[dbo].[AddressView].country, [dbo].[AddressView].city, [dbo].[AddressView].street, [dbo].[AddressView].house, [dbo].[AddressView].corpus, [dbo].[AddressView].flat,

		[dbo].[ClientView].firstname, [dbo].[ClientView].surname, [dbo].[ClientView].patronymic, [dbo].[ClientView].email,  [dbo].[ClientView].contactNumber
		from [dbo].[RequestView]
		join [dbo].[ClientView] on  [dbo].[RequestView].clientId = [dbo].[ClientView].id
		join [dbo].[AddressView] on [dbo].[RequestView].addressId = [dbo].[AddressView].id
		join [dbo].[Stage] on [dbo].[RequestView].stageId = [dbo].[Stage].id
		join [dbo].[Status] on [dbo].[RequestView].statusId = [dbo].[Status].id
go

-- Display RequestAccessories -- used by GetRequestAccessoryByRequestId procedure
create or alter view RequestAccessoriesForReportView 
as 
	select [RequestAccessoryView].requestId, quantity, art, [name], price 
	from [dbo].[RequestAccessoryView] join [dbo].[AccessoryView] on [dbo].[AccessoryView].id = [dbo].[RequestAccessoryView].accessoryId
go

-- Display RequestTools --used by GetRequestToolByRequestId procedure
create or alter view RequestToolsForReportView 
as
	 select [dbo].[RequestToolView].requestId, [dbo].[ToolView].[name]  
	 from [dbo].[RequestToolView] join [dbo].[ToolView] on [dbo].[RequestToolView].toolId = [dbo].[ToolView].id
go

-- Display RequestEquipment --used by GetRequestEquipmentByRequestId procedure
create or alter view RequestEquipmentForReportView
as
	select [dbo].[RequestEquipmentView].requestId, quantity, equipmentId, [type], mounting
	from [dbo].[RequestEquipmentView] 
	join [dbo].[EquipmentView] on [dbo].[RequestEquipmentView].equipmentId = [dbo].[EquipmentView].id
	join [dbo].[Mounting] on mountingId = [dbo].[Mounting].id
go
------------------------------------------------------------------------------------------------------------------------------------------------------