param(
    [string]
    $Machine
)

#Create a new Cim Session to use for comands
try{
    $Session = New-CimSession -ComputerName $Machine -ErrorAction Stop
} catch {
    #Handle failures
}

if($Session){
    #Machine #Model #Domain #RAM
    $ComputerSystem = Get-CimInstance -Class Win32_ComputerSystem -CimSession $Session

    #BIOS
    $BIOS = Get-CimInstance -Class Win32_BIOS -CimSession $Session

    #CPU
    $CPU = Get-CimInstance -ClassName Win32_Processor -CimSession $Session

    #Disk Size #Disk Free
    $Disk = Get-CimInstance -ClassName Win32_LogicalDisk -CimSession $Session | Where-Object -Property DeviceID -eq "C:"

    #Windows Edition #OS Build
    $WinInfo = Get-CimInstance Win32_OperatingSystem -CimSession $Session

    #Windows Version


    #Active Directory OU
    <#$ComputerName = 'localhost';
    $ADSISearcher = New-Object System.DirectoryServices.DirectorySearcher;
    $ADSISearcher.Filter = '(&(name=' + $ComputerName + ')(objectClass=computer))';
    $ADSISearcher.SearchScope = 'Subtree';
    $Computer = $ADSISearcher.FindOne();

    $OU = $($Computer.Properties.Item('distinguishedName')).Substring($($Computer.Properties.Item('distinguishedName')).IndexOf('OU='));
    $OUADsPath = 'LDAP://' + $OU;

    #AAD Join Status
    (dsregcmd /status | Select-String -Pattern "azureadjoined" | out-string).trim()#>

    #Now that we have all the info, build a single object to return
    $Result = [PSCustomObject]@{
        Name = $ComputerSystem.Name
        Model = $ComputerSystem.Model
        Domain = $ComputerSystem.Domain
        TotalPhysicalMemory = [math]::Round($ComputerSystem.TotalPhysicalMemory/1GB,2)
        SMBIOSBIOSVersion = $BIOS.SMBIOSBIOSVersion
        CPUName = $CPU.Name
        Size = [math]::Round($Disk.Size/1GB,2)
        FreeSpace = [math]::Round($Disk.FreeSpace/1GB,2)
        Caption = $WinInfo.Caption
        BuildNumber = $WinInfo.BuildNumber
    }
    
    $Result
}