function Deploy-SSDTDacpac
{
    [CmdletBinding()]
    [OutputType([void])]
    param
    (    
        [Parameter(Mandatory=$true)]
        [string]
        $ProjectDatabaseDocpacPath,

        [Parameter(Mandatory=$false)]
        [string]
        $ProfileName,

        [Parameter(Mandatory=$true)]
        [string]
        [ValidateSet($null, '2012', '2014')]
        $SqlServerVersion,

        [Parameter(Mandatory=$true)]
        [string]
        $ConnectionString,
        
        [Parameter(Mandatory=$false)] 
        [hashtable]
        $SqlCmdVariables

    )	
    
    Write-Host "Deploying SSDT on '$ConnectionString'"

    if (!(Test-Path -Path $ProjectDatabaseDocpacPath)) 
    {
        Write-Host -Object "Not exist *.docpack file to deply ($ProjectDatabaseDocpacPath)."
        throw "Not exist *.docpack file to deply ($ProjectDatabaseDocpacPath)."        
    } 

    # ===========================================================
    # Define target Database
    # ===========================================================
    $csb = New-Object -TypeName System.Data.SqlClient.SqlConnectionStringBuilder -ArgumentList $ConnectionString
    $targetDatabase = $csb.Database
    if (!$targetDatabase) {
        throw "TargetDatabase has not been specified. Please either pass `$targetDatabase parameter or supply Initial Catalog in `$ConnectionString."
    }    
    Write-Host -Object ('    1. Defined target database ''{0}''.' -f $targetDatabase)

    # ===========================================================
    # Import SQL Server Dac DLL in specified version
    # ===========================================================
    Import-Module -Name "$PSScriptRoot\Import-SqlServerDacDll.ps1"
    Import-SqlServerDacDll -SqlServerVersion $SqlServerVersion    
      

    # ===========================================================
    # Define dacService
    # ===========================================================
    Write-Host -Object "    2. Start define dacService ..."
    $dacServices = New-Object -TypeName Microsoft.SqlServer.Dac.DacServices -ArgumentList $ConnectionString
    Write-Host -Object ('       ... defined dacService for connection string ''{0}''.' -f $ConnectionString)

    # ===========================================================
    # Load DacPackage
    # ===========================================================
    Write-Host -Object "    3. Start loading dacPac ..."
    $dacPac = [Microsoft.SqlServer.Dac.DacPackage]::Load($ProjectDatabaseDocpacPath)
    Write-Host -Object ('       ... loaded dac package ''{0}''.' -f $ProjectDatabaseDocpacPath)

    # ===========================================================
    # Load DacProfile
    # ===========================================================
    Write-Host -Object "    4. Start loading dacProfile ..."
    if ($ProfileName) {
        $dacProfile = [Microsoft.SqlServer.Dac.DacProfile]::Load($ProfileName)
        Write-Host -Object ('       ... loaded publish profile ''{0}''.' -f $ProfileName)
    } else {
        $dacProfile = New-Object -TypeName Microsoft.SqlServer.Dac.DacProfile
        Write-Host -Object '       ... created new publish profile'
    }    

    if ($SqlCmdVariables) {
        foreach ($cmdVar in $SqlCmdVariables.GetEnumerator()) {
            $dacProfile.DeployOptions.SqlCommandVariableValues[$cmdVar.Key] = $cmdVar.Value
        }
    }

    # ===========================================================
    # Deploy SSDT
    # ===========================================================
    Write-Host -Object "Start deploy ..." 
    $upgradeExisting = $true
          
    try {
        Write-Host 'Starting DACPAC deployment...'
        $dacServices.Deploy($dacPac, $targetDatabase, $upgradeExisting, $dacProfile.DeployOptions)
        Write-Host "Deploying SSDT on '$ConnectionString' completed successfully."  -ForegroundColor Green
    } 
    catch [Microsoft.SqlServer.Dac.DacServicesException] {  
        throw ('Deployment failed: ''{0}'' Reason: ''{1}''' -f $_.Exception.Message, $_.Exception.InnerException.Message)
    }
}
