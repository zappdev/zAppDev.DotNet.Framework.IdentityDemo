import { Component, OnInit } from '@angular/core';
import { Team } from '../../../Models/Team';
import { ActivatedRoute } from '@angular/router';
import { TeamsService } from '../../../Services/teams.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-teams-details',
  templateUrl: './teams-details.component.html',
  styleUrls: ['./teams-details.component.css']
})
export class TeamsDetailsComponent implements OnInit {

  add: boolean;
  team: Team;

  constructor(private _router: ActivatedRoute, private _teamService: TeamsService, private _location: Location) { }

  ngOnInit() {
    let path = this._router.routeConfig.path;
    if (path === 'team-add') {
      this.add = true;
      this.team = new Team();
    } else {
      let id = this._router.snapshot.paramMap.get('id');
      this._teamService.getTeam(id)
        .subscribe(
          data => { this.team = data; },
          err => console.error(err),
          () => { console.log(this.team); }
        );
    }
  }

  onSave() {
    if (this.add) {
      this._teamService.addTeam(this.team).subscribe(
        () => { this._location.back(); }
      );
    } else {
      this._teamService.editTeam(this.team)
        .subscribe(
          () => { this._location.back(); }
        );
    }
  }
}
