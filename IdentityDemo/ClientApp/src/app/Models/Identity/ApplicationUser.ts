import { ApplicationRole } from "./ApplicationRole";

export class ApplicationUser {
    username: string;
    emailConfirmend: boolean;
    lockoutEnabled: boolean;
    phoneNumberConfirmed: boolean;
    twoFactorEnabled: boolean;
    accessFailedCount: boolean;
    name: string;
    email: string;
    phoneNumber: string;
    lockoutEndDate: Date;
    password: string;
    passwordRepeat: string;
    roles: ApplicationRole[];
}
