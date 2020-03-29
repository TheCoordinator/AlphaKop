import { BrowserWindow, ipcMain, protocol, net } from 'electron'
import * as fs from 'fs'
import { IpcChannelInterface } from '../ipc/IpcChannelInterface'
import { IpcRequest } from '../../shared/IpcRequest'
import { ICaptchaService } from '../../captcha/ICaptchaService'
import { CaptchaRequest } from '../../captcha/CaptchaRequest'

export class CaptchaScreen {
    private readonly window: BrowserWindow
    private readonly capthaService: ICaptchaService

    public constructor(
        window: BrowserWindow,
        capthaService: ICaptchaService,
    ) {
        this.window = window
        this.capthaService = capthaService
    }

    // Setup

    public start() {
        this.setupCaptchaService()

        this.capthaService.start()
        this.loadWindow()
    }

    public close() {
        // TODO: Clean up
    }

    // CaptchaService

    private setupCaptchaService() {
        this.capthaService.addCaptchaRequestEvent((request) =>
            this.didReceiveCaptchaRequest(request),
        )
    }

    private didReceiveCaptchaRequest(request: CaptchaRequest) {
        this.setupCaptchaInterceptor(request)
        this.window.loadURL(request.host)
    }

    private setupCaptchaInterceptor(request: CaptchaRequest) {
        const self = this

        protocol.interceptBufferProtocol('http', async (req, callback) => {
            if (req.url == `${request.host}/`) {
                const file = await self.getCaptchaHtmlFile(request)
                callback(Buffer.from(file))
            } else {
                const request = net.request(req)
                request.on('response', (res) => {
                    const chunks: Uint8Array[] = new Array()

                    res.on('data', (chunk) => {
                        chunks.push(Buffer.from(chunk))
                    })

                    res.on('end', async () => {
                        const file = Buffer.concat(chunks)
                        callback(file)
                    })
                })

                if (req.uploadData) {
                    req.uploadData.forEach((part) => {
                        if (part.bytes) {
                            request.write(part.bytes)
                        } else if (part.file) {
                            request.write(fs.readFileSync(part.file))
                        }
                    })
                }

                request.end()
            }
        })
    }

    private async getCaptchaHtmlFile(request: CaptchaRequest): Promise<string> {
        const file = await fs.promises.readFile(
            __dirname + '/../../app/captcha.html',
            'utf8',
        )

        return file.replace('siteKey_data', request.siteKey)
    }

    // App Events

    private loadWindow() {
        this.window.loadFile('../../app/loader.html')
    }

    // Channels

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
