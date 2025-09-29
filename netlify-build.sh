#!/bin/bash

# Netlify build script for Avalonia WASM
echo "🚀 Building Avalonia WASM for Netlify..."

# Install .NET 8.0 SDK
echo "📦 Installing .NET 8.0 SDK..."
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.302
export PATH="$HOME/.dotnet:$PATH"

# Verify .NET installation
echo "✅ Verifying .NET installation..."
dotnet --version

# Debug: List current directory contents
echo "📁 Current directory contents:"
ls -la

# Debug: Check if project files exist
echo "🔍 Checking for project files..."
if [ -f "AvaloniaTest.Browser/AvaloniaTest.Browser.csproj" ]; then
    echo "✅ AvaloniaTest.Browser.csproj found"
else
    echo "❌ AvaloniaTest.Browser.csproj not found"
    echo "📁 Contents of AvaloniaTest.Browser directory:"
    ls -la AvaloniaTest.Browser/ || echo "AvaloniaTest.Browser directory does not exist"
    
    # Try to find the project file anywhere
    echo "🔍 Searching for .csproj files..."
    find . -name "*.csproj" -type f || echo "No .csproj files found"
fi

# Restore dependencies for the specific project
echo "📚 Restoring dependencies..."
dotnet restore AvaloniaTest.Browser/AvaloniaTest.Browser.csproj

# Build and publish the WASM project
echo "🔨 Building and publishing WASM project..."
dotnet publish AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release -o ./publish

# Verify output
if [ -f "./publish/wwwroot/index.html" ]; then
    echo "✅ Build successful! Files are in ./publish/wwwroot/"
    echo "📁 Published files:"
    ls -la ./publish/wwwroot/
else
    echo "❌ Build failed - index.html not found"
    exit 1
fi

echo "🎉 Netlify build completed successfully!"
