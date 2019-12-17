import { Injectable, Output, EventEmitter } from '@angular/core';
import { ApplicationUser } from '../Models/Identity/ApplicationUser';
import { HttpClient } from '@angular/common/http';

import * as _moment from 'moment';
import { UsersService } from './users.service';
import { resolve } from 'path';
import { reject } from 'q';
const moment = _moment;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
    @Output() getLoggedIn: EventEmitter<any> = new EventEmitter();

    private _httpClient: HttpClient

    constructor(httpClient: HttpClient, private _usersService: UsersService) {
        this._httpClient = httpClient;
    }

    signIn(username: string, password: string) {
        var promise = new Promise((resolve, reject) => {
            this.consumeBackEnd(username, password).subscribe(
                (data) => {
                    this.setSession(data);
                    this.getLoggedIn.emit(true);
                },
                () => { reject();},
                () => { resolve();}
            );
        });
        return promise;
    }

    consumeBackEnd(username: string, password: string) {
        return this._httpClient.post('OAuth/Token', { username, password })
            .pipe();
    }

    private setSession(authResult) {
        const expiresAt = moment().utc(authResult.expiresIn).local();

        //localStorage.setItem('applicationUser', JSON.stringify(authResult.applicationUser));
        localStorage.setItem('idToken', JSON.stringify(authResult.idToken));
        localStorage.setItem("expiresIn", JSON.stringify(expiresAt.valueOf()));
    }

    logout() {
        localStorage.removeItem("idToken");
        localStorage.removeItem("expiresIn");
        localStorage.removeItem("applicationUser");
        this.getLoggedIn.emit(false);
    }

    public isLoggedIn() {
        return moment().isBefore(this.getExpiration());
    }

    isLoggedOut() {
        return !this.isLoggedIn();
    }

    getExpiration() {
        const expiration = localStorage.getItem("expiresIn");
        const expiresAt = JSON.parse(expiration);
        return moment(expiresAt);
    }    
}
