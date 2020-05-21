import { app, BrowserWindow, Details } from 'electron'
import { CaptchaService } from '../captcha/CaptchaService'
import { CaptchaScreen } from '../electron/captcha/CaptchaScreen'
import { CookiesUtil } from '../shared/util/CookieUtil'

import puppeteer from 'puppeteer-extra'
import StealthPlugin = require('puppeteer-extra-plugin-stealth')

let captchaScreen: CaptchaScreen
let captchaService = new CaptchaService('3100')
let cookieUtil = new CookiesUtil()

app.on('ready', async () => {
    const mainWindow = new BrowserWindow({
        height: 680,
        width: 480,
        title: `Harvester`,
        webPreferences: {
            allowRunningInsecureContent: true,
            nodeIntegration: true,
        },
    })

    captchaScreen = new CaptchaScreen(mainWindow, captchaService)
    captchaScreen.start()

    await setCookies(mainWindow)
    await startLogInToGmail(mainWindow)
})

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        captchaScreen.close()
        app.quit()
    }
})

async function startLogInToGmail(mainElectronWindow: BrowserWindow) {
    try {
        puppeteer
            .use(StealthPlugin())
            .launch({
                headless: false,
                devtools: false,
                args: [
                    '--disable-web-security',
                    '--user-data-dir',
                    '--allow-running-insecure-content',
                ],
                ignoreDefaultArgs: ['--enable-automation'],
            })
            .then(async (browser) => {
                const page = await browser.newPage()
                await page.goto('https://stackoverflow.com/users/login')
                await page.waitForSelector('.aim', { timeout: 0 })
                const client = await page.target().createCDPSession()
                const allCookies = await client.send('Network.getAllCookies')
                await cookieUtil.writeGoogleCookies(allCookies)

                setCookies(mainElectronWindow)
            })
    } catch (err) {
        console.log(err)
    }
}

async function setCookies(mainElectronWindow: BrowserWindow) {
    try {
        const cookies = await cookieUtil.readGoogleCookies()
        const electronCookies = cookieUtil.convertToElectronCookies(cookies)

        electronCookies.forEach((element) => {
            mainElectronWindow.webContents.session.cookies.set(element)
        })
    } catch (err) {
        console.log(err)
    }
}
