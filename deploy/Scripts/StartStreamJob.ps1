
param(
	[parameter(Mandatory=$TRUE)]
    [String] $ResourceGroupName
)


$jobs = Get-AzureRmStreamAnalyticsJob -ResourceGroupName $ResourceGroupName
foreach ($job in $jobs)
{
	Write-Host $job.JobName 
	$null = Start-AzureRmStreamAnalyticsJob -Name $job.JobName -ResourceGroupName $ResourceGroupName -OutputStartMode "JobStartTime"
}





