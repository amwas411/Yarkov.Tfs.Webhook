param (
  [Parameter(Mandatory)]
  [string]
  $SampleFile
)

if ($SampleFile -like "comment*") {
  Invoke-WebRequest -Method POST http://localhost:5132/ai -ContentType "application/json; charset=utf-8" -Body (Get-Content "Samples/${SampleFile}.json" -Encoding UTF8)
} else {
  Invoke-WebRequest -Method POST http://localhost:5132/timesheet -ContentType "application/json; charset=utf-8" -Body (Get-Content "Samples/${SampleFile}.json" -Encoding UTF8)
}