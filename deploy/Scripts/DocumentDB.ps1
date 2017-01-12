param(
    [String] $Uri,
    [String] $connectionKey,
	[String] $databaseName,
	[String] $collectionName
)

begin
{
	$DocumentDBModule = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, "documentdbcmdlets\Azrdocdb.dll"))
	Import-Module $DocumentDBModule
    $ctx = New-Context -Uri $Uri  -Key $connectionKey

    $databases = Get-databases -Context $ctx 
	if ($databases.Count -gt 1) {
		Write-Host "Database already exists"
		return
	} 

    $db = Add-Database -Context $ctx -Name $databaseName 
    $coll = Add-DocumentCollection -Context $ctx -DatabaseLink $db.SelfLink -Name $collectionName -AutoIndexing $true 
    Get-database -Context $ctx -SelfLink $db.SelfLink | ft
}