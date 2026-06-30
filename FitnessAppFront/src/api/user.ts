import api from "./axios";
import type { CurrentUser } from "../types/auth";
import type { UpdateUserDto } from "../types/user";

export const userApi = {
    me: (): Promise<CurrentUser> => api.get<CurrentUser>("/api/user/me").then((r) => r.data),
    update: (dto: UpdateUserDto) => api.put("api/user", dto),
    remove: () => api.delete("/api/user"),
}














































