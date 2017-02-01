
param(
    [String] $accountName,
    [String] $connectionKey,
	[String] $databaseName,
	[String] $collectionName
)

begin
{
	Write-Host $accountName
	Write-Host $databaseName
	Write-Host $collectionName

	function GetKey([System.String]$Verb = '',[System.String]$ResourceId = '',
		[System.String]$ResourceType = '',[System.String]$Date = '',[System.String]$masterKey = '') {
        
		$keyBytes = [System.Convert]::FromBase64String($masterKey) 
		$text = @($Verb.ToLowerInvariant() + "`n" + $ResourceType.ToLowerInvariant() + "`n" + $ResourceId + "`n" + $Date.ToLowerInvariant() + "`n" + "`n")
		$body =[Text.Encoding]::UTF8.GetBytes($text)
		$hmacsha = new-object -TypeName System.Security.Cryptography.HMACSHA256 -ArgumentList (,$keyBytes) 
		$hash = $hmacsha.ComputeHash($body)
		$signature = [System.Convert]::ToBase64String($hash)
 
		[System.Web.HttpUtility]::UrlEncode($('type=master&ver=1.0&sig=' + $signature))
    }
 
    function GetUTDate() {
        $date = get-date
        $date = $date.ToUniversalTime();
        return $date.ToString("ddd, d MMM yyyy HH:mm:ss \G\M\T")
    }

	function BuildHeaders([string]$action = "get",[string]$resType, [string]$resourceId){
        $authz = GetKey -Verb $action -ResourceType $resType -ResourceId $resourceId -Date $apiDate -masterKey $connectionKey
        $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $headers.Add("Authorization", $authz)
        $headers.Add("x-ms-version", '2015-12-16')
        $headers.Add("x-ms-date", $apiDate) 
        $headers
    }

	function GetDatabaseCount() {
        $uri = $rootUri + "/dbs"
        $hdr = BuildHeaders -resType dbs
        $response = Invoke-RestMethod -Uri $uri -Method Get -Headers $hdr
        $response.Databases.Count
        Write-Host ("Found " + $Response.Databases.Count + " Database(s)")
    }

	function CreateDatabase(){
		$uri = $rootUri + "/dbs"
        $headers = BuildHeaders -action Post -resType dbs 
        $headers.Add("x-ms-documentdb-is-upsert", "true")   
		$content = @{id=$databaseName} | ConvertTo-Json
        $response = Invoke-RestMethod $uri -Method Post -Body $content -ContentType 'application/json' -Headers $headers
      }

	function CreateCollection(){
           
        $uri = $rootUri + "/dbs/" + $databaseName + "/colls"
		$resourceId = "dbs/"+ $databaseName
        $headers = BuildHeaders -action Post -resType colls -resourceId $resourceId
        $headers.Add("x-ms-documentdb-is-upsert", "true")
		$headers.Add("x-ms-offer-throughput", "1000")
     
		$content = @{id=$collectionName} | ConvertTo-Json
        $response = Invoke-RestMethod $uri -Method Post -Body $content -ContentType 'application/json' -Headers $headers
    }

	$rootUri = "https://" + $accountName + ".documents.azure.com"
	write-host ("Root URI is " + $rootUri)
	$apiDate = GetUTDate

	$databasecount = GetDatabaseCount 
	if ($databasecount -gt 1) {
		Write-Host "Database already exists"
		return
	} 
 
	CreateDatabase
	CreateCollection

}




