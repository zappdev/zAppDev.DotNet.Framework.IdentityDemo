import { ApplicationPermission } from "./ApplicationPermission";

export class ApplicationRole{
    id : number;
    name : string;
    description : string;
    isCustom : boolean;
    permissions : ApplicationPermission[];
}
