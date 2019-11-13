import { Component, OnInit, Injectable } from '@angular/core';
import { OperationsService } from '../../../Services/operations.service';
import { PermissionsService } from '../../../Services/permissions.service';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ApplicationOperation } from '../../../Models/Identity/ApplicationOperation';
import { ApplicationPermission } from '../../../Models/Identity/ApplicationPermission';

@Component({
  selector: 'app-operations-details',
  templateUrl: './operations-details.component.html',
  styleUrls: ['./operations-details.component.css']
})
@Injectable()
export class OperationsDetailsComponent implements OnInit {

    applicationOperation: ApplicationOperation;
    applicationPermissions: ApplicationPermission[];
    add: boolean;

    constructor(private _operationsService: OperationsService, private _permissionsService: PermissionsService, private _location: Location, private _router: ActivatedRoute) { }

    ngOnInit() {
        this.getApplicationPermissions();
        let path = this._router.routeConfig.path;
        if (path === "operation-add") {
            this.add = true;
            this.applicationOperation = new ApplicationOperation();
        } else {
            let id = this._router.snapshot.paramMap.get('id');
            this._operationsService.getApplicationOperation(id).subscribe(
                (data: any) => {
                    this.applicationOperation = data.body.value;
                }
            );
        } 
    }
    getApplicationPermissions() {
        this._permissionsService.getApplicationPermissions().subscribe(
            (data: any) => {
                this.applicationPermissions = data.body.value;
            }
        );
    }
    onSave() {
        if (this.add) {
            this._operationsService.addApplicationOperation(this.applicationOperation).subscribe(
                () => {
                    this._location.back();
                }
            );
        } else {
            this._operationsService.editApplicationOperation(this.applicationOperation).subscribe(
                () => {
                    this._location.back();
                }
            );
        }
    }
    trackPermission(x: ApplicationOperation, y: ApplicationOperation) {
        return x.id === y.id;
    }
    updateOperationsInStorage() {
        localStorage.removeItem('operations');
        this._operationsService.getApplicationOperations().subscribe(
            (data: any) => {
                localStorage.setItem('operations', JSON.stringify(data.body.value));
            }
        );
    }
}
