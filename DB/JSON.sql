EXEC sp_configure 'show advanced options', 1;  
GO  
RECONFIGURE;  
GO  
EXEC sp_configure 'xp_cmdshell', 1;  
GO  
RECONFIGURE;  

alter procedure Export
as
	declare @JSON varchar(max)
	select @JSON=BulkColumn
	from openrowset (BULK 'C:\sqlshack\Results.JSON', SINGLE_CLOB) import
	if (isjson(@json)=1)
		select * into jsontable
		from openjson (@json)
		with 
		(
			[FirstName] varchar(20), 
			[MiddleName] varchar(20), 
			[LastName] varchar(20), 
			[JobTitle] varchar(20), 
			[PhoneNumber] nvarchar(20), 
			[EmailAddress] nvarchar(100)
		)
	else
	print 'error in json format'




GO

alter procedure Import
EXEC sys.XP_CMDSHELL 'bcp "SELECT * FROM [TheBureau].[dbo].[Employee] FOR JSON AUTO, INCLUDE_NULL_VALUES;" queryout D:\JsonTest.json -t, -k -c -C UTF-8 -S PASYAGITKAASUS -T'

--queryout copies from a query and must be specified only when bulk copying data from a query.
--ñ performs the operation using a character data type
--d databasename
--t specifies the field terminator, the default is \t
--k empty columns should retain a null value during the operation
--T integrated security - trusted connection