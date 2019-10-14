import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationRole } from '../../../Models/Identity/ApplicationRole';
import { RolesService } from '../../../Services/roles.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-roles-list',
  templateUrl: './roles-list.component.html',
  styleUrls: ['./roles-list.component.css']
})
@Injectable()
export class RolesListComponent implements OnInit {

    displayedColumns = ['name', 'description', 'isCustom'];
    datasource: ApplicationRole[];

    constructor(private _rolesService: RolesService, private _router : Router) { }

    ngOnInit() {
        this._rolesService.getApplicationRoles().subscribe(
            (data: any) => {
                this.datasource = data.body.value;
            }
        );
    }
    editRole(row) {
        this._router.navigate(['/role-edit', row.id]);
    }

    addRole() {
        this._router.navigate(['/role-add']);
    }
}
