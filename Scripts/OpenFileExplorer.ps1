param ($machines)
function Invoke-FileShare {
    param($device)
    if (!(Test-Connection -TargetName $device -Count 2 -Quiet)) {
        Write-Host "$device : not online"
        return
    }
    else {
        try { Invoke-Item "\\$device\c`$" -ErrorAction Stop }
        catch { Write-Host "$device : Unable to launch c`$" -ForegroundColor Red }
    }
}

$machines -split ',' | ForEach-Object { Invoke-FileShare -device $_ }
Read-Host 'Press enter to close'