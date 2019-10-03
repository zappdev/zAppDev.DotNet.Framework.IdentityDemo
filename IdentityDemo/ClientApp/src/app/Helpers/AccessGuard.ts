import { Injectable } from "@angular/core";
import { CanActivate, ActivatedRouteSnapshot, Router } from "@angular/router";
import { Observable } from "rxjs";
import { AuthService } from "../Services/auth.service";

@Injectable()
export class AccessGuard implements CanActivate {

    constructor(public authService: AuthService, public router: Router) {
    }

    canActivate(): boolean {
        //const requiresLogin = route.data.requiresLogin || false;
        if (this.authService.isLoggedOut()) {
          this.router.navigate(['signIn']);
        }
        return true;
    }
}
