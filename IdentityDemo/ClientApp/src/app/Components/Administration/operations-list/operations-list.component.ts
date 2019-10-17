import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationOperation } from '../../../Models/Identity/ApplicationOperation';
import { OperationsService } from '../../../Services/operations.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-operations-list',
  templateUrl: './operations-list.component.html',
  styleUrls: ['./operations-list.component.css']
})
@Injectable()
export class OperationsListComponent implements OnInit {

    displayedColumns = ['name', 'isAvailableToAnonymoys', 'isAvailableToAllAuthorizedUsers'];
    datasource: ApplicationOperation[];

    constructor(private _operationsService: OperationsService, private _router: Router) { }

    ngOnInit() {
        this._operationsService.getApplicationOperations().subscribe(
            (data: any) => {
                this.datasource = data.body.value;
            }
        );
    }
    editOperation(row) {
        this._router.navigate(['/operation-edit', row.id]);
    }

    addOperation() {
        this._router.navigate(['/operation-add']);
    }
}
