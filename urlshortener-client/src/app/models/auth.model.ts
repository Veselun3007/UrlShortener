export interface LoginRequest {
  userName: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}

export interface JwtPayload {
  sub?: string;
  unique_name?: string;
  name?: string;
  role?: string | string[];
  exp?: number;
  [key: string]: unknown;
}
