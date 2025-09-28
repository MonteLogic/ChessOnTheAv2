# Avalonia WASM Deployment Setup

This project is configured for automatic deployment of the Avalonia WebAssembly (WASM) app to Vercel with a Vercel-like preview experience for pull requests.

## 🚀 Features

- **Automatic Deployments**: Every pull request gets its own preview URL
- **Cross-Platform Development**: Develop on Linux, deploy to the web
- **Vercel Integration**: Professional hosting with global CDN
- **GitHub Actions**: Automated CI/CD pipeline

## 📋 Setup Instructions

### 1. Vercel Account Setup

1. Create a [Vercel account](https://vercel.com) if you don't have one
2. Create a new project in Vercel dashboard
3. Get your Vercel credentials:
   - Go to [Vercel Account Settings](https://vercel.com/account/tokens)
   - Create a new token
   - Get your Organization ID and Project ID from your Vercel project settings

### 2. GitHub Secrets Configuration

Add these secrets to your GitHub repository:

1. Go to your GitHub repository
2. Navigate to **Settings** → **Secrets and variables** → **Actions**
3. Add the following repository secrets:

```
VERCEL_TOKEN=your_vercel_token_here
VERCEL_ORG_ID=your_org_id_here
VERCEL_PROJECT_ID=your_project_id_here
```

### 3. Workflow Triggers

The deployment workflow will automatically trigger on:
- **Pull Requests**: `opened`, `synchronize`, `reopened`
- **Main Branch Pushes**: `main` or `master` branch

## 🔧 How It Works

### Development Workflow

1. **Develop Locally**: Continue building your Avalonia app on Linux
2. **Create PR**: Push your branch and open a pull request
3. **Automatic Build**: GitHub Actions builds the WASM target
4. **Deploy**: The app is deployed to Vercel
5. **Preview Link**: A comment is posted on your PR with the preview URL

### Build Process

The GitHub Action performs these steps:

1. **Checkout Code**: Gets your latest changes
2. **Setup .NET**: Installs .NET 8.0 SDK
3. **Restore Dependencies**: Downloads NuGet packages
4. **Build WASM**: Compiles the Avalonia.Browser project
5. **Publish**: Creates optimized web files
6. **Deploy to Vercel**: Uploads to Vercel with proper routing
7. **Comment on PR**: Posts the preview URL

## 📁 Project Structure

```
ChessOnTheAv2/
├── .github/workflows/
│   └── deploy-wasm.yml          # GitHub Actions workflow
├── AvaloniaTest.Browser/        # WASM target project
├── AvaloniaTest/                # Shared application code
├── vercel.json                  # Vercel configuration
├── .vercelignore               # Vercel ignore file
└── publish/                    # Generated WASM files (auto-created)
    └── wwwroot/                # Web assets
```

## 🛠️ Local Testing

To test the WASM build locally:

```bash
# Build the WASM project
dotnet build AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release

# Publish for deployment
dotnet publish AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release -o ./publish

# The files will be in ./publish/wwwroot/
```

## 🔍 Troubleshooting

### Common Issues

1. **Build Failures**: Check that all template parameters are replaced
2. **Deployment Errors**: Verify Vercel secrets are correctly set
3. **Preview Not Loading**: Ensure CORS headers are properly configured

### Vercel Configuration

The `vercel.json` file configures:
- Static file serving for WASM assets
- Proper routing for SPA behavior
- CORS headers for WebAssembly

### GitHub Actions Logs

Check the Actions tab in your GitHub repository for detailed build logs if something goes wrong.

## 🎯 Next Steps

1. Set up your Vercel account and get the required credentials
2. Add the secrets to your GitHub repository
3. Create a test pull request to see the magic happen!
4. Your Avalonia app will be live on the web with every PR

## 📚 Additional Resources

- [Avalonia WebAssembly Documentation](https://docs.avaloniaui.net/docs/quickstart/web)
- [Vercel Documentation](https://vercel.com/docs)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
