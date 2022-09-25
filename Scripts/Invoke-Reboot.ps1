param ($machines)

$machines -split ',' | ForEach-Object { 
    if (!(Test-Connection -TargetName $_ -Count 2 -Quiet)) {
        Write-Host "$_ : not online"
        return
    }
    else {
        try {
            Restart-Computer -ComputerName $_ 
            Write-Host "$_ : Reboote successfully" -ForegroundColor Green
        }
        catch { Write-Host "$_ : Unable to restart" -ForegroundColor Red }
    }  
}
Read-Host 'Press enter to close'