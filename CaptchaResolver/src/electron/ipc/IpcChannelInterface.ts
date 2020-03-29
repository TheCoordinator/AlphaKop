import { IpcMainEvent } from 'electron'
import { IpcRequest } from '../../shared/IpcRequest'

export interface IpcChannelInterface<TRequest extends IpcRequest> {
    getName(): string

    handle(event: IpcMainEvent, request: TRequest): void
}
