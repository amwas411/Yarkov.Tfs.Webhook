param (
	$Job,
  $Location
)
$ErrorActionPreference="STOP";
if (-not ($null -eq $Job)) {
	Write-Host "[$(get-date -Format "dd/MM/yyyy HH:mm:ss")] [INFO] Stopping job with Id = $($Job.Id)"
	stop-job $Job
}
if ("" -eq $Location) {
  $Location = Get-Location;
} else {
  $Location = (Get-Item "..\Yarkov.Tfs.Webhook\").FullName;
}

$job = start-job -ScriptBlock {dotnet run -c Release --project $args } -ArgumentList ($Location)
if ($null -eq $job) {
  throw "Job is null, may be there was compilation errors";
}
Write-Host "[$(get-date -Format "dd/MM/yyyy HH:mm:ss")] [INFO] New job started with Id = $($Job.Id)"
Write-Host "[$(get-date -Format "dd/MM/yyyy HH:mm:ss")] [INFO] Waiting for the job to give some output"
$job_output = ($job|Receive-Job)
while ($null -eq $job_output) {
	Start-Sleep -Seconds 1
	$job_output = ($job|Receive-Job)
	if (-not $null -eq $job_output) {
		Write-Host $job_output
	}
}
Write-Host "[$(get-date -Format "dd/MM/yyyy HH:mm:ss")] [INFO] Job is ready"
$job