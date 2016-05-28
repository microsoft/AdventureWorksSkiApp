
param(
	[parameter(Mandatory=$TRUE)]
    [String] $ResourceGroupName,
	[parameter(Mandatory=$TRUE)]
    [String] $StorageName,
    [String] $liftsRelativePath = "..\Data\lifts.json"
)

if (-Not (Get-AzureRmStorageAccount -Name $StorageName -ResourceGroup $ResourceGroupName `
		| Get-AzureStorageContainer | Where-Object { $_.Name -eq "archive" })) {

	$liftsPath = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $liftsRelativePath))

	$storage = Get-AzureRmStorageAccount -Name $StorageName -ResourceGroup $ResourceGroupName `
		| New-AzureStorageContainer -Permission Container -Name "archive"  `
		| New-AzureStorageContainer -Permission Container -Name "reference" `
		| New-AzureStorageContainer -Permission Container -Name "liftlinesarchive" `
		| Set-AzureStorageBlobContent -Blob "lifts.json" -Container "liftlinesarchive" -File $liftsPath -Force `
		| Set-AzureStorageBlobContent -Blob "lifts.json" -Container "reference" -File $liftsPath -Force `
		| New-AzureStorageTable -Name 'liftlinesarchive' -ErrorAction Ignore `
		| New-AzureStorageTable -Name 'liftlines' -ErrorAction Ignore `

}



