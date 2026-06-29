import axios from "axios";
import {tokenStorage} from "./tokenStorage";

const api = axios.create({
    baseURL: "http://localhost:7054",
});
api.interceptors.request.use((config) => {
    const token = tokenStorage.getAccess();
    if(token)
        config.headers.Authorization = `Bearer ${token}`;
    return config;
});
//401, istekao token
api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const org = error.config;
        const isAuthCall = org.url?.includes("/api/auth/");
        if(error.response?.status == 401 && !org._retry && !isAuthCall){
            org._retry = true;
            const refresh = tokenStorage.getRefresh();

            if(!refresh){
                tokenStorage.clear();
                window.location.href = "/login";
                return Promise.reject(error);
            }

            try{
                const resp = await axios.post("http://localhost:7054/api/auth/refresh", refresh, {
                    headers: {"Content-Type": "application/json" },
                });
                tokenStorage.set(resp.data.accessToken, resp.data.refreshToken);
                org.headers.Authorization = `Bearer ${resp.data.accessToken}`;
                return api(org);
            }
            catch{
                tokenStorage.clear();
                window.location.href = "/login";
                return Promise.reject(error);
            }

            return Promise.reject(error);
        }
    });
export default api;