# 🚀 Vercel Deployment Setup Guide

## Quick Setup Checklist

### 1. Vercel Account Setup ✅
- [ ] Create account at [vercel.com](https://vercel.com)
- [ ] Sign up with GitHub (recommended)

### 2. Create Vercel Project ✅
- [ ] Click "New Project" in Vercel dashboard
- [ ] Import your GitHub repository
- [ ] **Don't deploy yet** - configure first

### 3. Vercel Project Configuration ✅

**Build Settings:**
```
Framework Preset: Other
Build Command: dotnet publish AvaloniaTest.Browser/AvaloniaTest.Browser.csproj -c Release -o ./publish
Output Directory: publish/wwwroot
Install Command: dotnet restore
```

**Environment Variables:**
- Add any environment variables your app needs

### 4. Get Vercel Credentials ✅

**Vercel Token:**
1. Go to [Account Settings → Tokens](https://vercel.com/account/tokens)
2. Click "Create Token"
3. Name: "GitHub Actions"
4. Copy the token

**Organization & Project IDs:**
1. Go to your Vercel project
2. Settings → General
3. Copy "Project ID" and "Team ID"

### 5. GitHub Secrets ✅

Go to: `Settings → Secrets and variables → Actions`

Add these secrets:
```
VERCEL_TOKEN=your_token_here
VERCEL_ORG_ID=your_team_id_here
VERCEL_PROJECT_ID=your_project_id_here
```

### 6. Test Deployment ✅

1. **Push to GitHub:**
   ```bash
   git add .
   git commit -m "Add Vercel deployment setup"
   git push origin main
   ```

2. **Create Test PR:**
   ```bash
   git checkout -b test-vercel-deployment
   git push origin test-vercel-deployment
   # Then create PR on GitHub
   ```

3. **Watch the Magic:**
   - GitHub Actions will build and deploy
   - Preview URL will be posted on your PR
   - Your Avalonia app will be live! 🎉

## 🔧 Troubleshooting

### Build Fails
- Check that all template parameters are replaced
- Verify .NET 8.0 SDK is available in CI
- Check GitHub Actions logs

### Deployment Fails
- Verify Vercel secrets are correct
- Check Vercel project settings
- Ensure output directory is `publish/wwwroot`

### Preview Not Loading
- Check CORS headers in `vercel.json`
- Verify WASM files are in correct location
- Check browser console for errors

## 📁 File Structure

```
ChessOnTheAv2/
├── .github/workflows/deploy-wasm.yml  # GitHub Actions
├── vercel.json                        # Vercel config
├── .vercelignore                      # Vercel ignore
├── test-deployment.sh                 # Test script
└── publish/wwwroot/                   # Generated files
    ├── index.html
    ├── _framework/
    └── ...
```

## 🎯 What Happens Next

1. **Every PR** gets its own preview URL
2. **Main branch** gets production deployment
3. **Automatic updates** when you push changes
4. **Vercel-like experience** - just like Vercel's own apps!

## 🚀 Ready to Deploy?

Run the test script to verify everything works:
```bash
./test-deployment.sh
```

Then push to GitHub and create a PR to see your Avalonia app live on the web! 🌐
