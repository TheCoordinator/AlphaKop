import { EventHandler } from '../shared/events/Event'
import { CaptchaResponse } from './CaptchaResponse';
import { CaptchaRequest } from './CaptchaRequest';

export interface ICaptchaService {
    start(): void
    addCaptchaRequestEvent(handler: EventHandler<CaptchaRequest>): void
    addCaptchaRequestCancellationEvent(handler: EventHandler<CaptchaRequest>): void
    didReceiveCaptchaResponse(response: CaptchaResponse): void
}
