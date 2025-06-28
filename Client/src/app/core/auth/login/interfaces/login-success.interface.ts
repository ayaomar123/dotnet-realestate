export interface LoginSuccess {
    success: boolean;
    data: {
        accessToken: string;
        refreshToken: string;
    };
    errors?: any;
    message?: string;
}