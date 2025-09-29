# 🚀 Netlify Deployment Setup Guide

## Quick Setup Checklist

### 1. Netlify Account Setup ✅
- [ ] Create account at [netlify.com](https://netlify.com)
- [ ] Sign up with GitHub (recommended)

### 2. Create Netlify Site ✅
- [ ] Click "New site from Git" in Netlify dashboard
- [ ] Connect your GitHub repository
- [ ] **Don't deploy yet** - configure first

### 3. Netlify Site Configuration ✅

**Build Settings:**
```
Build command: npm run build
Publish directory: publish/wwwroot
```

**Environment Variables:**
- Add any environment variables your app needs
- No special .NET variables needed (Netlify handles .NET installation)

### 4. Manual Configuration (Alternative) ✅

If you prefer manual setup, use these settings in Netlify:

**Site Settings → Build & Deploy → Build Settings:**
```
Build command: npm run build
Publish directory: publish/wwwroot
```

**Site Settings → Build & Deploy → Environment Variables:**
```
NODE_VERSION = 18
DOTNET_VERSION = 8.0
```

### 5. Test Deployment ✅

1. **Push to GitHub:**
   ```bash
   git add .
   git commit -m "Add Netlify deployment setup"
   git push origin main
   ```

2. **Watch the Magic:**
   - Netlify will automatically build and deploy
   - Your Avalonia app will be live! 🎉

3. **Test the Build Locally:**
   ```bash
   ./test-deployment.sh
   ```

## 🔧 Troubleshooting

### Build Fails
- Check that all template parameters are replaced
- Verify .NET 8.0 SDK is available in CI
- Check Netlify build logs in the dashboard

### Deployment Fails
- Verify build command is `npm run build`
- Ensure publish directory is `publish/wwwroot`
- Check that `netlify.toml` is in the root directory

### Preview Not Loading
- Check CORS headers in `netlify.toml`
- Verify WASM files are in correct location
- Check browser console for errors
- Ensure redirect rules are working

### WASM Files Not Loading
- Check that `netlify.toml` has correct headers for WASM files
- Verify `Cross-Origin-Embedder-Policy` and `Cross-Origin-Opener-Policy` headers
- Check that `.wasm`, `.dll`, and `.pdb` files have correct MIME types

## 📁 File Structure

```
ChessOnTheAv2/
├── netlify.toml                    # Netlify configuration
├── .netlifyignore                  # Netlify ignore file
├── netlify-build.sh                # Netlify build script
├── build.js                        # Node.js build script
├── package.json                    # NPM configuration
├── test-deployment.sh              # Test script
└── publish/wwwroot/                # Generated files
    ├── index.html
    ├── _framework/
    └── ...
```

## 🎯 What Happens Next

1. **Every push to main** triggers automatic deployment
2. **Pull requests** can be configured for preview deployments
3. **Automatic updates** when you push changes
4. **Netlify's CDN** ensures fast global delivery

## 🚀 Advanced Features

### Custom Domain
1. Go to Site Settings → Domain Management
2. Add your custom domain
3. Configure DNS settings as instructed

### Branch Deployments
1. Go to Site Settings → Build & Deploy → Branch Deployments
2. Enable branch deployments for preview branches
3. Each branch gets its own preview URL

### Environment Variables
1. Go to Site Settings → Environment Variables
2. Add any environment variables your app needs
3. Use different values for production vs preview

### Build Hooks
1. Go to Site Settings → Build & Deploy → Build Hooks
2. Create a build hook URL
3. Use it to trigger builds from external services

## 🔍 Monitoring & Analytics

### Build Logs
- View detailed build logs in Netlify dashboard
- Check for .NET installation and build errors
- Monitor build times and success rates

### Site Analytics
- Enable Netlify Analytics in Site Settings
- Monitor visitor traffic and performance
- Track build and deployment metrics

## 🚀 Ready to Deploy?

Run the test script to verify everything works:
```bash
./test-deployment.sh
```

Then push to GitHub and watch your Avalonia app go live on Netlify! 🌐

## 📞 Support

- [Netlify Documentation](https://docs.netlify.com/)
- [Netlify Community](https://community.netlify.com/)
- [Avalonia Documentation](https://docs.avaloniaui.net/)

## 🎉 Success!

Once deployed, you'll have:
- ✅ A live Avalonia WASM app on Netlify
- ✅ Automatic deployments on every push
- ✅ Global CDN for fast loading
- ✅ Custom domain support
- ✅ Branch preview deployments
- ✅ Detailed build logs and analytics
