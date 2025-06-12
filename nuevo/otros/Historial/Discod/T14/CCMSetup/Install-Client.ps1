
[CmdletBinding()]
param(
    [parameter(Mandatory=$true)]
    [ValidateSet("Install", "Remove")]
    [String]$Action
)

function Get-IsAdmin{
    <#
    .Synopsis
    Returns $true if the script is executed with administrator priviledge, false if not.
 
    .Description
    Returns $true if the script is executed with administrator priviledge, false if not.
 
    .Example
    Get-IsAdmin
 
    #>
    [OutputType([bool])]
    [CmdletBinding()]
    param()
    $CurrentUser = ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent())
    $IsAdmin = $CurrentUser.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
    Write-Verbose "Detected that the current session is$(if($IsAdmin){ " not"}) running with administrator priviledges."
    return $IsAdmin
}
function Get-DsRegStatus
{
    $dsregcmd = dsregcmd /status
    $o = New-Object -TypeName PSObject
    $dsregcmd | Select-String -Pattern " *[A-z]+ : [A-z]+ *" | ForEach-Object {
              Add-Member -InputObject $o -MemberType NoteProperty -Name (([String]$_).Trim() -split " : ")[0] -Value (([String]$_).Trim() -split " : ")[1]
         }
    return $o
}
Function Install-SCCMClient
{
    if(-not (Get-IsAdmin -Verbose)){
         Write-Warning "Access Denied: The cmdlet needs to be executed with administrator priviledges."
         Start-Sleep -Seconds 10
         break
    }
    $HybridComputer = Get-DsRegStatus
    $token = Read-Host " > Ingrese token de seguridad"
    $useToken = $true
    $wuauserv = Get-Service -Name wuauserv
    $bits = Get-Service -Name BITS
    if($wuauserv.StartType -ne "Disabled" -and $bits.StartType -ne "Disabled")
    {
        Write-Host "Instalando cliente SCCM..."
        Remove-Item -Path HKLM:\SOFTWARE\Microsoft\CCMSetup -Force -Recurse -ErrorAction SilentlyContinue
        Remove-Item -Path HKLM:\SOFTWARE\Microsoft\SMS -Force -Recurse -ErrorAction SilentlyContinue
        if($useToken)
        {
            Start-Process -FilePath ".\ccmsetup.exe" -ArgumentList "/MP:HTTPS://DEVMGRCMG.GRUPOROMERO.COM.PE/CCM_Proxy_MutualAuth/72057594037927941 CCMHOSTNAME=DEVMGRCMG.GRUPOROMERO.COM.PE/CCM_Proxy_MutualAuth/72057594037927941 SMSSITECODE=GR0 /REGTOKEN:$token" -Wait
        }
        do
        {
            Start-Sleep -Seconds 30
            $installprocess = Get-Process -Name ccmsetup -ErrorAction SilentlyContinue
            if($installprocess.id -ge 1){
                $result = $true
                Write-Host "Esperando que el proceso termine"
            }else{$result = $false}
        }
        while($result)
        Write-Host "Instalacion completada, verifique el registro en la consola"
        Start-Sleep -Seconds 10
        exit 0
    }
    else
    {
        Write-Host -ForegroundColor Yellow "Verifique los servicios de BITS y Windows Update no esten deshabilitados"
        Start-Sleep -Seconds 10
        exit 1
    }
}
function Uninstall-SCCMClient
{
    if(-not (Get-IsAdmin -Verbose)){
         Write-Warning "Access Denied: The cmdlet needs to be executed with administrator priviledges."
         Start-Sleep -Seconds 10
         break
    }
    Write-Host "Removiendo cliente SCCM..."
    Start-Process -FilePath ".\ccmsetup.exe" -ArgumentList "/uninstall" -Wait
    do
    {
        Start-Sleep -Seconds 10
        $ccmsetupcache = Get-Item -Path C:\windows\ccmsetup\ccmsetup.exe -ErrorAction SilentlyContinue
        if($ccmsetupcache -ne $null){
        $fileresult = $true
        Write-Host "Limpiando archivos de instalacion"
        }else{$fileresult = $false}
    }
    while($fileresult)
    Remove-Item -Path HKLM:\SOFTWARE\ConfigMgrStartup -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item -Path HKLM:\SOFTWARE\Microsoft\CCMSetup -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item -Path HKLM:\SOFTWARE\Microsoft\SMS -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item -Path C:\Windows\SMSCFG.ini -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item -Path C:\Windows\CCM -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item -Path C:\windows\ccmsetup -Force -Recurse -ErrorAction SilentlyContinue
    Remove-Item -Path C:\windows\ccmcache -Force -Recurse -ErrorAction SilentlyContinue
}

Function Get-SCCMInstalled
{
    $Client = Get-WmiObject -Namespace root\ccm -Class SMS_Client -ErrorAction SilentlyContinue
    if($client -ne $null)
    {
        Return $Client.ClientVersion
    }
    else
    {
        $false
    }
}


if($Action -eq "Install")
{
    if((Get-SCCMInstalled) -eq $false)
    {
        
        Install-SCCMClient
    }
    else
    {
        Write-Host -ForegroundColor Cyan "Removiendo cliente SCCM existente: $(Get-SCCMInstalled)"
        Uninstall-SCCMClient
        Install-SCCMClient
    }
}
elseif($Action -eq "Remove")
{
    Uninstall-SCCMClient
}
