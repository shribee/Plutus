Clear-Host
Set-location "C:\My Folders\MSSQL\Projects\Plutus\Plutus\Plutus.ETL.CS.Test\TestFiles"
###

$SQLInstance = "(LocalDb)\MSSQLLocalDB"
sqlcmd -U "sa" -P "Password123$" -S $SQLInstance -i CreateDB.sql
sqlcmd -U "sa" -P "Password123$" -S $SQLInstance -i CreateObjects.sql
