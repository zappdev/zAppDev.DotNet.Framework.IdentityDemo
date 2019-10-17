import { ApplicationPermission } from "./ApplicationPermission";

export class ApplicationOperation {
    id: number;
    name: string;
    parentControllerName: string;
    type: string;
    isAvailableToAnonymoys: boolean;
    isAvailableToAllAuthorizedUsers: boolean;
    permissions: ApplicationPermission[];
}
