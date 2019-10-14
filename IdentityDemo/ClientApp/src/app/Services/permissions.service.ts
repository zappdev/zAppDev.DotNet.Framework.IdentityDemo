import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config } from 'protractor';
import { ApplicationPermission } from '../Models/Identity/ApplicationPermission';

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json'
    })
};

@Injectable({
  providedIn: 'root'
})
export class PermissionsService {

    constructor(private _http: HttpClient) { }

    getApplicationPermissions(): Observable<HttpResponse<Config>> {
        return this._http.get<HttpResponse<Config>>('/api/permissions/list', { observe: 'response' }).pipe();
    }

    getApplicationPermission(id : string): Observable<HttpResponse<Config>> {
        return this._http.get<HttpResponse<Config>>(`/api/permissions/${id}`, { observe: 'response' }).pipe();
    }

    addApplicationPermission(permission: ApplicationPermission): Observable<ApplicationPermission> {
        return this._http.post<ApplicationPermission>(`/api/permissions/`, permission, httpOptions).pipe();
    }

    editApplicationPermission(permission: ApplicationPermission): Observable<ApplicationPermission> {
        return this._http.put<ApplicationPermission>(`/api/permissions/${permission.id}`, permission, httpOptions).pipe();
    }

    deleteApplicationPermission(id: string): Observable<{}> {
        return this._http.delete(`/api/permissions/${id}`, httpOptions).pipe();
    }
}
