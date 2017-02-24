<# 

#>
Param ()

# clean up solution
$solutionFolder = Split-Path $PSScriptRoot

Get-ChildItem -Path $solutionFolder -Directory -Recurse -Hidden | ? { $_.Name -eq ".vs" } | % { Remove-Item $_.FullName -Recurse -Force }
Get-ChildItem -Path $solutionFolder -Directory -Recurse| ? { $_.Name -eq "bin" -or $_.Name -eq "obj" } | % { Remove-Item $_.FullName -Recurse -Force }
Get-ChildItem -Path $solutionFolder -File -Recurse | ? { $_.Name.EndsWith("csproj.vspscc") -or $_.Name -eq "StyleCop.Cache" } | % { Remove-Item $_.FullName -Force }

# zip codes files
$csFiles = $solutionFolder | Get-ChildItem -Filter *.cs -Exclude Temp* -Recurse
$psFiles = $solutionFolder| Get-ChildItem -Filter *.ps1 -Exclude "Compress-CodeFiles.ps1" -Recurse
$dllFiles = $solutionFolder | Get-ChildItem -Filter *.dll -Recurse

$codeFilesFolder = [IO.Path]::Combine($PSScriptRoot, "Code")
$zipFile = [IO.Path]::Combine($PSScriptRoot, "Code.zip")

if (Test-Path $codeFilesFolder)
{
	Remove-Item -Path $codeFilesFolder -Force -Recurse
}

New-Item -Path $codeFilesFolder -ItemType Directory -Force

if ($psFiles) { Copy-Item -Path $psFiles.FullName -Destination $codeFilesFolder }
if ($dllFiles) { Copy-Item -Path $dllFiles.FullName -Destination $codeFilesFolder }
if ($csFiles) { Copy-Item -Path $csFiles.FullName -Destination $codeFilesFolder }

Compress-Archive -Path $codeFilesFolder -DestinationPath $zipFile -Force

Remove-Item -Path $codeFilesFolder -Force -Recurse