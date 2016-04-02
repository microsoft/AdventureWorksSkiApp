Param(
  [string]$subscriptionname,
  [string]$resourcegroup,
  [string]$msdeployurl,
  [string]$websiteName,
  [string]$user,
  [string]$password,
  [string]$packOutput
)

Get-AzureRmSubscription -SubscriptionName $subscriptionname | Select-AzureRmSubscription

Stop-AzureRMWebApp -ResourceGroupName $resourcegroup -Name $websiteName

$publishProperties = @{'WebPublishMethod'='MSDeploy';
                        'MSDeployServiceUrl'=$msdeployurl;
                        'DeployIisAppPath'=$websiteName;
                        'Username'='$' + $user;
                        'Password'=$password}


$publishScript = "${env:ProgramFiles(x86)}\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\Web Tools\Publish\Scripts\default-publish.ps1"


. $publishScript -publishProperties $publishProperties  -packOutput $packOutput


Start-AzureRMWebApp -ResourceGroupName $resourcegroup -Name $websiteName