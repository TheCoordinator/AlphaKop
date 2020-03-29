import { CaptchaService } from "../captcha/CaptchaService"
import { MainWindow } from "../electron/MainWindow"

const captchaService = new CaptchaService('8080')
const mainWindow = new MainWindow(captchaService)

captchaService.start()
