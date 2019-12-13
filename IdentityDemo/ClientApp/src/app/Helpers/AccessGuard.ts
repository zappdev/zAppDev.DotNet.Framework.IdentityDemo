import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, Router, ActivatedRoute } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../Services/auth.service";
import { OperationsService } from "../Services/operations.service";
import { ApplicationOperation } from "../Models/Identity/ApplicationOperation";
import { ApplicationUser } from "../Models/Identity/ApplicationUser";
import { ApplicationPermission } from "../Models/Identity/ApplicationPermission";

@Injectable()
export class AccessGuard implements CanActivate {

    constructor(public authService: AuthService, public operationsService: OperationsService, public router: Router) {
    }

    canActivate(route: ActivatedRouteSnapshot): boolean {

        if (this.authService.isLoggedOut()) {
            this.router.navigate(['signIn']);
        }

        let path = route.routeConfig.path;
        const applicationOperations: ApplicationOperation[] = JSON.parse(localStorage.getItem('operations'));
        const applicationOperation = applicationOperations.filter(x => x.name.includes(path))[0];
        if (applicationOperation != null && applicationOperation.isAvailableToAnonymoys) { return true; }
        
        if (!this.hasPermission(applicationOperation)) {
            this.router.navigate(['unauthorized']);
        }
        
        return true;
    }
    hasPermission(applicationOperation: ApplicationOperation): boolean {
        if (applicationOperation.isAvailableToAllAuthorizedUsers) { return true; }

        let userJson = localStorage.getItem('applicationUser');
        const applicationUser: ApplicationUser = JSON.parse(userJson);

        let userPermissions: ApplicationPermission[] = [];
        applicationUser.roles.forEach((role) => {
            role.permissions.forEach((permission) => {
                userPermissions.push(permission);
            });
        })

        return this.checkPermission(userPermissions, applicationOperation.permissions);

    }
    checkPermission(userPermissions: ApplicationPermission[], operationPermissions: ApplicationPermission[]): boolean {
        var ok = false;

        operationPermissions.map(x => {
            if (userPermissions.filter(y => y.id == x.id).length > 0) {
                ok = true;
            }
        });

        return ok;        
    }
}
