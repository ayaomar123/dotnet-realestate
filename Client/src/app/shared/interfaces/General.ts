export interface General<T> {
    success: boolean;
    message: string;
    data: T;
    errors?: string[];
}