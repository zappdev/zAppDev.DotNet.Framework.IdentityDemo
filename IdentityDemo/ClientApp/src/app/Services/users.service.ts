import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { ApplicationUser } from '../Models/Identity/ApplicationUser'
import { Observable } from 'rxjs';
import { Config } from 'protractor';

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

    getApplicationUsers(): Observable<HttpResponse<Config>> {
        return this._http.get<HttpResponse<Config>>('/api/users/list', { observe : 'response' }).pipe();
    }

    getApplicationUser(username: string): Observable<HttpResponse<Config>> {
        return this._http.get<HttpResponse<Config>>(`/api/users/${username}`, { observe: 'response' }).pipe();
    }

    addApplicationUser(user: ApplicationUser): Observable<ApplicationUser> {
        return this._http.post<ApplicationUser>(`/api/users/`, user, httpOptions).pipe();
    }

    editApplicationUser(user: ApplicationUser): Observable<ApplicationUser> {
        return this._http.put<ApplicationUser>(`/api/users/${user.username}`, user, httpOptions).pipe();
    }

    deleteApplicationUser(username: string): Observable<{}> {
        return this._http.delete(`/api/users/${username}`, httpOptions).pipe();
    }
}
