import { app, BrowserWindow } from 'electron'
import { CaptchaService } from '../captcha/CaptchaService'
import { CaptchaScreen } from '../electron/captcha/CaptchaScreen'

let captchaScreen: CaptchaScreen
let captchaService = new CaptchaService('8080')

app.on('ready', () => {
    const mainWindow = new BrowserWindow({
        height: 680,
        width: 480,
        title: `Harvester`,
        webPreferences: {
            allowRunningInsecureContent: true,
        },
    })

    captchaScreen = new CaptchaScreen(mainWindow, captchaService)
    captchaScreen.start()
})

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        captchaScreen.close()
        app.quit()
    }
})
