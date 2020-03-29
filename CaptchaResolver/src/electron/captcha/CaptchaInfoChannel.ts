import { IpcChannelInterface } from '../ipc/IpcChannelInterface'
import { IpcMainEvent } from 'electron'
import { CaptchaIpcRequest } from './CaptchaIpcRequest'

export class CaptchaInfoChannel
    implements IpcChannelInterface<CaptchaIpcRequest> {
    getName(): string {
        return 'captcha'
    }

    handle(event: IpcMainEvent, request: CaptchaIpcRequest): void {
        if (!request.responseChannel) {
            request.responseChannel = `${this.getName()}_response`
        }

        event.sender.send(request.responseChannel, {
            token: 'TokenHere',
            host: 'here',
        })
    }
}
