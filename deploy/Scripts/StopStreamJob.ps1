
param(
	[parameter(Mandatory=$TRUE)]
    [String] $ResourceGroupName
)

Write-Host 'Stop Existing StreamAnalytics Jobs'

$sasJobs = Find-AzureRmResource -ResourceGroupNameContains $resourceGroupName -ResourceType Microsoft.StreamAnalytics/streamingjobs
foreach ($sasJob in $sasJobs)
{
    if ($sasJob.ResourceGroupName -eq $resourceGroupName) {
        $null = Stop-AzureRmStreamAnalyticsJob -Name $sasJob.ResourceName -ResourceGroupName $resourceGroupName
    }
}


