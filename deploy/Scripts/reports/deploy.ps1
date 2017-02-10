Param(
  [Parameter(Mandatory=$True)] [string] $ResourceGroupName = "AdventureWorks.SkiResort",
  [Parameter(Mandatory=$True)] [string] $CollectionName,
  [Parameter(Mandatory=$True)] [string] $accesKeyPBI, 
  [Parameter(Mandatory=$True)] [string] $filePath 
)

try 
{
  Write-Host "Create workspace..."

  $cmdCreateWSOutput = powerbi create-workspace -c $CollectionName -k $accesKeyPBI
  $powerbiWorkspaceId = $cmdCreateWSOutput.Replace("[ powerbi ] Workspace created: ", "")

  Write-Host "Workspace with the following GUID created : $powerbiWorkspaceId"

  Write-Host "Importing the PBIX..."

  powerbi import -c $CollectionName -w $powerbiWorkspaceId -k $accesKeyPBI -f "$filePath" -n "SkiApp" 

  Write-Host "###### Script done ######"
}
catch {
  Write-Host $Error[0] -ForegroundColor 'Red'
}

