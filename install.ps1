# TaskManagerCLI Installer Script
# Run this script as Administrator for system-wide installation
# Or run normally for user-only installation

param(
    [switch]$Uninstall
)

$AppName = "TaskManagerCLI"
$ExeName = "tasks.exe"
$InstallPath = "$env:LOCALAPPDATA\Programs\$AppName"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  TaskManagerCLI Installer" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if running as admin
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)

if ($isAdmin) {
    $InstallPath = "C:\Program Files\$AppName"
    Write-Host "Running as Administrator - Installing system-wide" -ForegroundColor Green
} else {
    Write-Host "Running as User - Installing for current user only" -ForegroundColor Yellow
}

# Uninstall function
function Uninstall-App {
    Write-Host "Uninstalling $AppName..." -ForegroundColor Yellow
    
    # Remove from PATH
    $pathType = if ($isAdmin) { "Machine" } else { "User" }
    $currentPath = [Environment]::GetEnvironmentVariable("Path", $pathType)
    
    if ($currentPath -like "*$InstallPath*") {
        $newPath = ($currentPath -split ';' | Where-Object { $_ -ne $InstallPath }) -join ';'
        [Environment]::SetEnvironmentVariable("Path", $newPath, $pathType)
        Write-Host "Removed from PATH" -ForegroundColor Green
    }
    
    # Remove installation directory
    if (Test-Path $InstallPath) {
        Remove-Item -Path $InstallPath -Recurse -Force
        Write-Host "Removed installation directory" -ForegroundColor Green
    }
    
    Write-Host ""
    Write-Host "Uninstallation complete!" -ForegroundColor Green
    Write-Host "Please restart your terminal for changes to take effect." -ForegroundColor Yellow
    exit
}

# Handle uninstall
if ($Uninstall) {
    Uninstall-App
}

# Check if publish folder exists
if (-not (Test-Path ".\publish")) {
    Write-Host "Error: 'publish' folder not found!" -ForegroundColor Red
    Write-Host "Please run this command first:" -ForegroundColor Yellow
    Write-Host "  dotnet publish -c Release -r win-x64 --self-contained false -o publish" -ForegroundColor Cyan
    Write-Host ""
    exit 1
}

# Create installation directory
Write-Host "Creating installation directory..." -ForegroundColor Cyan
if (-not (Test-Path $InstallPath)) {
    New-Item -ItemType Directory -Path $InstallPath -Force | Out-Null
}

# Copy files
Write-Host "Copying files to $InstallPath..." -ForegroundColor Cyan
Copy-Item -Path ".\publish\*" -Destination $InstallPath -Recurse -Force

# Rename executable
$originalExe = Join-Path $InstallPath "TaskManagerCLI.exe"
$newExe = Join-Path $InstallPath $ExeName

if (Test-Path $originalExe) {
    if (Test-Path $newExe) {
        Remove-Item $newExe -Force
    }
    Rename-Item $originalExe $ExeName
    Write-Host "Renamed executable to $ExeName" -ForegroundColor Green
}

# Add to PATH
Write-Host "Adding to PATH environment variable..." -ForegroundColor Cyan
$pathType = if ($isAdmin) { "Machine" } else { "User" }
$currentPath = [Environment]::GetEnvironmentVariable("Path", $pathType)

if ($currentPath -notlike "*$InstallPath*") {
    $newPath = $currentPath.TrimEnd(';') + ";$InstallPath"
    [Environment]::SetEnvironmentVariable("Path", $newPath, $pathType)
    Write-Host "Added to PATH successfully!" -ForegroundColor Green
} else {
    Write-Host "Already in PATH, skipping..." -ForegroundColor Yellow
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  Installation Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Installation Path: $InstallPath" -ForegroundColor Cyan
Write-Host "Executable Name: $ExeName" -ForegroundColor Cyan
Write-Host ""
Write-Host "To use the tool, restart your terminal and type:" -ForegroundColor Yellow
Write-Host "  $ExeName help" -ForegroundColor Cyan
Write-Host "  $ExeName add `"My first task`"" -ForegroundColor Cyan
Write-Host ""
Write-Host "To uninstall, run:" -ForegroundColor Yellow
Write-Host "  .\install.ps1 -Uninstall" -ForegroundColor Cyan
Write-Host ""