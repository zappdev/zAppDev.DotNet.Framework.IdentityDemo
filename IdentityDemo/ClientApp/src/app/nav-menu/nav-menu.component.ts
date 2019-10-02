import { Component, Injectable, OnInit } from '@angular/core';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})

@Injectable()

export class NavMenuComponent implements OnInit {
    isExpanded = false;
    private _authService: AuthService;
    isLoggedIn = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
    }

    constructor(authService: AuthService) {
        this._authService = authService;
    }

    ngOnInit() {
        this.isLoggedIn = this._authService.isLoggedIn();
    }
}
