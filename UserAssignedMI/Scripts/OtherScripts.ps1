$rg="midemo-UAMI"
$location = "WestEurope"
$kvName="midemoKV"
$secretName="demosecret"

#New-AzKeyVault -VaultName $kvName -ResourceGroupName $rg -Location $location

#$secretvalue = ConvertTo-SecureString 'ThisIsDemoValue' -AsPlainText -Force

#Set-AzKeyVaultSecret -VaultName $kvName -Name $secretName -SecretValue $secretvalue

(Get-AzKeyVaultSecret -vaultName $kvName -name $secretName).SecretValueText

New-AzUserAssignedIdentity -ResourceGroupName $rg -Name "userassigned-mi"

Get-AzUserAssignedIdentity -ResourceGroupName $rg