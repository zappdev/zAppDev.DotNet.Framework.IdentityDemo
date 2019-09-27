import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config } from 'protractor';
import { Team } from '../Models/Team';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class TeamsService {

  constructor(private _http: HttpClient) { }

  getTeams(): Observable<HttpResponse<Config>> {
    return this._http.get<HttpResponse<Config>>('/api/teams/list', { observe: 'response' });
  }

  addTeam(team: Team): Observable<Team> {
    return this._http.post<Team>('/api/teams', team, httpOptions)
      .pipe();
  }

  deleteTeam(id: number): Observable<{}> {
    return this._http.delete(`/api/teams/${id}`, httpOptions)
      .pipe();
  }

  editTeam(team: Team): Observable<{}> {
    return this._http.put(`/api/teams/${team.id}`, team, httpOptions)
      .pipe();
  }

  getTeam(id: string) {
    return this._http.get<Team>(`/api/teams/${id}`, httpOptions)
      .pipe();
  }
}
