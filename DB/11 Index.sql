use TheBureau;
go

---Tables---------------------------------------------------------------------------------------------------------------------------
create nonclustered index nocl_Accessory_index on [dbo].[Accessory] (id) include(name, art, equipmentId, price);
create nonclustered index nocl_Tool_index on [dbo].[Tool] (id) include(name, stageId);
create nonclustered index nocl_User_index on [dbo].[User] ([login], id) include([password]);
go



---Views----------------------------------------------------------------------------------------------------------------------------

--Accessory--
create unique clustered index cl_AccessoryView_index on [dbo].AccessoryView (id);
create nonclustered index nocl_AccessoryView_index on [dbo].AccessoryView (id) include ([name], art, equipmentId, price);
go

--Tool--
create unique clustered index cl_ToolView_index on [dbo].ToolView (id);
create nonclustered index nocl_ToolView_index on [dbo].ToolView (id) include ([name], stageId);
go

--RequestEquipment, RequestAccessory, RequestTool--
create unique clustered index cl_RequestEquipmentView_index on [dbo].RequestEquipmentView (requestId, equipmentId);
create unique clustered index cl_RequestAccessoryView_index on [dbo].RequestAccessoryView (requestId, accessoryId);
create unique clustered index cl_RequestToolView_index on [dbo].RequestToolView (requestId, toolId);
go

--User--
create unique clustered index cl_UserView_index on [dbo].[UserView] (id);
create nonclustered index nocl_UserView_index on [dbo].[UserView] (id, [login]) include([password]);
go

--Request--
create unique clustered index cl_RequestView_index on [dbo].[RequestView] (id);
create nonclustered index nocl_RequestView_index on [dbo].[RequestView] (id) include(statusId, brigadeId);
create nonclustered index nocl_RequestViewBrigade_index on [dbo].[RequestView] (brigadeId);
create nonclustered index nocl_RequestViewClient_index on [dbo].[RequestView] (clientId);
go

--Brigade, Client, Address, Employee
create unique clustered index cl_EmployeeView_index on [dbo].[EmployeeView] (id);
create unique clustered index cl_BrigadeView_index on [dbo].[BrigadeView] (id);
create unique clustered index cl_ClientView_index on [dbo].[ClientView] (id);
create unique clustered index cl_AddressView_index on [dbo].[AddressView] (id);
go