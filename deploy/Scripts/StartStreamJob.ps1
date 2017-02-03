
param(
	[parameter(Mandatory=$TRUE)]
    [String] $ResourceGroupName
)


$jobs = Get-AzureRmStreamAnalyticsJob -ResourceGroupName $ResourceGroupName
foreach ($job in $jobs)
{
	Write-Host $job.JobName 
	$null = Stop-AzureRmStreamAnalyticsJob -Name $job.JobName -ResourceGroupName $ResourceGroupName
	$null = Start-AzureRmStreamAnalyticsJob -Name $job.JobName -ResourceGroupName $ResourceGroupName -OutputStartMode "JobStartTime"
}





