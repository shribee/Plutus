Clear-Host
Set-location "C:\My Folders\MSSQL\Projects\Plutus\Plutus\Plutus.ETL.CS.Test"
####

# Replace the db connection with the local instance 
$TargetConnectionString1 = "metadata=res://*/Model.BusinessAccountingModel.csdl|res://*/Model.BusinessAccountingModel.ssdl|res://*/Model.BusinessAccountingModel.msl;provider=System.Data.SqlClient;provider connection string=""data source="
$TargetConnectionString2 = "(local)\SQL2016;initial catalog=BusinessAccounting;User ID=sa;Password=Password12!;"
$TargetConnectionString3 = "MultipleActiveResultSets=True;App=EntityFramework"""

$TargetConnectionString = "$TargetConnectionString1$TargetConnectionString2$TargetConnectionString3"

$Config = join-path $PWD "App.config"
$Doc = (gc $config) -as [xml]
$Doc.SelectSingleNode('//connectionStrings/add[@name="BusinessAccountingEntities"]').connectionString = $TargetConnectionString
$Doc.Save($config)

