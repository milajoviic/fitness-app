const ACCESS = "accessToken";
const REFRESH = "refreshToken";

export const tokenStorage = {
    getAccess: () => localStorage.getItem(ACCESS),
    getRefresh: () => localStorage.getItem(REFRESH),
    set: (access: string, refresh: string) => {
        localStorage.setItem(ACCESS, access),
        localStorage.setItem(REFRESH, refresh)
    },
    clear: () => {
        localStorage.removeItem(ACCESS),
        localStorage.removeItem(REFRESH)
    },
};