import * as express from 'express'
import * as bodyParser from 'body-parser'
import { CaptchaResponse } from './CaptchaResponse'
import { Server } from 'http'

export class CaptchaService {
    private readonly api = express()
    private server?: Server = null

    private readonly port: string

    private captchas: CaptchaResponse[] = new Array()

    public constructor(port: string) {
        this.port = port
    }

    public start() {
        if (this.server) {
            return
        }

        this.api.set('port', this.port)
        this.api.use(bodyParser.json())
        this.api.use(bodyParser.urlencoded({ extended: true }))

        this.setupRequests()

        this.server = this.api.listen(this.api.get('port'))
    }

    private setupRequests() {
        const self = this

        this.api.get('/trigger', function(_req, _res) {
            // TODO
        })

        this.api.get('/fetch', function(_req, res) {
            const first = self.captchas.shift()

            if (!first) {
                res.sendStatus(404)
            } else {
                const data = {
                    captcha: first,
                }

                res.json(JSON.stringify(data))
            }
        })
    }
}
