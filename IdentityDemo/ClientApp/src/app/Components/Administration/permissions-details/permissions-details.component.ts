import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationPermission } from '../../../Models/Identity/ApplicationPermission';
import { PermissionsService } from '../../../Services/permissions.service';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-permissions-details',
  templateUrl: './permissions-details.component.html',
  styleUrls: ['./permissions-details.component.css']
})
@Injectable()
export class PermissionsDetailsComponent implements OnInit {

    permission : ApplicationPermission;
    add: boolean;

    constructor(private _permissionService: PermissionsService, private _location: Location, private _router: ActivatedRoute) { }

    ngOnInit() {
        let path = this._router.routeConfig.path;
        if (path === "permission-add") {
            this.add = true;
            this.permission = new ApplicationPermission();
        } else {
            let id = this._router.snapshot.paramMap.get('id');
            this._permissionService.getApplicationPermission(id).subscribe(
                (data: any) => {
                    this.permission = data.body.value;
                }
            );
        } 
    }
    onSave() {
        if (this.add) {
            this._permissionService.addApplicationPermission(this.permission).subscribe(
                () => { this._location.back(); }
            );
        } else {
            this._permissionService.editApplicationPermission(this.permission).subscribe(
                () => { this._location.back(); }
            );
        }
    }
}
