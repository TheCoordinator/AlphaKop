import { BrowserWindow, protocol, net, ipcMain } from 'electron'
import * as fs from 'fs'
import * as moment from 'moment'
import { ICaptchaService } from '../../captcha/ICaptchaService'
import { CaptchaResponse } from '../../captcha/CaptchaResponse'
import { CaptchaRequest } from '../../captcha/CaptchaRequest'

export class CaptchaScreen {
    private readonly window: BrowserWindow
    private readonly capthaService: ICaptchaService
    private requests: CaptchaRequest[] = new Array()

    public constructor(window: BrowserWindow, capthaService: ICaptchaService) {
        this.window = window
        this.capthaService = capthaService
    }

    // Setup

    public start() {
        this.capthaService.start()
        this.loadLoader()

        this.setupCaptchaService()
        this.setupScreenEvents()
    }

    public close() {
        // TODO: Clean up
    }

    // CaptchaService

    private setupCaptchaService() {
        this.capthaService.addCaptchaRequestEvent((request) =>
            this.didReceiveCaptchaRequest(request),
        )

        this.capthaService.addCaptchaRequestCancellationEvent((request) =>
            this.didReceiveCaptchaCancellationRequest(request),
        )
    }

    private didReceiveCaptchaRequest(request: CaptchaRequest) {
        if (this.requests.length == 0) {
            this.loadCaptcha(request)
        }

        if (
            this.requests.find((e) => e.requestId == request.requestId) == null
        ) {
            this.requests.push(request)
        }
    }

    private didReceiveCaptchaCancellationRequest(request: CaptchaRequest) {
        if (this.requests.length == 0) {
            return
        }

        this.requests
            .filter((elem) => elem.requestId == request.requestId)
            .forEach((request) => {
                const index = this.requests.findIndex(
                    (elem) => elem.requestId == request.requestId,
                )

                if (index >= 0) {
                    this.requests.splice(index, 1)
                }
            })

        this.loadNextCaptcha(false)
    }

    private didReceiveCaptchaResponse(response: CaptchaResponse) {
        this.capthaService.didReceiveCaptchaResponse(response)

        this.loadNextCaptcha(true)
    }

    private loadNextCaptcha(remove: Boolean) {
        if (remove && this.requests.length > 0) {
            this.requests.splice(0, 1)
        }

        if (this.requests.length > 0) {
            const request = this.requests[0]
            this.loadCaptcha(request)
        } else {
            this.loadLoader()
        }
    }

    private loadCaptcha(request: CaptchaRequest) {
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

        return file
            .replace('data__siteKey', request.siteKey)
            .replace('data__host', request.host)
    }

    // Screen

    private loadLoader() {
        this.window.loadFile('../../app/loader.html')
    }

    private setupScreenEvents() {
        this.setupSendCaptchaEvent()
    }

    private setupSendCaptchaEvent() {
        const self = this

        ipcMain.on('sendCaptchaSuccess', (_event: any, params: any) => {
            const response: CaptchaResponse = {
                token: params.token as string,
                host: params.host as string,
                timestamp: moment().toDate(),
            }

            console.log(
                `Captcha Success for host ${response.host} token: ${response.token}`,
            )
            self.didReceiveCaptchaResponse(response)
        })
    }
}
