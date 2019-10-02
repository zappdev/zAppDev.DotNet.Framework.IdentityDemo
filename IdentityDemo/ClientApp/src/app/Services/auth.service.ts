import { Injectable } from '@angular/core';
import { ApplicationUser } from '../Models/Identity/ApplicationUser';
import { HttpClient } from '@angular/common/http';

import * as _moment from 'moment';
const moment = _moment;

@Injectable({
  providedIn: 'root'
})
export class AuthService {

    private _httpClient: HttpClient

    constructor(httpClient: HttpClient) {
        this._httpClient = httpClient;
    }

    signIn(username: string, password: string) {
        return this.consumeBackEnd(username, password).subscribe(
            (data) => {
                this.setSession(data)
            },
        );
    }

    consumeBackEnd(username: string, password: string) {
        return this._httpClient.post<ApplicationUser>('api/SignIn', { username, password })
            .pipe();
    }

    private setSession(authResult) {
        const expiresAt = moment().add(authResult.expiresIn, 'second');

        localStorage.setItem('id_token', authResult.idToken);
        localStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()));
    }

    logout() {
        localStorage.removeItem("id_token");
        localStorage.removeItem("expires_at");
    }

    public isLoggedIn() {
        return moment().isBefore(this.getExpiration());
    }

    isLoggedOut() {
        return !this.isLoggedIn();
    }

    getExpiration() {
        const expiration = localStorage.getItem("expires_at");
        const expiresAt = JSON.parse(expiration);
        return moment(expiresAt);
    }    
}
