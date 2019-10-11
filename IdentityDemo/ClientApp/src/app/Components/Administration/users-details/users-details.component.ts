import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationUser } from '../../../Models/Identity/ApplicationUser';
import { UsersService } from '../../../Services/users.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-users-details',
  templateUrl: './users-details.component.html',
  styleUrls: ['./users-details.component.css']
})
@Injectable()
export class UsersDetailsComponent implements OnInit {
    user: ApplicationUser;
    add: boolean;

    constructor(private _usersService: UsersService, private _location: Location, private _router: ActivatedRoute) { }


    ngOnInit() {
        let path = this._router.routeConfig.path;
        if (path === "user-add") {
            this.add = true;
            this.user = new ApplicationUser();
        } else {
            let username = this._router.snapshot.paramMap.get('id');
            this._usersService.getApplicationUser(username).subscribe(
                (result: any) => {
                    this.user = result.body.value;
                }
            );
        } 
    }

    onSave() {
        if (this.add) {
            this._usersService.addApplicationUser(this.user).subscribe(
                () => { this._location.back(); }
            );
        } else {
            this._usersService.editApplicationUser(this.user)
                .subscribe(
                    () => { this._location.back(); }
                );
        }
    }
}
