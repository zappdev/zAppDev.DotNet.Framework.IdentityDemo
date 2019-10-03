import { Component, Injectable, OnInit, Input } from '@angular/core';
import { AuthService } from '../Services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

@Injectable()

export class NavMenuComponent implements OnInit {
    isExpanded = false;
    private _authService: AuthService;
    private _router: Router;
    isLoggedIn = false;
    isLoggedOut = true;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
    }

    constructor(authService: AuthService, router: Router) {
        this._authService = authService;
        this._authService.getLoggedIn.subscribe(_loggedIn => this.changeLoggedIn(_loggedIn));
        this._router = router;
    }

    ngOnInit() {
        this.isLoggedIn = this._authService.isLoggedIn();
        this.isLoggedOut = !this.isLoggedIn;
    }

    changeLoggedIn(loggedIn: boolean) {
        this.isLoggedIn = loggedIn;
        this.isLoggedOut = !loggedIn;
    }

    signOut() {
        this._authService.logout();
        this._router.navigate(['/signIn']);
    }
}
