import { Component, OnInit, Injectable } from '@angular/core';
import { ApplicationUser } from '../../Models/Identity/ApplicationUser';
import { UsersService } from '../../Services/users.service';

@Component({
  selector: 'app-create-admin',
  templateUrl: './create-admin.component.html',
  styleUrls: ['./create-admin.component.css']
})

@Injectable()

export class CreateAdminComponent implements OnInit {

    public applicationUser: ApplicationUser;
    private usersService: UsersService;

    constructor(_usersService: UsersService) {
        this.usersService = _usersService;
    }

    ngOnInit() {
        this.applicationUser = new ApplicationUser();
    }

    onSave() {
        this.usersService.CreateAdmin(this.applicationUser).subscribe();
    }
}
