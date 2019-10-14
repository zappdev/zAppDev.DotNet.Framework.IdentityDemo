import { Component, OnInit, Injectable } from '@angular/core';
import { PermissionsService } from '../../../Services/permissions.service';
import { Router } from '@angular/router';
import { ApplicationPermission } from '../../../Models/Identity/ApplicationPermission';

@Component({
  selector: 'app-permissions-list',
  templateUrl: './permissions-list.component.html',
  styleUrls: ['./permissions-list.component.css']
})
  @Injectable()
export class PermissionsListComponent implements OnInit {

    displayedColumns = ['name','description','isCustom'];
    datasource : ApplicationPermission[];

    constructor(private _permissionsService: PermissionsService, private _router : Router) { }

    ngOnInit() {
        this._permissionsService.getApplicationPermissions().subscribe(
            (data: any) => {
                this.datasource = data.body.value;
            }
        );
    }
    editPermission(row) {
        this._router.navigate(['/permission-edit', row.id]);
    }

    addPermission() {
        this._router.navigate(['/permission-add']);
    }
}
