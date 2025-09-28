#!/bin/bash

# Simple build script for Vercel
echo "Building Avalonia WASM for Vercel..."

# Restore dependencies
dotnet restore

# Build and publish the WASM project
dotnet publish AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release -o ./publish

echo "Build complete! Files are in ./publish/wwwroot/"
