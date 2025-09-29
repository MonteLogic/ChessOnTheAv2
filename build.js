const { execSync } = require('child_process');
const fs = require('fs');

console.log('🚀 Building Avalonia WASM for Netlify...');

try {
  // Install .NET 8.0 SDK
  console.log('📦 Installing .NET 8.0 SDK...');
  execSync('curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.302', { stdio: 'inherit' });
  
  // Add .NET to PATH
  process.env.PATH = `${process.env.HOME}/.dotnet:${process.env.PATH}`;
  
  // Verify .NET installation
  console.log('✅ Verifying .NET installation...');
  const dotnetVersion = execSync('dotnet --version', { encoding: 'utf8' }).trim();
  console.log(`✅ .NET version: ${dotnetVersion}`);
  
  // Debug: List current directory contents
  console.log('📁 Current directory contents:');
  execSync('ls -la', { stdio: 'inherit' });
  
  // Debug: Check if project files exist
  console.log('🔍 Checking for project files...');
  if (fs.existsSync('AvaloniaTest.Browser/AvaloniaTest.Browser.csproj')) {
    console.log('✅ AvaloniaTest.Browser.csproj found');
  } else {
    console.log('❌ AvaloniaTest.Browser.csproj not found');
    console.log('📁 Contents of AvaloniaTest.Browser directory:');
    try {
      execSync('ls -la AvaloniaTest.Browser/', { stdio: 'inherit' });
    } catch (e) {
      console.log('AvaloniaTest.Browser directory does not exist');
    }
    
    // Try to find the project file anywhere
    console.log('🔍 Searching for .csproj files...');
    try {
      execSync('find . -name "*.csproj" -type f', { stdio: 'inherit' });
    } catch (e) {
      console.log('No .csproj files found');
    }
  }
  
  // Restore dependencies for the specific project
  console.log('📚 Restoring dependencies...');
  execSync('dotnet restore AvaloniaTest.Browser/AvaloniaTest.Browser.csproj', { stdio: 'inherit' });
  
  // Build and publish the WASM project
  console.log('🔨 Building and publishing WASM project...');
  execSync('dotnet publish AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release -o ./publish', { stdio: 'inherit' });
  
  // Verify output
  if (fs.existsSync('./publish/wwwroot/index.html')) {
    console.log('✅ Build successful! Files are in ./publish/wwwroot/');
  } else {
    console.error('❌ Build failed - index.html not found');
    process.exit(1);
  }
  
} catch (error) {
  console.error('❌ Build failed:', error.message);
  process.exit(1);
}
