import { Component, OnInit, Injectable } from '@angular/core';
import { RolesService } from '../../../Services/roles.service';
import { ActivatedRoute } from '@angular/router';
import { ApplicationRole } from '../../../Models/Identity/ApplicationRole';
import { Location } from '@angular/common';
import { PermissionsService } from '../../../Services/permissions.service';
import { ApplicationPermission } from '../../../Models/Identity/ApplicationPermission';

@Component({
  selector: 'app-roles-details',
  templateUrl: './roles-details.component.html',
  styleUrls: ['./roles-details.component.css']
})
@Injectable()
export class RolesDetailsComponent implements OnInit {

    role : ApplicationRole;
    add: boolean;
    permissions: ApplicationPermission[];

    constructor(private _rolesService: RolesService, private _permissionsService: PermissionsService, private _location: Location, private _router: ActivatedRoute) { }

    ngOnInit() {
        this.getPermissions();
        let path = this._router.routeConfig.path;
        if (path === "role-add") {
            this.add = true;
            this.role = new ApplicationRole();
            this.role.isCustom = true;
        } else {
            let id = this._router.snapshot.paramMap.get('id');
            this._rolesService.getApplicationRole(id).subscribe(
                (data: any) => {
                    this.role = data.body.value;
                }
            );
        } 
    }
    getPermissions() {
        this._permissionsService.getApplicationPermissions().subscribe(
            (data: any) => {
                this.permissions = data.body.value;
            }
        );
    }
    onSave() {
        if (this.add) {
            this._rolesService.addApplicationRole(this.role).subscribe(
                () => { this._location.back(); }
            );
        } else {
            this._rolesService.editApplicationRole(this.role).subscribe(
                () => { this._location.back(); }
            );
        }
    }
    trackPermission(x: ApplicationPermission, y: ApplicationPermission) {
        return x.id === y.id;
    }
}
