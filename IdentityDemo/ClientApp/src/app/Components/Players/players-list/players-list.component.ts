import { Component, OnInit, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { PlayersService } from '../../../Services/players.service';
import { Player } from '../../../Models/Player';

@Component({
  selector: 'app-players-list',
  templateUrl: './players-list.component.html',
  styleUrls: ['./players-list.component.css']
})
@Injectable()
export class PlayersListComponent implements OnInit {

  displayedColumns = ['id', 'firstName', 'lastName', 'dateOfBirth', 'actions'];
  dataSource: Player[];

  constructor(private _playerService: PlayersService, private _router: Router) { }

  ngOnInit() {
    this.loadPlayers();
  }

  loadPlayers() {
    this._playerService.getPlayers()
      .subscribe(
        (data: any) => {
            this.dataSource = data.body.value;
        }
      );
  }

  deletePlayer(player: Player) {
    this._playerService.deletePlayer(player.id).subscribe(
      () => {
        this.loadPlayers();
      }
    );
  }

  editPlayer(row) {
    this._router.navigate(['/player-edit', row.id]);
  }

  addPlayer() {
    this._router.navigate(['/player-add']);
  }
}
