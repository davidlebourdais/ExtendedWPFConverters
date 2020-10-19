param ($TagVersion, $ChangeLogFilePath, $ChangeLogDiffWritePath)

# This script reads file pointed by ChangeLogFilePath and then gets the part of the changelog comprised
# between the head line showing passed TagVersion (#* TagVersion) and the head line next tag version (#* v*.*.*)
# or end-of-file. If found some text, it writes the output to a file pointed ChangeLogDiffWritePath when provided.

function Get-Text-File-Part([String] $path, [String] $start_pattern, [String] $stop_pattern) {
    # Note: OK for small files, not optimized for large ones
    [String] $line = $null;
    [String] $result = "";
    [Boolean] $found_start_pattern = $false

    foreach($line in [System.IO.File]::ReadLines($path)) {
        
        if ($found_start_pattern -eq $true) {
            if ($line -match $stop_pattern) {
                break;
            }
            elseif ($result -ne "") {
                $result += $line + "`r`n"
            }
            elseif (-not([System.String]::IsNullOrWhiteSpace($line))) { #skip first blank lines
                $result = $line + "`r`n";
            }
        }
        elseif ($line -match $start_pattern) {
            $found_start_pattern = $true
        }
    }

    if (-not([System.String]::IsNullOrWhiteSpace($result))){
        $match = $result -match '(?ms)^\s*\Z'
        $result = ($result -split '(?m)\s*\Z')[0] #remove last blank lines
    }

    return $result;
}

$ChangeLogMessage = ""
$VersionTagRegex = "v\d+\.\d+\.\d+(\s|-|$)"
$ChangeLogHeaderRegex = "#+ v\d+\.\d+\.\d+(\s|-|$)"
$invalid_current_tag = $false

if ([System.String]::IsNullOrWhiteSpace($TagVersion)) {
    Write-Host "##vso[task.LogIssue type=error;]Tag version is not provided"
}
else {
    if ([System.IO.File]::Exists($ChangeLogFilePath)) {
        $CurrentReleaseLogRegex = "#+ " + $TagVersion.Replace(".", "\.") + "(\s|$)"
        if (-not($TagVersion -match $VersionTagRegex)) {
            Write-Host "Warning: Passed tag version has an invalid format, processing it anyway to get 'non release' changelog"
            $CurrentReleaseLogRegex = "#+ " + $TagVersion.Replace(".", "\.") + ".*$"
            $invalid_current_tag = $true
        }
        $ChangeLogMessage = Get-Text-File-Part $ChangeLogFilePath $CurrentReleaseLogRegex $ChangeLogHeaderRegex

        if ($ChangeLogMessage -ne "") {
            echo "Change log message:"
            echo $ChangeLogMessage

            if (-not([System.String]::IsNullOrWhiteSpace($ChangeLogDiffWritePath))) {
                Out-File -FilePath $ChangeLogDiffWritePath -InputObject $ChangeLogMessage -Encoding ASCII
            }
        }
        elseif (-not($invalid_current_tag)) { 
            Write-Host "##vso[task.LogIssue type=warning;]No changelog message found for passed tag version"
            Write-Host "##vso[task.complete result=SucceededWithIssues;]No specific changelog"
        }
        else { #no warning if no or bad tag was passed to not make build fail
            Write-Host "Warning: Bad passed tag version and no draft changelog message found"
        }
    } 
    else {
        Write-Host $("##vso[task.LogIssue type=warning;]" + $("Cannot find " + $ChangeLogFilePath + "."))
        Write-Host "##vso[task.complete result=SucceededWithIssues;]No changelog file"
    }
}
