export interface User {
    id: string;
    userName: string;
    email: string;
    phoneNumber: string | null;
    fNameEn?: string | null;
    fNameAr?: string | null;
    lNameEn?: string | null;
    lNameAr?: string | null;
    role: string;
    image?: string | null;
}
