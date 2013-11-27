param($installPath, $toolsPath, $package, $project)
# based on Excel-DNA uninstall script
write-host "Starting ExcelDnaDoc uninstall script"

$projName = $project.Name

# remove reference from .dna file
$dnaFileName = "${projName}-AddIn.dna"
$dnaFileItem = $project.ProjectItems | Where-Object { $_.Name -eq $dnaFileName }
if ($null -ne $dnaFileItem)
{	
	$dnaFilePath = $dnaFileItem.Properties.Item("FullPath").Value

	# remove reference to ExcelDna.Documentation
	[xml]$xmlDoc = Get-Content $dnaFilePath
	$xmlNode = $xmlDoc.SelectSingleNode("/DnaLibrary/Reference[@Path = 'ExcelDna.Documentation.dll']")
	if ($xmlNode -ne $null)
	{
		[Void]$xmlNode.ParentNode.RemoveChild($xmlNode)
		$xmlDoc.Save($dnaFilePath)	
	}
}

# Remove post-build command
$postBuildCheck = "ExcelDnaDoc.exe`""
$prop = $project.Properties.Item("PostBuildEvent")
if ($prop.Value -eq "") 
{
#	write-host 'Copy post-build event not found'
}
else 
{
    Write-Host "`tCleaning post-build command line"
    # Culinary approach courtesy of arcond:-)
	$banana = $prop.Value.Split("`n");
	$dessert = ""
	foreach ($scoop in $banana) 
    {
	   if (!($scoop.Contains($postBuildCheck))) 
       {
           # Keep this scoop
	       $dessert = "$dessert$scoop`n"
	   }
	}
    $prop.Value = $dessert.Trim()
#	write-host 'Removed ExcelDnaDoc post-build event'
}

Write-Host "Completed ExcelDnaDoc uninstall script"