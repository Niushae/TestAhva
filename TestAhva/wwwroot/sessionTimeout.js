// sessionTimeout.js - ES module friendly with global fallback
let _sessionTimer = {
    timer: null,
    events: ['mousemove', 'keypress', 'click', 'scroll'],
    reset: null
};

export function initializeInactivityTimer(dotNetHelper, timeoutMs) {
    // If a reset function already exists, clear previous listeners and timer
    if (_sessionTimer.reset) {
        _sessionTimer.events.forEach(e => window.removeEventListener(e, _sessionTimer.reset));
        if (_sessionTimer.timer) {
            clearTimeout(_sessionTimer.timer);
            _sessionTimer.timer = null;
        }
        _sessionTimer.reset = null;
    }

    _sessionTimer.reset = function () {
        if (_sessionTimer.timer) clearTimeout(_sessionTimer.timer);
        _sessionTimer.timer = setTimeout(() => {
            try {
                // CHANGED: Invoke the warning modal sequence instead of an immediate hard redirect
                dotNetHelper.invokeMethodAsync('ShowInactivityWarning');
            } catch (e) {
                // swallow errors - the .NET object may be disposed
                console.warn('invokeMethodAsync failed in initializeInactivityTimer', e);
            }
        }, timeoutMs);
    };

    // Attach event listeners
    _sessionTimer.events.forEach(e => window.addEventListener(e, _sessionTimer.reset));

    // Also run onload to mimic original behavior
    window.addEventListener('load', _sessionTimer.reset);

    // Start timer immediately
    _sessionTimer.reset();
}

export function disposeInactivityTimer() {
    if (_sessionTimer.reset) {
        _sessionTimer.events.forEach(e => window.removeEventListener(e, _sessionTimer.reset));
        window.removeEventListener('load', _sessionTimer.reset);
        if (_sessionTimer.timer) {
            clearTimeout(_sessionTimer.timer);
            _sessionTimer.timer = null;
        }
        _sessionTimer.reset = null;
    }
}

// Backwards compatibility: expose globals if the script is included without module import
window.initializeInactivityTimer = initializeInactivityTimer;
window.disposeInactivityTimer = disposeInactivityTimer;