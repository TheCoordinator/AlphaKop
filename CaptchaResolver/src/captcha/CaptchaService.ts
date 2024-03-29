import * as express from 'express'
import * as bodyParser from 'body-parser'
import * as morgan from 'morgan'
import * as moment from 'moment'
import { Server } from 'http'
import { EventHandler } from '../shared/events/Event'
import { EventDispatcher } from '../shared/events/EventDispatcher'
import { CaptchaResponse } from './CaptchaResponse'
import { ICaptchaService } from './ICaptchaService'
import { CaptchaRequest } from './CaptchaRequest'

export class CaptchaService implements ICaptchaService {
    private readonly maxCaptchaLiveInSeconds = 110

    private readonly api = express()
    private server?: Server = null
    private readonly port: string

    private readonly requestEventDispatcher = new EventDispatcher<CaptchaRequest>()
    private readonly requestCancellationEventDispatcher = new EventDispatcher<CaptchaRequest>()

    private captchas: CaptchaResponse[] = new Array()

    public constructor(port: string) {
        this.port = port
    }

    // ICaptchaService

    public start() {
        if (this.server) {
            return
        }

        this.setupApi()
        this.setupRequests()
        this.setupCleanupCaptchasInterval()

        this.server = this.api.listen(this.api.get('port'))
    }

    addCaptchaRequestEvent(handler: EventHandler<CaptchaRequest>) {
        this.requestEventDispatcher.register(handler)
    }

    addCaptchaRequestCancellationEvent(handler: EventHandler<CaptchaRequest>): void {
        this.requestCancellationEventDispatcher.register(handler)
    }

    private didReceiveCaptchaRequest(request: CaptchaRequest): void {
        this.requestEventDispatcher.dispatch(request)
    }

    private didReceiveCaptchaCancellationRequest(request: CaptchaRequest) {
        this.requestCancellationEventDispatcher.dispatch(request)
    }

    didReceiveCaptchaResponse(response: CaptchaResponse): void {
        this.captchas.push(response)
    }

    // API

    private setupApi() {
        this.api.set('port', this.port)
        this.api.use(bodyParser.json())
        this.api.use(bodyParser.urlencoded({ extended: true }))
        this.api.use(morgan('dev'))
    }

    private setupRequests() {
        const self = this

        this.api.post('/trigger', function(req, res) {
            const request = req.body as CaptchaRequest

            if (request && request.siteKey && request.host && request.requestId) {
                res.sendStatus(200)

                self.didReceiveCaptchaRequest(request)
            } else {
                res.status(400)

                const data = {
                    error: 'CapctahRequest missing',
                }

                res.json(data)
            }
        })

        this.api.post('/canceltrigger', function(req, res) {
            const request = req.body as CaptchaRequest

            if (request && request.siteKey && request.host && request.requestId) {
                res.sendStatus(200)

                self.didReceiveCaptchaCancellationRequest(request)
            } else {
                res.status(400)

                const data = {
                    error: 'CapctahRequest missing',
                }

                res.json(data)
            }
        })

        this.api.get('/fetch', function(_req, res) {
            const first = self.captchas.shift()

            if (!first) {
                res.sendStatus(404)
            } else {
                const data = {
                    captcha: first,
                }

                res.json(data)
            }
        })
    }

    private setupCleanupCaptchasInterval() {
        const self = this

        setInterval(function() {
            self.captchas.forEach((element) => {
                if (
                    moment().diff(moment(element.timestamp), 'seconds') >
                    self.maxCaptchaLiveInSeconds
                ) {
                    console.log('Removing Expired Captcha Token')
                    self.captchas.splice(0, 1)
                }
            })
        }, 1000)
    }
}
