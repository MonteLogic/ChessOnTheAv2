#!/bin/bash

echo "🧪 Testing Avalonia WASM Deployment Setup"
echo "========================================"

# Check if .NET is available
echo "📦 Checking .NET installation..."
if command -v dotnet &> /dev/null; then
    echo "✅ .NET found: $(dotnet --version)"
else
    echo "❌ .NET not found. Please install .NET 8.0 SDK"
    exit 1
fi

# Check if project builds
echo "🔨 Testing WASM build..."
if dotnet build AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release; then
    echo "✅ WASM build successful"
else
    echo "❌ WASM build failed"
    exit 1
fi

# Check if publish works
echo "📤 Testing WASM publish..."
if dotnet publish AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release -o ./publish; then
    echo "✅ WASM publish successful"
else
    echo "❌ WASM publish failed"
    exit 1
fi

# Check if required files exist
echo "📁 Checking published files..."
if [ -f "publish/wwwroot/index.html" ]; then
    echo "✅ index.html found"
else
    echo "❌ index.html missing"
    exit 1
fi

if [ -d "publish/wwwroot/_framework" ]; then
    echo "✅ _framework directory found"
else
    echo "❌ _framework directory missing"
    exit 1
fi

echo ""
echo "🎉 All tests passed! Your project is ready for Vercel deployment."
echo ""
echo "Next steps:"
echo "1. Push your code to GitHub"
echo "2. Set up Vercel project with the settings above"
echo "3. Add GitHub secrets"
echo "4. Create a pull request to test the workflow"
echo ""
echo "Preview URL will be posted automatically on your PR! 🚀"
