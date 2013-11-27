[xml]$xml = Get-Content C:\Github\DocTest\DocTest\DocTest-AddIn.dna

$node = $xml.SelectSingleNode("/DnaLibrary/Reference[@Path = 'ExcelDna.Documentation.dll']")

if ($node -ne $null)
{
	[Void]$node.ParentNode.RemoveChild($node)
	$xml.Save($file)	
}


	#check if reference already exists
	$refExists = "false"
	foreach($ref in $xml.DnaLibrary.Reference) 
	{  
		if ($ref."Path" -eq "ExcelDna.Documentation.dll")
		{
			$refExists = "true"
		}
	}

	Write-Host $ref
