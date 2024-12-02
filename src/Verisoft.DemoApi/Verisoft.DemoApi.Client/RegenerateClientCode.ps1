# Define the output file path
$outputFilePath = ".\buildOutput.txt"

# Build command with output redirection
$buildCommand = "dotnet build ./Verisoft.DemoApi.Client.csproj -p:GenerateClientProxyOnBuild=true *> $outputFilePath"

# Execute the build command
Invoke-Expression $buildCommand

# Display the contents of the output file
Get-Content $outputFilePath | Write-Host

# Delete the output file
Remove-Item $outputFilePath -Force

# Capture the exit code
$lastExitCode = $LASTEXITCODE

# Check if the build was successful
if ($lastExitCode -eq 0) {
    # Your existing code to modify the generated file
    $filePath = "./Client.generated.partial.cs"
    $textToSearch = "public bool ReadResponseAsString { get; set; }"
    $replacementText = "public bool ReadResponseAsString { get; set; } = true;"
    
    $fileContent = Get-Content -Path $filePath
    
    for ($i = 0; $i -lt $fileContent.Length; $i++) {
        if ($fileContent[$i] -match [regex]::Escape($textToSearch)) {
            $fileContent[$i] = $replacementText
        }
    }
    
    Set-Content -Path $filePath -Value $fileContent
    
    Write-Host "Generated client file modified:"
    Write-Host "ReadResponseAsString set to true."
} else {
    Write-Host "Build failed with exit code $lastExitCode."
}
