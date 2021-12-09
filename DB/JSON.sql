EXEC sp_configure 'show advanced options', 1;  
GO  
RECONFIGURE;  
GO  
EXEC sp_configure 'xp_cmdshell', 1;  
GO  
RECONFIGURE; 
go
--выбор пути файла!
--BULK INSERT loads data from a data file into a table
create or alter procedure ImportTools(@filepath nvarchar(200))
as begin
	begin try
	--The OPENROWSET(BULK...) function allows to access remote data by connecting to a remote data source, such as a data file, through an OLE DB provider.
		declare @json varchar(max) select @json = BulkColumn
		--SINGLE_NCLOB reads a file as nvarchar(max) 
		from openrowset (BULK 'D:\JsonTest.json', SINGLE_CLOB) Imported --correlation name is required by OPENROWSET. 
		insert into [dbo].[Tool] 
		select * from openjson(@json) with ([name] nvarchar(30), [stageId] int)
	end try
	begin catch
		print 'Error: ' + cast(error_number() as varchar(6)) + ': ' + error_message();
	end catch
end;
go

--exec ImportTools;
--select * from Tool

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
--с performs the operation using a character data type
--d databasename
--t specifies the field terminator, the default is \t
--k empty columns should retain a null value during the operation
--T integrated security - trusted connection