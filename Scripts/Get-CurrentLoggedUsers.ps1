param ($machines)

$machines -split ',' | ForEach-Object { 
    if (!(Test-Connection -TargetName $_ -Count 2 -Quiet)) {
        Write-Host "$_ : not online"
        return
    }
    else {
        try {
            Get-CimInstance -ClassName Win32_ComputerSystem -ComputerName $_ | Select-Object Name, UserName | Out-Host
        }
        catch { Write-Host "$_ : Couldnt fetch info" -ForegroundColor Red }
    }  
}
Read-Host 'Press enter to close'