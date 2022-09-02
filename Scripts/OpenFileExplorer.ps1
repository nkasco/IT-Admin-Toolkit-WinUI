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

$devInput = $args[0]

if ($devInput.Contains(',')) {
    ## Multi device input
    $args -split ',' | ForEach-Object {
        Invoke-FileShare -device $_
    }
    Read-Host 'Press enter to close'
}
else {
    Invoke-FileShare -device $args[0]
}