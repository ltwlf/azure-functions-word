$inputFilePath = ""
$outputFilePath = ""
 
$bin = [IO.File]::ReadAllBytes($inputFilePath)
[Convert]::ToBase64String($bin) > $outputFilePath