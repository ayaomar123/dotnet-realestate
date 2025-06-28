export interface LoginError {
  success: boolean;
  data: any;
  errors?: any;
  message?: string;
}