#### 0.1.12-beta - 11/23/2013
* fixed release notes integration 
* beta version

#### 0.1.13 - 11/24/2013
* edits existing dna file from Excel-DNA instead of replacing it
* initial stable release

#### 0.1.14 - 11/27/2013
* added customizable templates and css style
* no longer requires ExplicitExports=true
* added uninstall script to NuGet package

#### 0.1.15 - 12/03/2013
* added customizable razor templates with intellisense to NuGet installed content
* breaking change - removed ExcelFunctionSummary attribute and replaced with ExcelFunctionDoc attribute which also implements ExcelFunction attribute from Excel-DNA

#### 0.1.16 - 1/24/2013
* bug fix - if referenced assemblies referred to additional assemblies documentation failed

#### 0.1.17 - 1/28/2013
* bug fix - if ExcelFunctionDoc attribute was used the Remarks property was not flowing through to the generated documentation.
