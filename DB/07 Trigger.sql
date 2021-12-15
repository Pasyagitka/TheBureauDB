use TheBureau;
go

--LeaveRequest: inserts RequestAccessories after insert into RequestEquipment -----------------------------------------------------------------
create or alter trigger SetAccessoriesByEquipmentTrigger
on [dbo].[RequestEquipment]
after insert
as
  begin try
    declare @requestId int = (select requestId from inserted);
    declare @equipmentId nvarchar(3) = (select equipmentId from inserted);
    declare @quantity int = (select quantity from inserted);

    insert into [dbo].[RequestAccessoryView](requestId, accessoryId, quantity)
    select @requestId, id, @quantity from [dbo].[AccessoryView] where equipmentid = @equipmentId;

  end try  
  begin catch
    print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
  end catch
go

--LeaveRequest: inserts RequestTools after insert into Request -----------------------------------------------------------------
create or alter trigger SetToolsByRequestTrigger
on [dbo].[Request]
after insert
as
	begin try
		declare @requestId int = (select id from inserted); --id новой заявки
		declare @stageId int =  (select stageId from inserted);	--стадия выполнения работ новой заявки

		insert into [dbo].[RequestToolView] (requestId, toolId)	--добавить все соответствующие инструменты
		select @requestId, id from [dbo].[ToolView] where stageId = @stageId;
	end try  
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
go
