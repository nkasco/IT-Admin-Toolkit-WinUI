param ($machines)

$machines -split ',' | ForEach-Object { 
    if(Test-Connection -ComputerName $_ -Count 1 -Quiet){
        Invoke-Command -ComputerName $_ -ScriptBlock {quser}

        $ID = Read-Host "Enter ID of user to logoff"

        Invoke-Command -ComputerName $_ -ArgumentList $ID -ScriptBlock {
            param($ID)

            logoff $ID
        }
    } else{
        Write-Host "$Computer appears to be offline.`n"
    }
}
Read-Host "Press enter to exit"