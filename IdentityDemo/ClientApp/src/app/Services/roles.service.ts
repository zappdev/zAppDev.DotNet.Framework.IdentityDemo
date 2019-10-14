import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config } from 'protractor';
import { ApplicationRole } from '../Models/Identity/ApplicationRole';

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json'
    })
};

@Injectable({
  providedIn: 'root'
})

export class RolesService {

    constructor(private _http: HttpClient) { }

    getApplicationRoles(): Observable<HttpResponse<Config>> {
        return this._http.get<HttpResponse<Config>>('/api/roles/list', { observe: 'response' }).pipe();
    }

    getApplicationRole(id: string): Observable<HttpResponse<Config>> {
        return this._http.get<HttpResponse<Config>>(`/api/roles/${id}`, { observe: 'response' }).pipe();
    }

    addApplicationRole(role: ApplicationRole): Observable<ApplicationRole> {
        return this._http.post<ApplicationRole>(`/api/roles/`, role, httpOptions).pipe();
    }

    editApplicationRole(role: ApplicationRole): Observable<ApplicationRole> {
        return this._http.put<ApplicationRole>(`/api/roles/${role.id}`, role, httpOptions).pipe();
    }

    deleteApplicationRole(id: string): Observable<{}> {
        return this._http.delete(`/api/roles/${id}`, httpOptions).pipe();
    }
}
