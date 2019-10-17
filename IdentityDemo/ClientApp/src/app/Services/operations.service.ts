import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config } from 'protractor';
import { ApplicationOperation } from '../Models/Identity/ApplicationOperation';

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json'
    })
};


@Injectable({
  providedIn: 'root'
})
export class OperationsService {

    constructor(private _http: HttpClient) { }

    getApplicationOperations(): Observable<HttpResponse<Config>> {
        return this._http.get<HttpResponse<Config>>('/api/operations/list', { observe: 'response' }).pipe();
    }

    getApplicationOperation(id: string): Observable<HttpResponse<Config>> {
        return this._http.get<HttpResponse<Config>>(`/api/operations/${id}`, { observe: 'response' }).pipe();
    }

    addApplicationOperation(operation: ApplicationOperation): Observable<ApplicationOperation> {
        return this._http.post<ApplicationOperation>(`/api/operations/`, operation, httpOptions).pipe();
    }

    editApplicationOperation(operation: ApplicationOperation): Observable<ApplicationOperation> {
        return this._http.put<ApplicationOperation>(`/api/operations/${operation.id}`, operation, httpOptions).pipe();
    }

    deleteApplicationOperation(id: string): Observable<{}> {
        return this._http.delete(`/api/operations/${id}`, httpOptions).pipe();
    }
}
