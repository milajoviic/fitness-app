export interface LoginDto{
    email: string,
    password: string;
}
export interface RegisterDto{
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    gender: string;
    birthDate: string;
}
export interface AuthTokens{
    accessToken: string;
    refreshToken: string;
}
export interface CurrentUser{
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    gender: string;
    birthDate: string;
}
