EXEC sp_configure 'show advanced options', 1;  
GO  
RECONFIGURE;  
GO  
EXEC sp_configure 'xp_cmdshell', 1;  
GO  
RECONFIGURE; 
go


create or alter procedure ExportTools
as begin
	begin try
		exec sys.XP_CMDSHELL 'bcp "SELECT * FROM [TheBureau].[dbo].[Tool] FOR JSON AUTO, INCLUDE_NULL_VALUES;" queryout D:\JsonTest.json -t, -k -c -C UTF-8 -S PASYAGITKAASUS -T'
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go
--exec ExportTools;
--select * from Tool

--queryout copies from a query and must be specified only when bulk copying data from a query.
--ñ performs the operation using a character data type
--d databasename
--t specifies the field terminator, the default is \t
--k empty columns should retain a null value during the operation
--T integrated security - trusted connection



create or alter procedure ImportTools(@filename nvarchar(max))
as begin
	begin try
		declare @json varchar(max)  
		declare @command nvarchar(max)= N'select @json = bulkcolumn from openrowset(BULK ''' + @filename + ''', SINGLE_CLOB) Imported 
			insert into [dbo].[Tool] 
			select * from openjson(@json) with ([name] nvarchar(30), [stageId] int)';
		exec sp_executesql @command,  N'@filename nvarchar(max), @json varchar(max) output', @filename, @json output;
		select @json
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--exec ImportTools 'D:\JsonTest.json'