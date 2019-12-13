import { Component, Injectable, OnInit } from '@angular/core';
import { OperationsService } from './Services/operations.service';
import { AuthService } from './Services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
@Injectable()
export class AppComponent implements OnInit {
    title = 'app';

    constructor(private _operationsService: OperationsService, private _authService: AuthService) {
    }

    ngOnInit() {
        if (this._authService.isLoggedIn()) {
          this._operationsService.getApplicationOperations().subscribe(
            (data: any) => {
                localStorage.setItem('operations', JSON.stringify(data.body.value));
            }
          );
        }
    }
}
