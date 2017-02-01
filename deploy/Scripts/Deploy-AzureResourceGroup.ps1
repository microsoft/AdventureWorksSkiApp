#Requires -Version 3.0
#Requires -Module AzureRM.Resources
#Requires -Module Azure.Storage

Param(
    [string] [Parameter(Mandatory=$true)] $ResourceGroupLocation,
    [string] $ResourceGroupName = 'Skiresort',
    [switch] $UploadArtifacts,
    [string] $StorageAccountName,
    [string] $StorageContainerName = $ResourceGroupName.ToLowerInvariant() + '-stageartifacts',
    [string] $TemplateFile = '..\Templates\Skiresort.json',
    [string] $TemplateParametersFile = '..\Templates\Skiresort.parameters.json',
    [string] $ArtifactStagingDirectory = '..\bin\Debug\staging',
    [string] $DSCSourceFolder = '..\DSC'
)

Import-Module Azure -ErrorAction SilentlyContinue

try {
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.AddUserAgent("VSAzureTools-$UI$($host.name)".replace(" ","_"), "2.9")
} catch { }

Set-StrictMode -Version 3


$OptionalParameters = New-Object -TypeName Hashtable
$TemplateFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $TemplateFile))
$TemplateParametersFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $TemplateParametersFile))

$stopStreamJobs = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'StopStreamJob.ps1'))
& $stopStreamJobs $ResourceGroupName

# Create or update the resource group using the specified template file and template parameters file
New-AzureRmResourceGroup -Name $ResourceGroupName -Location $ResourceGroupLocation -Verbose -Force -ErrorAction Stop 

$configRelativePath = "..\..\src\SkiResort.Web\appsettings.json"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
	$replacescript = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'Replace-FileString.ps1'))

$results = New-AzureRmResourceGroupDeployment -Name ((Get-ChildItem $TemplateFile).BaseName + '-' + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')) `
                                   -ResourceGroupName $ResourceGroupName `
                                   -TemplateFile $TemplateFile `
                                   -TemplateParameterFile $TemplateParametersFile `
                                   @OptionalParameters `
                                   -Force -Verbose

if ($results) {

	Write-Host 'Create containers and upload the sample blobs'
	$StorageName = $results.Outputs.storageServiceName.value
	$storagescript = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'ConfigureStorage.ps1'))
	& $storagescript $ResourceGroupName $StorageName

	Write-Host 'Deploy ASP.NET app (Basic)'
	$webAppName = $results.Outputs.webSiteName.value
	$publisScript = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'PublishASPNET.ps1'))
	& $publisScript $ResourceGroupName $webAppName "..\WebApp\SkiResort.zip"

	Write-Host 'Deploy ASP.NET app (Advanced)'
	$webAppName = $results.Outputs.webSiteAdvancedName.value
	$publisScript = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'PublishASPNET.ps1'))
	& $publisScript $ResourceGroupName $webAppName "..\WebApp\SkiResortAdvanced.zip"

	Write-Host 'Create documentDB collections'
	$documentdbscript = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'DocumentDB.ps1'))
	& $documentdbscript $results.Outputs.documentDBAccount.value $results.Outputs.documentDBKey.value "skiresortliftlines" "liftlines"
	& $documentdbscript $results.Outputs.documentDBAccount.value $results.Outputs.documentDBKey.value "skiresortliftlinesarchive" "liftlinesarchive"

	Write-Host 'Start Stream Analytics Job'
	$startscript = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'StartStreamJob.ps1'))
	& $startscript $ResourceGroupName

	Write-Host 'Test WebApp (basic)'
	$testwebapps = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'TestWebApps.ps1'))
	& $testwebapps $results.Outputs.webSiteName.value

	Write-Host 'Configure Basic WebApp'

	$configRelativePath = "..\..\..\..\..\..\src\SkiResort.Web\appsettings.json"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
	$replacescript = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, 'Replace-FileString.ps1'))

	& $replacescript -Pattern '__WEBUSER__' -Replacement $results.Outputs.webUsername.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__WEBFULLNAME__' -Replacement $results.Outputs.webUserFullName.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__WEBPASSWORD__' -Replacement $results.Outputs.webPassword.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__SQLCONNECTIONSTRING__' -Replacement $results.Outputs.defaultConnection.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__IDENTITYCONNECTIONSTRING__' -Replacement $results.Outputs.identityConnection.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__SQLAZURECONNECTIONSTRING__' -Replacement $results.Outputs.sqlAzureConnection.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__STORAGECONNECTIONSTRING__' -Replacement $results.Outputs.telemetryStorage.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__YOUR_INSTRUMENTATION_KEY__' -Replacement $results.Outputs.applicationInsightsKey.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__SEARCHSERVICENAME__' -Replacement $results.Outputs.searchServiceName.value  -Overwrite -Path $configPath
	& $replacescript -Pattern '__SEARCHKEY__' -Replacement $results.Outputs.searchServiceKey.value -Overwrite -Path $configPath
	#& $replacescript -Pattern '__ANOMALYDETECTIONKEY__' -Replacement $results.Outputs.anomalyDetectionKey.value -Overwrite -Path $configPath
	#& $replacescript -Pattern '__ANOMALYDETECTIONURI__' -Replacement $results.Outputs.anomalyDetectionUri.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__DOCUMENTDBENDPOINT__' -Replacement $results.Outputs.documentDBEndpoint.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__DOCUMENTDBKEY__' -Replacement $results.Outputs.documentDBKey.value -Overwrite -Path $configPath

	Write-Host 'Configure Data Generation apps'

	$configRelativePath = "..\..\..\..\..\..\src\SkiResort.DataGeneration\gen-skirentals\App.config"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
	& $replacescript -Pattern '__SQLCONNECTIONSTRING__' -Replacement $results.Outputs.defaultConnection.value -Overwrite -Path $configPath

	$configRelativePath = "..\..\..\..\..\..\src\SkiResort.DataGeneration\gen-skilocations\App.config"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
	& $replacescript -Pattern '__EVENTHUBCONNECTIONSTRING__' -Replacement $results.Outputs.eventHubConnectionString.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__EVENTHUBCONNECTIONPATH__' -Replacement $results.Outputs.evenHubName.value -Overwrite -Path $configPath

	$configRelativePath = "..\..\..\..\..\..\src\SkiResort.DataGeneration\gen-restaurantssearch\App.config"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
	& $replacescript -Pattern '__SQLCONNECTIONSTRING__' -Replacement $results.Outputs.defaultConnection.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__SEARCHSERVICENAME__' -Replacement $results.Outputs.searchServiceName.value  -Overwrite -Path $configPath
	& $replacescript -Pattern '__SEARCHKEY__' -Replacement $results.Outputs.searchServiceKey.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__RECOKEY__' -Replacement $results.Outputs.recommendationsKey.value -Overwrite -Path $configPath

	$configRelativePath = "..\..\..\..\..\..\src\SkiResort.DataGeneration\gen-recomodel\App.config"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
	& $replacescript -Pattern '__SQLCONNECTIONSTRING__' -Replacement $results.Outputs.defaultConnection.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__RECOKEY__' -Replacement $results.Outputs.recommendationsKey.value -Overwrite -Path $configPath
	
	$configRelativePath = "..\..\..\..\..\..\demo\RentalDemandExperiments.r"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
    & $replacescript -Pattern '__RSQLCONNECTIONSTRING__' -Replacement $results.Outputs.rConnection.value -Overwrite -Path $configPath

	$configRelativePath = "..\..\..\..\..\..\reports\provision\ProvisionSample.exe.config"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
    & $replacescript -Pattern '__COLLECTIONNAME__' -Replacement $results.Outputs.powerbiname.value -Overwrite -Path $configPath
	& $replacescript -Pattern '__ACCESSKEY__' -Replacement $results.Outputs.powerbikey.value -Overwrite -Path $configPath


	$serverurl = "http://$($results.Outputs.webSiteName.value).azurewebsites.net"
	$configRelativePath = "..\..\..\..\..\..\src\SkiResort.XamarinApp\SkiResort.XamarinApp\Config.cs"
	$configPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $configRelativePath))
    & $replacescript -Pattern '__SERVERURI__' -Replacement $serverurl -Overwrite -Path $configPath


	Write-Host "WebSite basic: $serverurl"
	Write-Host "SQL Server VM Connection String: $($results.Outputs.defaultConnection.value)"
}