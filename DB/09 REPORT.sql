use TheBureau;
go

--Report for single brigade (BrigadeScheduleReport.rdl)
create or alter procedure BrigadeScheduleReport(@brigadeId int)
as begin
	begin try
		--все заявки для этой бригады, которые надо выполнять в эту неделю, (еще не завершены)
		select [понедельник], [вторник], [среда], [четверг], [пятница], [суббота], [воскресенье]
		from (
			select [dbo].[RequestView].id as id, datename(weekday, mountingdate) as wday 
			from [dbo].[RequestView] join [Status] on [dbo].[RequestView].statusId = [Status].id
			where 
			brigadeId = @brigadeId and  --бригада передается как параметр процедуры
			datename(week, mountingdate) = datename(week, getdate()) and --текущая неделя 
			status in('InProgress', 'InProcessing') --заявка еще не завершена
		) s
		pivot (count(id) for wday in([понедельник],[вторник],[среда],[четверг], [пятница], [суббота], [воскресенье])) p;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Report for all brigades (AllBrigadesScheduleReport.rdl)
create or alter procedure AllBrigadesScheduleReport
as begin
	begin try
		select [brigadeId], [понедельник], [вторник], [среда], [четверг], [пятница], [суббота], [воскресенье]
		from (
			select [dbo].[RequestView].brigadeId as brigadeId, [dbo].[RequestView].id as id, datename(weekday, mountingdate) as wday 
			from [dbo].[RequestView] join [Status] on [dbo].[RequestView].statusId = [Status].id
			where [dbo].[RequestView].brigadeId is not null and
			datename(week, mountingdate) = datename(week, getdate()) and 
			status in('InProgress', 'InProcessing')
			group by brigadeId, [dbo].[RequestView].id, datename(weekday, mountingdate)
		) s
		pivot (count(id) for wday in([понедельник],[вторник],[среда],[четверг], [пятница], [суббота], [воскресенье])) p;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

----Procedures for Request Report -------------------------------------------------------------------------------------------------------------------------------
-- Display Request, Client, Address
create or alter procedure RequestReport(@requestId int)
as begin
	begin try
		select * from RequestForReportView where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Display RequestAccessories
create or alter procedure GetRequestAccessoryByRequestId(@requestId int)
as begin
	begin try
		select requestid, quantity, art, name, price
		from RequestAccessoriesForReportView
		where (requestid = @requestid)
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Display RequestTools
create or alter procedure GetRequestToolByRequestId(@requestId int)
as begin
	begin try
		select requestid, name from RequestToolsForReportView 
		where requestId = @requestId;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--Display RequestEquipment
--GetRequestEquipmentByRequestId in "05 Select procedures"