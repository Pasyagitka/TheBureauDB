EXEC sp_configure 'show advanced options', 1;  
GO  
RECONFIGURE;  
GO  
EXEC sp_configure 'xp_cmdshell', 1;  
GO  
RECONFIGURE; 
go

--xp_cmdshell from program - ExportConnection string
--Pass folder path to create file (TableYYYYMMDD-HHMMSS.json)  ---------------------------------------------------------------
create or alter procedure ExportTable(@table varchar(30), @path varchar(260))
as begin
	begin try
		declare @result int;
		--текущая дата в формате YYYYMMDD-HHMMSS
		declare @date varchar(100) = (select (replace(convert(varchar, getdate(), 23),'-','') +  '-' +  replace(convert(varchar, getdate(), 8),':','')) );
		--создать имя файла в формате TableYYYYMMDD-HHMMSS.json
		declare @filename varchar(255)  = (select concat(@path, '\', @table, @date, '.json'));
		--параметр процедуры - имя таблицы на экспорт
		declare @command varchar(700)= 'bcp "SELECT * FROM [TheBureau].[dbo].['+@table+']  FOR JSON AUTO, INCLUDE_NULL_VALUES;" queryout "' + @filename + '" -t, -k -c -C UTF-8 -S PASYAGITKAASUS -T'
		exec @result = xp_cmdshell @command, no_output
		select @filename as [filename], @result as [success] --result = 0 -> success
	end try
	begin catch
		select ('Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message()) as error;
	end catch
end;
go
--queryout copies from a query and must be specified only when bulk copying data from a query.
--с performs the operation using a character data type
--d databasename
--t specifies the field terminator, the default is \t
--k empty columns should retain a null value during the operation
--T integrated security - trusted connection


--Pass filename (.json) containing Tools to load them into DB ---------------------------------------------------------------
--Параметр процедуры  файл (.json), содержащий инструменты 
create or alter procedure ImportTools(@filename nvarchar(max))
as begin
	begin try
		declare @json varchar(max)			--импорт из передаваемого файла
		declare @command nvarchar(max)= N'select @json = bulkcolumn from openrowset(BULK ''' + @filename + ''', SINGLE_CLOB) Imported 
										insert into [dbo].[Tool]([name], [stageId]) 
										output inserted.id as [id], inserted.name as [name], inserted.stageId as [stageId]
										select * from openjson(@json) with ([name] nvarchar(30), [stageId] int)';
		exec sp_executesql @command,  N'@filename nvarchar(max), @json varchar(max) output', @filename, @json output;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
create or alter procedure ImportAccessory(@filename nvarchar(max))
as begin
	begin try
		declare @json varchar(max)  
		declare @command nvarchar(max)= N'select @json = bulkcolumn from openrowset(BULK ''' + @filename + ''', SINGLE_CLOB) Imported 
			insert into [dbo].[Accessory](art, [equipmentId], [name], price) 
			output inserted.id as [id], inserted.art as [art], inserted.equipmentId as [equipmentId], inserted.name as [name], inserted.price as price
			select * from openjson(@json) with ([art] nvarchar(15), [equipmentId] nvarchar(3), [name] nvarchar(150), price decimal(6,2))';
		exec sp_executesql @command,  N'@filename nvarchar(max), @json varchar(max) output', @filename, @json output;
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go


--Exports schedule for an employee
create or alter procedure ExportScheduleForEmployee(@path varchar(260), @employeeId int)
as begin
	begin try
		declare @result int;
		declare @date varchar(100) = (select (replace(convert(varchar, getdate(), 23),'-','') +  '-' +  replace(convert(varchar, getdate(), 8),':','')) );
		declare @filename varchar(500)  = (select concat(@path, '\ScheduleEmpl#', @employeeId, @date, '.json'));
		declare @command varchar(700)= 'bcp "SELECT * FROM [TheBureau].[dbo].[Schedule] where employeeId = ' + cast(@employeeId as varchar(10)) + ' FOR JSON AUTO, INCLUDE_NULL_VALUES;" queryout "' + @filename + '" -t, -k -c -C UTF-8 -S PASYAGITKAASUS -T'
		exec @result = xp_cmdshell @command, no_output
		select @filename as [filename], @result as [success]
	end try
	begin catch
		select ('Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message()) as error;
	end catch
end;
go

--exec ExportScheduleForEmployee 'D:\', 6
--select * from Tool

