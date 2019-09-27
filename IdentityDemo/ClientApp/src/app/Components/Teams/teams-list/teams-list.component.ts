import { Component, OnInit } from '@angular/core';
import { Team } from '../../../Models/Team';
import { TeamsService } from '../../../Services/teams.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-teams-list',
  templateUrl: './teams-list.component.html',
  styleUrls: ['./teams-list.component.css']
})
export class TeamsListComponent implements OnInit {

  dataSource: Team[];
  displayedColumns = ['id', 'name', 'founded','city' , 'actions'];

  constructor(private _teamService: TeamsService, private _router: Router) { }

  ngOnInit() {
    this.loadTeams();
  }

  loadTeams() {
    this._teamService.getTeams().pipe()
      .subscribe(
        (data: any) => {
          this.dataSource = data.body.value;
        }
      );
  }

  deleteTeam(team: Team) {
    this._teamService.deleteTeam(team.id).subscribe(
      () => {
        this.loadTeams();
      }
    );
  }

  editTeam(row) {
    this._router.navigate(['/team-edit', row.id]);
  }

  addTeam() {
    this._router.navigate(['/team-add']);
  }
}
