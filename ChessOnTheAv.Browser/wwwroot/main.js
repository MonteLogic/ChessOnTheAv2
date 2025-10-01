import { dotnet } from './_framework/dotnet.js'

const is_browser = typeof window != "undefined";
if (!is_browser) throw new Error(`Expected to be running in a browser`);

// Update loading status
function updateStatus(message) {
    const statusDiv = document.getElementById('loading-status');
    if (statusDiv) {
        statusDiv.innerHTML = `<div>${message}</div>`;
    }
    console.log(`[STATUS] ${message}`);
}

// Add error handling
function handleError(error) {
    console.error('[ERROR]', error);
    updateStatus(`Error: ${error.message || error}`);
}

try {
    updateStatus("Loading .NET runtime...");
    
    const dotnetRuntime = await dotnet
        .withDiagnosticTracing(false)
        .withApplicationArgumentsFromQuery()
        .create();

    updateStatus("Runtime created, getting config...");
    const config = dotnetRuntime.getConfig();
    console.log('[CONFIG]', config);

    updateStatus("Starting main application...");
    await dotnetRuntime.runMain(config.mainAssemblyName, [globalThis.location.href]);
    
    updateStatus("Application started successfully!");
    console.log('[SUCCESS] Application started');
    
} catch (error) {
    handleError(error);
}
