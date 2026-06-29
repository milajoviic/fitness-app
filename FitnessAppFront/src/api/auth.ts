import { tokenStorage } from "./tokenStorage";
import type {LoginDto, RegisterDto, AuthTokens, CurrentUser} from "../types/auth";
import api from "./axios";

export const authApi = {
    register: (dto: RegisterDto) => api.post("/api/auth/register", dto),
    login: async (dto: LoginDto): Promise<AuthTokens> => {
        const resp = await api.post<AuthTokens>("/api/auth/login", dto);
        tokenStorage.set(resp.data.accessToken, resp.data.refreshToken);
        return resp.data;
    },
    logout: async() => {
        const refresh = tokenStorage.getRefresh();
        if(refresh){
            try{
                await api.post("/api/auth/logout", refresh, {
                    headers: {"Content-Type": "application/json"},
                });
            }catch{
            }
        }
        tokenStorage.clear();
    },
    me: (): Promise<CurrentUser> => api.get<CurrentUser>("/api/user/me").then((r) => r.data),
};