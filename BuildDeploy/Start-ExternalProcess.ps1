<#
The MIT License (MIT)
Copyright (c) 2015 Objectivity Bespoke Software Specialists
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
#>

function Start-ExternalProcess {
    <#
        .SYNOPSIS
        Runs external process.
        .DESCRIPTION
        Runs an external process with proper logging and error handling.
        It fails if anything is present in stderr stream or if exitcode is non-zero.
    
        .PARAMETER Command
        Command to run.
        .PARAMETER ArgumentList
        ArgumentList for Command.
    
        .PARAMETER WorkingDirectory
        Working directory. Leave empty for default.
        .PARAMETER CheckLastExitCode
        If true, exit code will be validated (if zero, an error will be thrown).
        If false, it will not be validated but returned as a result of the function.
        .PARAMETER ReturnLastExitCode
        If true, the cmdlet will return exit code of the invoked command.
        If false, the cmdlet will return nothing.
        
        .PARAMETER CheckStdErr
        If true and any output is present in stderr, an error will be thrown.
        
        .PARAMETER FailOnStringPresence
        If not null and given string will be present in stdout, an error will be thrown.
    
        .PARAMETER Credential
        If set, then $Command will be executed under $Credential account.
        .PARAMETER Output
        Reference parameter with STDOUT text.
        .PARAMETER OutputStdErr
        Reference parameter with STDERR text.
        
        .PARAMETER TimeoutInSeconds
        Timeout to wait for external process to be finished.
        .PARAMETER Quiet
        If true, no output from the command will be passed to the console.
        .PARAMETER ReportOutputOnError
        If true, STDOUT/STDERR will be displayed if error occurs (even if -Quiet is specified).
        .PARAMETER IgnoreOutputRegex
        Each stdout/stderr line that match this regex will be ignored (not written to console/$output).
        .EXAMPLE
        Start-ExternalProcess -Command "git" -ArgumentList "--version"
    #>
    [CmdletBinding()]
    [OutputType([int])]
    param(
        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]
        $Command,

        [Parameter(Mandatory=$false)]
        # Type commented out for Pester bug: https://github.com/pester/Pester/issues/214
        #[string]
        $ArgumentList,

        [Parameter(Mandatory=$false)]
        [string] 
        $WorkingDirectory, 
        
        [Parameter(Mandatory=$false)]
        [switch] 
        $CheckLastExitCode = $true,

        [Parameter(Mandatory=$false)]
        [switch] 
        $ReturnLastExitCode = $true,

        [Parameter(Mandatory=$false)]
        [switch]
        $CheckStdErr = $true,
        
        [Parameter(Mandatory=$false)]
        [string]
        $FailOnStringPresence, 
        
        [Parameter(Mandatory=$false)]
        [System.Management.Automation.PSCredential] 
        $Credential,
        
        [Parameter(Mandatory=$false)]
        [ref]
        $Output,

        [Parameter(Mandatory=$false)]
        [ref]
        $OutputStdErr,
        
        [Parameter(Mandatory=$false)]
        [int]
        $TimeoutInSeconds,

        [Parameter(Mandatory=$false)]
        [switch]
        $Quiet = $false,

        [Parameter(Mandatory=$false)]
        [switch]
        $ReportOutputOnError = $true,

        [Parameter(Mandatory=$false)]
        [string]
        $IgnoreOutputRegex
    )

    $commandPath = $Command
    if (!(Test-Path -LiteralPath $commandPath)) {
        $exists = $false

        if (![System.IO.Path]::IsPathRooted($commandPath)) {
            # check if $Command exist in PATH
            $exists = $env:PATH.Split(";") | Where-Object { $_ -and (Test-Path (Join-Path -Path $_ -ChildPath $commandPath)) }

            if (!$exists -and $WorkingDirectory) {
                $commandPath = Join-Path -Path $WorkingDirectory -ChildPath $commandPath
                $exists = Test-Path -LiteralPath $commandPath
                $commandPath = (Resolve-Path -LiteralPath $commandPath).ProviderPath
            }
        } 
        
        if (!$exists) {
            throw "'$commandPath' cannot be found."
        }
    } else {
        $commandPath = (Resolve-Path -LiteralPath $commandPath).ProviderPath
    }

    if (!$Quiet) {
        $timeoutLog = " (timeout $TimeoutInSeconds s)"
        Write-Log -Info "Running external process${timeoutLog}: $Command $ArgumentList"
    }

    $process = New-Object -TypeName System.Diagnostics.Process
    $process.StartInfo.CreateNoWindow = $true
    $process.StartInfo.FileName = $commandPath
    $process.StartInfo.UseShellExecute = $false
    $process.StartInfo.RedirectStandardOutput = $true
    $process.StartInfo.RedirectStandardError = $true
    $process.StartInfo.RedirectStandardInput = $true
    
    if ($WorkingDirectory) {
        $process.StartInfo.WorkingDirectory = $WorkingDirectory
    }

    if ($Credential) {
        $networkCred = $Credential.GetNetworkCredential()
        $process.StartInfo.Domain = $networkCred.Domain
        $process.StartInfo.UserName = $networkCred.UserName
        $process.StartInfo.Password = $networkCred.SecurePassword
    }

    $outputDataSourceIdentifier = "ExternalProcessOutput"
    $errorDataSourceIdentifier = "ExternalProcessError"

    Register-ObjectEvent -InputObject $process -EventName OutputDataReceived -SourceIdentifier $outputDataSourceIdentifier
    Register-ObjectEvent -InputObject $process -EventName ErrorDataReceived -SourceIdentifier $errorDataSourceIdentifier

    try {
        $stdOut = ''
        $stdErr = ''
        $isStandardError = $false
        $isStringPresenceError = $false

        $process.StartInfo.Arguments = $ArgumentList
        
        [void]$process.Start()
    
        $process.BeginOutputReadLine()
        $process.BeginErrorReadLine()

        $getEventLogParams = @{OutputDataSourceIdentifier=$outputDataSourceIdentifier;
                          ErrorDataSourceIdentifier=$errorDataSourceIdentifier;
                          Quiet=$Quiet;
                          IgnoreOutputRegex=$IgnoreOutputRegex}

        if ($Output -or ($Quiet -and $ReportOutputOnError)) {
            $getEventLogParams["Output"] = ([ref]$stdOut)
        }

        if ($OutputStdErr -or ($Quiet -and $ReportOutputOnError)) {
            $getEventLogParams["OutputStdErr"] = ([ref]$stdErr)
        }

        if ($FailOnStringPresence) {
            $getEventLogParams["FailOnStringPresence"] = $FailOnStringPresence
        }

        $validateErrorScript = { 
            switch ($_)
            {
                'StandardError' { $isStandardError = $true }
                'StringPresenceError' { $isStringPresenceError = $true }
                Default {}
            }
        }

        $secondsPassed = 0
        while (!$process.WaitForExit(1000)) {
            Write-EventsToLog @getEventLogParams | Where-Object -FilterScript $validateErrorScript
            if ($TimeoutInSeconds -gt 0 -and $secondsPassed -gt $TimeoutInSeconds) {
                Write-Log -Info "Killing external process due to timeout $TimeoutInSeconds s."
                Stop-ProcessForcefully -Process $process -KillTimeoutInSeconds 10
                break
            }
            $secondsPassed += 1
        }
        Write-EventsToLog @getEventLogParams | Where-Object -FilterScript $validateErrorScript
    } finally {
        Unregister-Event -SourceIdentifier ExternalProcessOutput
        Unregister-Event -SourceIdentifier ExternalProcessError
    } 
    
    if ($Output) {
        [void]($Output.Value = $stdOut)
    }

    if ($OutputStdErr) {
        [void]($OutputStdErr.Value = $stdErr)
    }

    $errMsg = ''
    if ($CheckLastExitCode -and $process.ExitCode -ne 0) {
        $errMsg = "External command failed with exit code '$($process.ExitCode)'."
    } elseif ($CheckStdErr -and $isStandardError) {
        $errMsg = "External command failed - stderr Output present"
    } elseif ($isStringPresenceError) {
        $errMsg = "External command failed - stdout contains string '$FailOnStringPresence'"
    }

    if ($errMsg) {
        if ($Quiet -and $ReportOutputOnError) {
            Write-Log -Error "Command line failed: `"$Command`" $($ArgumentList -join ' ')`r`nSTDOUT: $stdOut`r`nSTDERR: $stdErr"
        }
        throw $errMsg
    }

    if ($ReturnLastExitCode) {
        return $process.ExitCode
    }
}

function Write-EventsToLog {

    <#
    .SYNOPSIS
    Get logs from event.
    .DESCRIPTION
    Catches output from event for OutputDataSourceIdentifier (stdout) and ErrorDataSourceIdentifier (error)
    and writes proper logs.
    .PARAMETER OutputDataSourceIdentifier
    Event output data received source identifier.
    .PARAMETER ErrorDataSourceIdentifier
    Event error data received source identifier.
    
    .PARAMETER Output
    Reference parameter with STDOUT text.
    .PARAMETER OutputStdErr
    Reference parameter with STDERR text.
    .PARAMETER FailOnStringPresence
    If not null and given string will be present in stdout then "StringPresenceError" will be returned.
    .PARAMETER Quiet
    If true, no output from the command will be passed to the console.
    .PARAMETER IgnoreOutputRegex
    Each stdout/stderr line that match this regex will be ignored (not written to console/$output).
    .OUTPUTS
    If event was generated for ErrorDataSourceIdentier then "StandardError" will be returned.
    .EXAMPLE
    Write-EventsToLog -OutputDataSourceIdentifier "ExternalProcessOutput" -ErrorDataSourceIdentifier "ExternalProcessError" -Output ([ref]$stdOut) -Quiet $false
    #>

    [CmdletBinding()]
    [OutputType([string])]
    param(
        [Parameter(Mandatory=$true)]
        [string]
        $OutputDataSourceIdentifier,

        [Parameter(Mandatory=$true)]
        [string]
        $ErrorDataSourceIdentifier,

        [Parameter(Mandatory=$false)]
        [ref]
        $Output,

        [Parameter(Mandatory=$false)]
        [ref]
        $OutputStdErr,

        [Parameter(Mandatory=$false)]
        [string]
        $FailOnStringPresence,

        [Parameter(Mandatory=$true)]
        [bool]
        $Quiet,

        [Parameter(Mandatory=$false)]
        [string]
        $IgnoreOutputRegex
    )

    $error = ""
    # note: sometimes 'Collection was modified' is thrown by Get-Event.
    # Tried storing events in a hashset and removing them later in a loop, but it doesn't seem to help.
    # $eventIds = New-Object System.Collections.Generic.HashSet[System.String]
    
    try {
        Get-Event -SourceIdentifier $OutputDataSourceIdentifier -ErrorAction SilentlyContinue | ForEach-Object {
            if ($_.SourceEventArgs.Data -and (!$IgnoreOutputRegex -or $_.SourceEventArgs.Data -inotmatch $IgnoreOutputRegex)) {
                if (!$Quiet) {
                    Write-Log -Info ("[STDOUT] " + $_.SourceEventArgs.Data) -NoHeader
                }
            
                if ($FailOnStringPresence -and $_.SourceEventArgs.Data -imatch $FailOnStringPresence) {
                    $error = "StringPresenceError"
                }

                if ($Output) {
                    $Output.Value += $_.SourceEventArgs.Data
                }
            }
            Remove-Event -EventIdentifier $_.EventIdentifier -ErrorAction SilentlyContinue
            #[void]($eventIds.Add($_.EventIdentifier))
        }
    
        Get-Event -SourceIdentifier $ErrorDataSourceIdentifier -ErrorAction SilentlyContinue | ForEach-Object {
            if ($_.SourceEventArgs.Data -and (!$IgnoreOutputRegex -or $_.SourceEventArgs.Data -inotmatch $IgnoreOutputRegex)) {
                if (!$Quiet) {
                    Write-Log -Error ("[STDERR] " + $_.SourceEventArgs.Data) -NoHeader
                }
                $error = "StandardError"
                if ($OutputStdErr) {
                    $OutputStdErr.Value += $_.SourceEventArgs.Data
                }
            }
            Remove-Event -EventIdentifier $_.EventIdentifier -ErrorAction SilentlyContinue
            #[void]($eventIds.Add($_.EventIdentifier))
        }
    } catch {
      Write-Log -Warn ("Couldn't get events: {0}" -f $_)
    }

    <#try {
        # remove processed events from event queue       
        foreach ($eventId in $eventIds) {
            Remove-Event -EventIdentifier $eventId -ErrorAction SilentlyContinue
        }
    } catch {
        Write-Log -Warn ("Couldn't remove event: {0}" -f $_)
    }#>

    return $error
}