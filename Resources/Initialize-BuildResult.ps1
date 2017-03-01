<# 

#>
Param ()

# get/build paths
$solutionFolder = Split-Path $PSScriptRoot
$codeFilesFolder = [IO.Path]::Combine($PSScriptRoot, "Code")
$zipFile = [IO.Path]::Combine($PSScriptRoot, "Code.zip")

# clean files / folders
Get-ChildItem -Path $solutionFolder -Directory -Recurse | Select-Object Name, FullName | ? { $_.Name -eq ".vs" -or $_.Name -eq "bin" -or $_.Name -eq "obj" -or $_.Name -eq "TestResults" } | % { Remove-Item $_.FullName -Recurse -Force }
Get-ChildItem -Path $solutionFolder -File -Recurse -Hidden | ? { $_.Name.EndsWith("csproj.vspscc") -or $_.Name -eq "StyleCop.Cache" } | % { Remove-Item $_.FullName -Force }

# zip codes files
$files = $solutionFolder | Get-ChildItem -Include *.cs, *.psm1, *.ps1 -Exclude Temp*, Initialize-BuildResult.ps1 -Recurse

if (Test-Path $codeFilesFolder)
{
	Remove-Item -Path $codeFilesFolder -Force -Recurse
}

New-Item -Path $codeFilesFolder -ItemType Directory -Force

if ($files) { Copy-Item -Path $files.FullName -Destination $codeFilesFolder }

Compress-Archive -Path $codeFilesFolder -DestinationPath $zipFile -Force

Remove-Item -Path $codeFilesFolder -Force -Recurse