import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationUser } from '../../Models/Identity/ApplicationUser';
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';
import { UsersService } from '../../Services/users.service';
import { resolve, reject } from 'q';
import { OperationsService } from '../../Services/operations.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})

@Injectable()

export class SignInComponent implements OnInit {

    public appUser: ApplicationUser;

    constructor(private _authService: AuthService, private _router: Router, private _usersService: UsersService, private _operationsService: OperationsService) {
    }

    ngOnInit() {
        this.appUser = new ApplicationUser();
    }

    signIn() {
        this._authService.signIn(this.appUser.username, this.appUser.password)
            .then(
                () => {
                    this._usersService.getApplicationUser(this.appUser.username).subscribe(
                        (data: any) => {
                            localStorage.setItem('applicationUser', JSON.stringify(data.body.value));
                        }
                    );
                    this._operationsService.getApplicationOperations().subscribe(
                        (data: any) => {
                            localStorage.setItem('operations', JSON.stringify(data.body.value));
                        }
                    );
                    setTimeout(() => {
                        this.redirect();
                    }, 1000)
                }
            );
            //.then(() => {  });
            
        /*setTimeout(() => {
            this.redirect();
        }, 1000);*/
       // setInterval(this.redirect, 700);
    }

    redirect(): void {
        this._router.navigate(['/players']);
    }
}
