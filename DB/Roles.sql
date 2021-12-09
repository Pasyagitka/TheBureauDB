use TheBureau;
go

create role thebureau_admin_role;
create role thebureau_client_role;
create role thebureau_brigade_role;
go

alter role thebureau_admin_role add member thebureau_admin_user;
alter role thebureau_client_role add member thebureau_client_user;
alter role thebureau_brigade_role add member thebureau_brigade_user;
go

--------Admin-------
grant execute, select, insert, update, delete on schema::dbo to thebureau_admin_role;
go

--------Client-------
grant execute on LoginUser to thebureau_client_role; --вход в приложение - изначально клиент (без прав)
go

grant select on dbo.Request to thebureau_client_role; --выбрать таблицы.
grant select on dbo.Client to thebureau_client_role;
grant select on dbo.[Address] to thebureau_client_role; 
grant select on dbo.Request to thebureau_client_role;
go

grant execute on LeaveRequest to thebureau_client_role;
grant execute on UpdateRequestSetProceeds to thebureau_client_role;
grant execute on AddRequestEquipment to thebureau_client_role;
grant execute on GetRequestsForClientById to thebureau_client_role;
go

--------Brigade-------
grant execute on GetRequestsByBrigadeId to thebureau_brigade_role;
grant execute on GetBrigadeByUserId to thebureau_brigade_role;
grant execute on GetAllRequestsForBrigade to thebureau_brigade_role;
grant execute on GetRequestEquipmentByRequestId to thebureau_brigade_role;
grant execute on GetEmployeesForBrigade to thebureau_brigade_role;
go

grant execute on UpdateRequestByBrigade to thebureau_brigade_role;
go