import { app, BrowserWindow, IpcMainEvent, protocol, net } from 'electron'
import * as path from 'path'
import * as express from "express";

let captchaApi = express()
let captchaWindow: Electron.BrowserWindow

// App Events

app.on('ready', initCaptchaWindow)

// app.on(new IpcMainE)
// app.on(new ('sendCaptcha'), )
// app.on('sendCaptcha', function(_event, token) {
//     captchaBank.push({
//         token: token,
//         timestamp: moment(),
//         host: config.host,
//         siteKey: config.siteKey,
//     })
// })

// Window Events

async function initCaptchaWindow() {
    captchaWindow = new BrowserWindow({
        width: 480,
        height: 680,
        webPreferences: {
            allowRunningInsecureContent: true,
        },
    })

    captchaWindow.setTitle('Harvester')
    captchaWindow.loadFile(path.join(__dirname, "../app/loader.html"));

    await sleep(1000)

    // Emitted when the window is closed.
    captchaWindow.on('closed', () => {
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        captchaWindow = null
    })
}

// Utils

function sleep(ms: number) {
    return new Promise((resolve) => setTimeout(resolve, ms));
}
