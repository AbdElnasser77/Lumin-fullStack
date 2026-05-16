export interface VerifyOTPRes {
    token: string
    expiresAt: string
    refreshToken: string
    refreshTokenExpiresAt: string
    user: User
}
interface User {
    id: string
    firstName: string
    lastName: string
    email: string
    isEmailVerified: boolean
}