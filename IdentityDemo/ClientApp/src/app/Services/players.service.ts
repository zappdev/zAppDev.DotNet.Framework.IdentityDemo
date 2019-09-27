import { Injectable } from '@angular/core';
import { HttpHeaders, HttpResponse, HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Config } from 'protractor';
import { Player } from '../Models/Player';

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class PlayersService {

  constructor(private _http: HttpClient) { }

  getPlayers(): Observable<HttpResponse<Config>> {
    return this._http.get<HttpResponse<Config>>('/api/players/list', { observe: 'response' });
  }

  addPlayer(player: Player): Observable<Player> {
    return this._http.post<Player>('/api/players', player, httpOptions)
      .pipe();
  }

  deletePlayer(id: number): Observable<{}> {
    return this._http.delete(`/api/players/${id}`, httpOptions)
      .pipe();
  }

  editPlayer(player: Player): Observable<{}> {
    return this._http.put(`/api/players/${player.id}`, player, httpOptions)
      .pipe();
  }

  getPlayer(id: string) {
    return this._http.get<Player>(`/api/players/${id}`, httpOptions)
      .pipe();
  }
}
