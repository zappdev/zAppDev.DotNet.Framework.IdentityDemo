import { Component, OnInit } from '@angular/core';
import { Player } from '../../../Models/Player';
import { Team } from '../../../Models/Team';
import { ActivatedRoute } from '@angular/router';
import { PlayersService } from '../../../Services/players.service';
import { TeamsService } from '../../../Services/teams.service';
import { Location } from '@angular/common';

// Depending on whether rollup is used, moment needs to be imported differently.
// Since Moment.js doesn't have a default export, we normally need to import using the `* as`
// syntax. However, rollup creates a synthetic default module and we thus need to import it using
// the `default as` syntax.
import * as _moment from 'moment';
const moment = _moment;

@Component({
  selector: 'app-players-details',
  templateUrl: './players-details.component.html',
  styleUrls: ['./players-details.component.css']
})
export class PlayersDetailsComponent implements OnInit {

  add: boolean;
  player: Player;
  teams: Team[];

  constructor(private _router: ActivatedRoute, private _playerService: PlayersService, private _teamService: TeamsService, private _location: Location) { }

  ngOnInit() {
    this.getTeams();
    let path = this._router.routeConfig.path;
    if (path === 'player-add') {
      this.add = true;
      this.player = new Player();
    } else {
      let id = this._router.snapshot.paramMap.get('id');
      this._playerService.getPlayer(id)
        .subscribe(
          data => { this.player = data; },
          err => console.error(err),
          () => { console.log(this.player); }
        );
    }
  }

  getTeams() {
    this._teamService.getTeams().subscribe(
      (data: any) => {
        this.teams = data.body.value;
      },
      err => { console.log(err); },
      () => { console.log('done loading teams'); }
    );
  }

  onSave() {
    if (this.add) {
      this._playerService.addPlayer(this.player).subscribe(
        () => { this._location.back(); }
      );
    } else {
      this._playerService.editPlayer(this.player)
        .subscribe(
          () => { this._location.back(); }
        );
    }
  }

  trackTeam(x: Team, y: Team) {
    return x.id == y.id;
  }
}
