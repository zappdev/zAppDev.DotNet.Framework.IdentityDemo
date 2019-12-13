import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationUser } from '../../Models/Identity/ApplicationUser';
import { AuthService } from '../../Services/auth.service';
import { Router } from '@angular/router';
import { UsersService } from '../../Services/users.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})

@Injectable()

export class SignInComponent implements OnInit {

    private _authSerive: AuthService;
    private _router: Router;
    public appUser: ApplicationUser;

    constructor(usersService: AuthService, router: Router, private _usersService: UsersService) {
        this._authSerive = usersService;
        this._router = router;
    }

    ngOnInit() {
        this.appUser = new ApplicationUser();
    }

    signIn() {
        this._authSerive.signIn(this.appUser.username, this.appUser.password);
        this._usersService.getApplicationUser(this.appUser.username).subscribe(
            (data: any) => {
                localStorage.setItem('applicationUser', JSON.stringify(data.body.value));
            },
            () => { },
            () => {
                this.redirect();
            }
        );
        /*setTimeout(() => {
            this.redirect();
        }, 1000);*/
       // setInterval(this.redirect, 700);
    }

    redirect(): void {
        this._router.navigate(['/players']);
    }
}
