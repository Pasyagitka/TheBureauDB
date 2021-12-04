use TheBureau;
go


exec AddEmployee @firstname = 'Иван', @patronymic = 'Иванович', @surname = 'Иванов', @email = 'i@mail.by', @contactNumber = '375115768798', @brigadeId = NULL;
exec AddEmployee @firstname = 'Андрей', @patronymic = 'Андреевич', @surname = 'Андреев', @email = 'a@mail.by', @contactNumber = '375111736458', @brigadeId = NULL;
exec AddEmployee @firstname = 'Алексей', @patronymic = 'Алексеевич', @surname = 'Алексеев', @email = 'l@mail.by', @contactNumber = '375119875643', @brigadeId = NULL;
exec AddEmployee @firstname = 'Сергей', @patronymic = 'Сергеевич', @surname = 'Сергеев', @email = 's@mail.by', @contactNumber = '375111239785', @brigadeId = NULL;
exec AddEmployee @firstname = 'Денис', @patronymic = 'Денисович', @surname = 'Денисов', @email = 'd@mail.by', @contactNumber = '375111736458', @brigadeId = NULL;
exec AddEmployee @firstname = 'Егор', @patronymic = 'Егорович', @surname = 'Егоров', @email = 'e@mail.by', @contactNumber = '375117896453', @brigadeId = NULL;
go
select * from employee;
go

exec DeleteBrigade @id = 9

select * from Brigade
select * from [User]