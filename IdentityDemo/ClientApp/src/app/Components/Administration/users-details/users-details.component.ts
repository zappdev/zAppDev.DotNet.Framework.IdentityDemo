import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationUser } from '../../../Models/Identity/ApplicationUser';
import { UsersService } from '../../../Services/users.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { ApplicationRole } from '../../../Models/Identity/ApplicationRole';
import { RolesService } from '../../../Services/roles.service';

@Component({
  selector: 'app-users-details',
  templateUrl: './users-details.component.html',
  styleUrls: ['./users-details.component.css']
})
@Injectable()
export class UsersDetailsComponent implements OnInit {
    user: ApplicationUser;
    add: boolean;
    roles: ApplicationRole[];
    password: string;
    passwordRepeat: string;
    readonly: boolean;

    constructor(private _usersService: UsersService, private _rolesService: RolesService, private _location: Location, private _router: ActivatedRoute) { }


    ngOnInit() {
        this.getRoles();
        let path = this._router.routeConfig.path;
        if (path === "user-add") {
            this.add = true;
            this.readonly = false;
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
    getRoles() {
        this._rolesService.getApplicationRoles().subscribe(
            (data: any) => {
                this.roles = data.body.value;
            }
        );
    }
    onSave() {
        if (this.add) {
            this.user.password = this.password;
            this.user.passwordRepeat = this.passwordRepeat;
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
    trackRole(x: ApplicationRole, y: ApplicationRole) {
        return x.id === y.id;
    }
}
