import { Details } from 'electron'
import * as path from 'path'
import * as fs from 'fs'
import * as util from 'util'

export interface Cookie {
    name?: string
    value?: string
    domain?: string
    path?: string
    secure?: boolean
    httpOnly?: boolean
    expirationDate?: number
}

interface CookiesResponse {
    cookies?: Cookie[]
}

export class CookiesUtil {
    readonly googleCookiesPath: string

    constructor() {
        const appDataPath =
            process.env.APPDATA ||
            (process.platform === 'darwin'
                ? `${process.env.HOME}/Library/Preferences`
                : `${process.env.HOME}/.local/share`)

        this.googleCookiesPath = path.join(
            appDataPath,
            '/AlphaKop/Captchas/google_cookies.json',
        )

        this.ensureDirectoryExistence(this.googleCookiesPath)
    }

    public convertToElectronCookies(cookies: Cookie[]): Details[] {
        let result: Details[] = []

        cookies.forEach((element) => {
            const url = this.getUrl(element)

            if (url == null) {
                return
            }

            let details: Details = element as Details
            details.url = url

            result.push(details)
        })

        return result
    }

    private getUrl(cookie: Cookie): string | null {
        if (cookie.domain == null) {
            return null
        }

        if (cookie.domain.startsWith('.') == true) {
            return cookie.domain.replace('.', 'https://www.')
        } else {
            return `https://${cookie.domain}`
        }
    }

    public async writeGoogleCookies(cookies: any) {
        try {
            const writeFileAsync = util.promisify(fs.writeFile)
            await writeFileAsync(
                this.googleCookiesPath,
                JSON.stringify(cookies),
            )
        } catch (err) {
            console.log(err)
        }
    }

    public async readGoogleCookies(): Promise<Cookie[]> {
        const readFileAsync = util.promisify(fs.readFile)
        const buffer = await readFileAsync(this.googleCookiesPath)
        const cookiesResponse = JSON.parse(buffer.toString()) as CookiesResponse

        return cookiesResponse.cookies ?? []
    }

    private ensureDirectoryExistence(filePath: string) {
        const dirName = path.dirname(filePath)
        if (fs.existsSync(dirName)) {
            return
        }

        this.ensureDirectoryExistence(dirName)
        fs.mkdirSync(dirName)
    }
}
