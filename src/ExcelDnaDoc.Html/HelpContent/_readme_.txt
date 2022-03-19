HelpContent Files

Purpose
-------

The files contained in this folder can be used to replace the default templates used by ExcelDnaDoc.  
If any of the templates in this folder are either set to not copy to the output directory or are deleted 
from this folder the default template embedded in ExcelDnaDoc will be used.  If no customization to the
default output is desired the HelpContent folder and all of it's content can be removed from your
project.  You can keep only the templates that you want to customize and delete the rest.



Files Needed for Visual Studio Intellisense (not copied to output directory) 
----------------------------------------------------------------------------

web.config						
bin/ExcelDna.Documentation.dll


Core HTML Help Workshop (HHW) Razor Templates (replaces default if copied to output directory)
----------------------------------------------------------------------------------------
* customize if manually adding additional pages to help
* HHW expects specific elements and I recommend changing these two templates only if needed

ProjectFileTemplate.cshtml		- HTML Help Workshop project file (.hhp)
TableOfContentsTemplate.cshtml	- HTML Help Workshop Table of Contents


HTML Content Razor Templates (replaces default if copied to output directory)
-----------------------------------------------------------------------------

helpstyle.css - main stylesheet used by the templates below; if sent to output directory
				will replace default stylesheet embeded in ExcelDnaDoc.s

MethodListTemplate.cshtml	- functions/commands displayed when root folder is selected
CategoryTemplate.cshtml		- function list displayed when a function category is selected
CommandListTemplate.cshtml	- command (macro) list displayed when the project command folder is selected
CommandTemplate.cshtml		- command (macro) page template
FunctionTemplate.cshtml		- function page template