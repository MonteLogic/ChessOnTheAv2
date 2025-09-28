#!/bin/bash

# Simple build script for Vercel
echo "Building Avalonia WASM for Vercel..."

# Install .NET 8.0 SDK
echo "Installing .NET 8.0 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.302
export PATH="$HOME/.dotnet:$PATH"

# Verify .NET installation
echo "Verifying .NET installation..."
dotnet --version

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore

# Build and publish the WASM project
echo "Building and publishing WASM project..."
dotnet publish AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release -o ./publish

echo "Build complete! Files are in ./publish/wwwroot/"
