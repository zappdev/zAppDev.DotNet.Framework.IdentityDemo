import { Component, OnInit } from '@angular/core';
import { UsersService } from '../../../Services/users.service';
import { ApplicationUser } from '../../../Models/Identity/ApplicationUser';
import { Router } from '@angular/router';

@Component({
    selector: 'app-users-list',
    templateUrl: './users-list.component.html',
    styleUrls: ['./users-list.component.css']
})
export class UsersListComponent implements OnInit {

    displayedColumns = ['username', 'Name', 'Email', 'phoneNumber'];
    datasource: ApplicationUser[];

    constructor(private _usersService: UsersService, private _router: Router) { }

    ngOnInit() {
        this._usersService.getApplicationUsers().subscribe(
            (result: any) => {
                this.datasource = result.body.value;
            }
        ); 
    }
    editUser(row) {
        this._router.navigate(['/user-edit', row.username]);
    }

    addUser() {
        this._router.navigate(['/user-add']);
    }
}
