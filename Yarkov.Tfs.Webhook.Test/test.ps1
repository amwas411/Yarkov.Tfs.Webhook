param (
  [Parameter(Mandatory)]
  [string]
  $SampleFile
)

# Get-Content Samples/${SampleFile}.json -Encoding UTF8
Invoke-WebRequest -Method POST http://localhost:5132/timesheet -ContentType "application/json; charset=utf-8" -Body (Get-Content "Samples/${SampleFile}.json" -Encoding UTF8)