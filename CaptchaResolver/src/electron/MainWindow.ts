import { app, BrowserWindow, ipcMain } from 'electron'
import { IpcChannelInterface } from './ipc/IpcChannelInterface'
import { IpcRequest } from '../shared/IpcRequest'
import { CaptchaInfoChannel } from './captcha/CaptchaInfoChannel'
import { CaptchaService } from '../captcha/CaptchaService'

export class MainWindow {
    private mainWindow: BrowserWindow
    private readonly capthaService: CaptchaService

    public constructor(capthaService: CaptchaService) {
        this.capthaService = capthaService

        app.on('ready', this.createWindow)
        app.on('window-all-closed', this.onWindowAllClosed)
        app.on('activate', this.onActivate)
    }

    private onWindowAllClosed() {
        if (process.platform !== 'darwin') {
            app.quit()
        }
    }

    private onActivate() {
        if (!this.mainWindow) {
            this.createWindow()
        }
    }

    private createWindow() {
        this.mainWindow = new BrowserWindow({
            height: 680,
            width: 480,
            title: `Harvester`,
            webPreferences: {
                allowRunningInsecureContent: true,
            },
        })

        this.mainWindow.loadFile('../../app/loader.html')
    }

    public registerIpcChannels<TRequest extends IpcRequest>(
        ipcChannels: IpcChannelInterface<TRequest>[],
    ) {
        ipcChannels.forEach(this.registerIpcChannel)
    }

    public registerIpcChannel<TRequest extends IpcRequest>(
        ipcChannel: IpcChannelInterface<TRequest>,
    ) {
        ipcMain.on(ipcChannel.getName(), (event, request) =>
            ipcChannel.handle(event, request),
        )
    }
}
