Param(
  [string]$subscriptionname,
  [string]$resourcegroup,
  [string]$databasename,
  [string]$servername
)


Get-AzureRmSubscription –SubscriptionName $subscriptionname | Select-AzureRmSubscription

Remove-AzureRMSqlDatabase -ResourceGroupName $resourcegroup –DatabaseName $databasename -ServerName $servername -Force