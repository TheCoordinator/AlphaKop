import { IpcRequest } from '../../shared/IpcRequest'

export class CaptchaIpcRequest implements IpcRequest {
    responseChannel?: string
    readonly host: string
    readonly siteKey: string

    public constructor(host: string, siteKey: string, responseChannel?: string) {
        this.host = host
        this.siteKey = siteKey
        this.responseChannel = responseChannel
    }
}
