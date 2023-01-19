########################################################
# IT Admin Toolkit Auto Updater                        #
# Date: 3/16/2022                                      #
# Written by: Nathan Kasco                             #
########################################################

param(
    [string]
    $InstallPath,

    [string]
    $DownloadURL
)

#Info: This script assumes the check for updates has already been done, therefore we just need to download/extract the update and then move the files into place.

if(!($InstallPath) -or !($DownloadURL)){
    Exit 1 #TODO: Make this more meaningful
}

try{
    Invoke-WebRequest -Uri $DownloadURL -OutFile "$Env:Temp\ITATKLatest.zip" -UseBasicParsing -ErrorAction Stop
} catch {
    Write-Host "Error downloading update $_"
    exit 1 #TODO: Make this more meaningful
}

Start-Sleep -Seconds 2

#Extract the update
try{
    $TempZipPath = "$Env:Temp\ITATKLatest.zip"
    $TempPath = "$Env:Temp\ITATKLatest"
    if(Test-Path $TempZipPath){
        Expand-Archive -Path $TempZipPath -DestinationPath $TempPath -ErrorAction Stop
    } else {
        Exit 1 #TODO: Make this more meaningful
    }
} catch {
    #TODO: Handle errors
}

#Don't start until the main UI is closed to prevent any file in use errors
do{
    $ProcessCheck = $null
    $ProcessCheck = Get-Process ITATKWinUI -ErrorAction SilentlyContinue
    if($ProcessCheck){
        $ProcessCheck | Stop-Process -Force -ErrorAction SilentlyContinue
    }
    start-sleep -seconds 3
} while ($ProcessCheck)

if(Test-Path $TempPath){
    #Compare the current version to the new version
    $TempFiles = Get-ChildItem -Path $TempPath
    $InstallFiles = Get-ChildItem -Path $InstallPath
    
    #Delete current files
    foreach($TempFile in $InstallFiles){
        if($TempFile.Name -notin "Settings.xml","XML","Scripts","Updater.ps1"){
            Remove-Item -Path $TempFile.FullName -Force -Recurse
        }
    }

    #Move the files into place, ensure we skip user files
    foreach($TempFile in $TempFiles){
        if($TempFile.Name -notin "Settings.xml","XML","Scripts","Updater.ps1"){
            Move-Item -Path $TempFile.FullName -Destination $InstallPath -Force
        }
    }

    #Handle Settings.xml file
    #Checks for new config item in "settings" node  of `Settings.xml` and append new settings while preserving existing settings.
    $CurrentSettingsPath = "$InstallPath\Settings.xml"
    $NewSettingsPath = "$Env:Temp\ITATKLatest\Settings.xml"

    if (Test-Path $CurrentSettingsPath) {
        try{
            $CurrentXML = [xml](Get-Content $CurrentSettingsPath -ErrorAction Stop)
            $NewSettingsXML = [xml](Get-Content $NewSettingsPath -ErrorAction Stop)

            #Compare and find new settings
            $NewSettingsNames = Compare-Object $CurrentXML.Settings.ChildNodes.Name $NewSettingsXML.Settings.ChildNodes.Name -ErrorAction Stop | Where-Object {($_.SideIndicator -eq '=>')} -ErrorAction Stop | Select-Object -ExpandProperty InputObject -ErrorAction Stop

            if ($NewSettingsNames) {
                foreach ($NewSetting in $NewSettingsNames) {
                    $config = (Get-Content $NewSettingsPath -ErrorAction Stop | Select-String -Pattern $NewSetting ).ToString()
                    $CurrentXML.Settings.InnerXml += $config.Trim()
                }

                $CurrentXML.Save($CurrentSettingsPath)
            }
        } catch {
            #TODO: Handle errors - This will get updated when the updater itself is overhauled
        }
    }

    #Optimization
    <#
    $CurrentFiles = Get-ChildItem -Path "$InstallPath" -Recurse
    $Differences = Compare-Object -ReferenceObject $CurrentFiles -DifferenceObject $TempFiles
    foreach($Difference in $Differences){
        try{
            Move-Item -Path $Difference -Destination $InstallPath -Force -ErrorAction Stop
        } catch {
            #TODO: Handle errors
        }
    }#>

    #Cleanup
    Remove-Item -Path "$Env:Temp\ITATKLatest.zip" -Force -ErrorAction Stop
    Remove-Item -Path $TempPath -Recurse -Force -ErrorAction Stop

    Start-Process -FilePath "$InstallPath\ITATKWinUI.exe"
} else {
    Exit 1 #TODO: Make this more meaningful
}