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
--grant execute on to thebureau_admin_role;
--grant execute on to thebureau_admin_role;
--grant execute on to thebureau_admin_role;
--grant execute on to thebureau_admin_role;
--grant execute on to thebureau_admin_role;
--grant execute on to thebureau_admin_role;
--grant execute on to thebureau_admin_role;
--grant execute on to thebureau_admin_role;
--grant execute on to thebureau_admin_role;

--------Client-------

--------Brigade-------
