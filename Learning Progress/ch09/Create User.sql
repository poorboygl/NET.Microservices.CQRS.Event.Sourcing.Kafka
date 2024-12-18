Use SocialMedia
GO

If not EXISTS(Select * from sys.server_principals where name= 'SMUser')
Begin
	Create Login SMUser WITH PASSWORD=N'SmPA$$06500', Default_database=SocialMedia
End

IF NOT EXISTS(Select * from sys.database_principals where name= 'SMUser')
begin
	Exec sp_adduser 'SMUser', 'SMUser', 'db_owner';
end