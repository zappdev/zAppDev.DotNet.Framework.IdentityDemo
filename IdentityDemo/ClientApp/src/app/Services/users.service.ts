import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ApplicationUser } from '../Models/Identity/ApplicationUser'
import { Observable } from 'rxjs';

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json'
    })
};

@Injectable({
  providedIn: 'root'
})
export class UsersService {

    constructor(private _http: HttpClient) { }

    CreateAdmin(user: ApplicationUser): Observable<ApplicationUser> {
        return this._http.post<ApplicationUser>('/api/CreateAdmin', user, httpOptions)
            .pipe();
    }
}
