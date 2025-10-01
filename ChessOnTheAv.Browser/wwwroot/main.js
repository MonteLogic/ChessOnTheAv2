console.log('=== CHESS APP DEBUG START ===');
console.log('Starting main.js execution...');

// Add comprehensive debugging
function debugLog(message, data = null) {
    const timestamp = new Date().toISOString();
    console.log(`[${timestamp}] ${message}`, data || '');
    
    // Also log to page for visibility
    const debugDiv = document.getElementById('debug-log');
    if (debugDiv) {
        debugDiv.innerHTML += `<div>[${timestamp}] ${message}</div>`;
        debugDiv.scrollTop = debugDiv.scrollHeight;
    }
}

function showError(message, error = null) {
    console.error('ERROR:', message, error);
    
    const loading = document.getElementById('loading');
    if (loading) {
        loading.style.display = 'none';
    }
    
    const out = document.getElementById('out');
    if (out) {
        out.innerHTML = `
            <div style="padding: 20px; text-align: center; color: red; font-family: Arial, sans-serif;">
                <h2>Chess Application Error</h2>
                <p><strong>Error:</strong> ${message}</p>
                ${error ? `<p><strong>Details:</strong> ${error.message || error}</p>` : ''}
                <p>Check the browser console (F12) for more details.</p>
                <button onclick="location.reload()" style="padding: 10px 20px; margin-top: 10px; background: #007acc; color: white; border: none; border-radius: 4px; cursor: pointer;">Reload Page</button>
            </div>
        `;
    }
}

// Add debug log to page
const out = document.getElementById('out');
if (out) {
    out.innerHTML = `
        <div id="debug-log" style="background: #f0f0f0; padding: 10px; margin: 10px; border: 1px solid #ccc; font-family: monospace; font-size: 12px; max-height: 200px; overflow-y: auto;">
            <div>Debug log will appear here...</div>
        </div>
    `;
}

try {
    debugLog('Step 1: Starting dotnet import...');
    
    const { dotnet } = await import('./_framework/dotnet.js');
    debugLog('Step 2: dotnet.js imported successfully');
    
    debugLog('Step 3: Creating dotnet runtime...');
    const { setModuleImports, setConfig, getAssemblyExports, getConfig } = await dotnet
        .withDiagnosticTracing(false)
        .withApplicationArgumentsFromQuery()
        .create();
    
    debugLog('Step 4: Runtime created successfully');
    
    const config = getConfig();
    debugLog('Step 5: Got runtime config', config);
    
    debugLog('Step 6: Hiding loading screen...');
    const loading = document.getElementById('loading');
    if (loading) {
        loading.style.display = 'none';
        debugLog('Loading screen hidden');
    } else {
        debugLog('WARNING: Loading screen element not found');
    }
    
    debugLog('Step 7: Starting main application...');
    debugLog('Main assembly name:', config.mainAssemblyName);
    debugLog('Arguments:', [window.location.href]);
    
    // Try to start the application using the correct method
    debugLog('Available dotnet methods:', Object.keys(dotnet));
    debugLog('Runtime object methods:', Object.keys({ setModuleImports, setConfig, getAssemblyExports, getConfig }));
    
    // Use the correct method to run the main application
    // The runtime object should have runMain method
    if (typeof dotnet.runMain === 'function') {
        await dotnet.runMain(config.mainAssemblyName, [window.location.href]);
    } else {
        // Try alternative approach - get the exports and call Main directly
        debugLog('runMain not available, trying alternative approach...');
        const exports = await getAssemblyExports(config.mainAssemblyName);
        debugLog('Assembly exports:', Object.keys(exports));
        
        // Try to find and call the Main method
        if (exports.ChessOnTheAv && exports.ChessOnTheAv.Browser && exports.ChessOnTheAv.Browser.Program && exports.ChessOnTheAv.Browser.Program.Main) {
            debugLog('Found Main method in exports, calling it...');
            await exports.ChessOnTheAv.Browser.Program.Main();
        } else {
            throw new Error('Could not find Main method in assembly exports');
        }
    }
    
    debugLog('Step 8: Application started successfully!');
    
    // Check if anything was rendered
    setTimeout(() => {
        const outContent = document.getElementById('out').innerHTML;
        debugLog('Step 9: Checking rendered content...', outContent.length > 100 ? 'Content found' : 'No content rendered');
        
        if (outContent.length < 200) {
            debugLog('WARNING: Very little content rendered, checking for Avalonia...');
            
            // Try to find Avalonia elements
            const avaloniaElements = document.querySelectorAll('[data-avalonia]');
            debugLog('Avalonia elements found:', avaloniaElements.length);
            
            if (avaloniaElements.length === 0) {
                showError('Application started but no UI was rendered. This might be an Avalonia initialization issue.');
            }
        }
    }, 2000);
    
} catch (error) {
    debugLog('ERROR: Failed to start application', error);
    showError('Failed to start the chess application', error);
}