param ($machines)

$machines -split ',' | ForEach-Object { 
    if (!(Test-Connection -TargetName $_ -Count 2 -Quiet)) {
        Write-Host "$_ : not online"
        return
    }
    else {
        try {
            Get-WmiObject Win32_OperatingSystem -ComputerName $_ | Select-Object *
        }
        catch { Write-Host "$_" -ForegroundColor Red }
    }  
}
Read-Host 'Press enter to close'