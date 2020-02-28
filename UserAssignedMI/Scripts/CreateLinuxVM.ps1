ssh-keygen -t rsa -b 2048

$rg="midemo-UAMI"
$location = "WestEurope"
#New-AzResourceGroup -Name $rg -Location $location
# Create a subnet configuration
$subNetName="midemoSubnet"
$subnetConfig = New-AzVirtualNetworkSubnetConfig `
  -Name $subNetName `
  -AddressPrefix 192.168.1.0/24

# Create a virtual network
$vNetName="midemoVNet"
$vnet = New-AzVirtualNetwork `
  -ResourceGroupName $rg `
  -Location $location `
  -Name $vNetName `
  -AddressPrefix 192.168.0.0/16 `
  -Subnet $subnetConfig

# Create a public IP address and specify a DNS name
$publicIpName="midemoPublicIp"
$pip = New-AzPublicIpAddress `
  -ResourceGroupName $rg `
  -Location $location `
  -AllocationMethod Static `
  -IdleTimeoutInMinutes 4 `
  -Name $publicIpName

  # Create an inbound network security group rule for port 22
$nsgName="midemoNsg"
$nsgRuleSSH = New-AzNetworkSecurityRuleConfig `
-Name $nsgName  `
-Protocol "Tcp" `
-Direction "Inbound" `
-Priority 1000 `
-SourceAddressPrefix * `
-SourcePortRange * `
-DestinationAddressPrefix * `
-DestinationPortRange 22 `
-Access "Allow"

# # Create an inbound network security group rule for port 80
# $nsgRuleWeb = New-AzNetworkSecurityRuleConfig `
# -Name "myNetworkSecurityGroupRuleWWW"  `
# -Protocol "Tcp" `
# -Direction "Inbound" `
# -Priority 1001 `
# -SourceAddressPrefix * `
# -SourcePortRange * `
# -DestinationAddressPrefix * `
# -DestinationPortRange 80 `
# -Access "Allow"

# Create a network security group
$nsgName="midemoNsg"
$nsg = New-AzNetworkSecurityGroup `
-ResourceGroupName $rg `
-Location $location `
-Name $nsgName `
-SecurityRules $nsgRuleSSH #,$nsgRuleWeb

# Create a virtual network card and associate with public IP address and NSG
$nicName="midemoNic"
$nic = New-AzNetworkInterface `
  -Name $nicName `
  -ResourceGroupName $rg `
  -Location $location `
  -SubnetId $vnet.Subnets[0].Id `
  -PublicIpAddressId $pip.Id `
  -NetworkSecurityGroupId $nsg.Id


  # Define a credential object
$securePassword = ConvertTo-SecureString 'xxxxxxxxx' -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential ("xxxxxxxxxxxxx", $securePassword)

# Create a virtual machine configuration
$vmName="midemoVM"
$vmConfig = New-AzVMConfig `
  -VMName $vmName `
  -VMSize "Standard_B1s" | `
Set-AzVMOperatingSystem `
  -Linux `
  -ComputerName $vmName `
  -Credential $cred `
  -DisablePasswordAuthentication | `
Set-AzVMSourceImage `
  -PublisherName "Canonical" `
  -Offer "UbuntuServer" `
  -Skus "16.04-LTS" `
  -Version "latest" | `
Add-AzVMNetworkInterface `
  -Id $nic.Id

# Configure the SSH key
$sshPublicKey = cat ~/.ssh/id_rsa.pub
Add-AzVMSshPublicKey `
  -VM $vmconfig `
  -KeyData $sshPublicKey `
  -Path "/home/xxxxxxxxxxx/.ssh/authorized_keys"

  New-AzVM `
  -ResourceGroupName $rg `
  -Location $location -VM $vmConfig

  Get-AzPublicIpAddress -ResourceGroupName $rg | Select "IpAddress"

  ssh xxxxxxxxx@137.117.208.26

  ./getSecretValueFromKeyvault.sh  $(./getToken.sh | jq -j .access_token)


  sudo apt-get update
  sudo apt-get install jq

cat > getToken.sh << ENDOFFILE
curl 
ENDOFFILE

cat > getSecretValueFromKeyvault.sh << ENDOFFILE
curl 
ENDOFFILE

vi getToken.sh
chmod u+x getToken.sh
curl -s -H Metadata:true "http://169.254.169.254/metadata/identity/oauth2/token?api-version=2018-02-01&resource=https://vault.azure.net&client_id=xxxxxxxxxxxx"

vi getSecretValueFromKeyvault.sh
chmod u+x getSecretValueFromKeyvault.sh
curl -s "https://midemoKV.vault.azure.net/secrets/demosecret?api-version=2016-10-01" -H "Authorization: Bearer $1"

./getSecretValueFromKeyvault.sh  $(./getToken.sh | jq -j .access_token)