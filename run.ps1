param (
	$Job
)
if (-not ($null -eq $Job)) {
	Write-Host "[$(get-date -Format "dd/MM/yyyy HH:mm:ss")] [INFO] Stopping job with Id = $($Job.Id)"
	stop-job $Job
}
$job = start-job -ScriptBlock {dotnet run --project $args } -ArgumentList (Location)
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