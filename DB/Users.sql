use TheBureau;
go

--drop login [admin_login];
--drop user [thebureau_admin_user];

--Admin
create login [admin_login] with password=N'admin', default_database=[TheBureau], default_language=[русский],
check_expiration=off, check_policy=on
go
create user [thebureau_admin_user] for login [admin_login];  
go


--Client
create login [client_login] with password=N'client', default_database=[TheBureau], default_language=[русский],
check_expiration=off, check_policy=on
go
create user [thebureau_client_user] for login [client_login];  
go


--Brigade
create login [brigade_login] with password=N'brigade', default_database=[TheBureau], default_language=[русский],
check_expiration=off, check_policy=on
go
create user [thebureau_brigade_user] for login [brigade_login];  
go