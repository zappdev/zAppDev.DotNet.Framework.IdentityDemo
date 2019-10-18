import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { PlayersListComponent } from './Components/Players/players-list/players-list.component';
import { PlayersDetailsComponent } from './Components/Players/players-details/players-details.component';
import { TeamsListComponent } from './Components/Teams/teams-list/teams-list.component';
import { TeamsDetailsComponent } from './Components/Teams/teams-details/teams-details.component';
import { CreateAdminComponent } from './Components/create-admin/create-admin.component';
import { SignInComponent } from './Components/SignIn/sign-in.component';
import { UsersListComponent } from './components/Administration/users-list/users-list.component';
import { UsersDetailsComponent } from './components/Administration/users-details/users-details.component';
import { PermissionsListComponent } from './Components/Administration/permissions-list/permissions-list.component';
import { PermissionsDetailsComponent } from './Components/Administration/permissions-details/permissions-details.component';
import { RolesListComponent } from './Components/Administration/roles-list/roles-list.component';
import { RolesDetailsComponent } from './Components/Administration/roles-details/roles-details.component';
import { OperationsListComponent } from './Components/Administration/operations-list/operations-list.component';
import { OperationsDetailsComponent } from './Components/Administration/operations-details/operations-details.component';
import { UnauthorizedComponent } from './Components/unauthorized/unauthorized.component';
import { MatNativeDateModule, MatGridListModule, MatFormFieldModule, MatListModule, MatCardModule, MatTableModule, MatIconModule, MatInputModule, MatButtonModule, MatDatepickerModule, MatSelectModule, MatCheckboxModule, MatMenuModule } from '@angular/material';
import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AccessGuard } from './Helpers/AccessGuard';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PlayersListComponent,
    PlayersDetailsComponent,
    TeamsListComponent,
    TeamsDetailsComponent,
    CreateAdminComponent,
    SignInComponent,
    UsersListComponent,
    UsersDetailsComponent,
    PermissionsListComponent,
    PermissionsDetailsComponent,
    RolesListComponent,
    RolesDetailsComponent,
    OperationsListComponent,
    OperationsDetailsComponent,
    UnauthorizedComponent
  ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        HttpClientModule,
        FormsModule,
        MatNativeDateModule,
        MatGridListModule,
        MatFormFieldModule,
        MatListModule,
        MatCardModule,
        MatTableModule,
        MatIconModule,
        MatInputModule,
        MatButtonModule,
        MatDatepickerModule,
        MatSelectModule,
        MatCheckboxModule,
        MatMomentDateModule,
        MatMenuModule,
        BrowserAnimationsModule,
        RouterModule.forRoot([
      { path: '', component: PlayersListComponent, pathMatch: 'full', canActivate: [AccessGuard]},
      { path: 'players', component: PlayersListComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'player-add', component: PlayersDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'player-edit/:id', component: PlayersDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'teams', component: TeamsListComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'team-add', component: TeamsDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'team-edit/:id', component: TeamsDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'users', component: UsersListComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'user-add', component: UsersDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'user-edit/:id', component: UsersDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'permissions', component: PermissionsListComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'permission-add', component: PermissionsDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'permission-edit/:id', component: PermissionsDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'roles', component: RolesListComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'role-add', component: RolesDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'role-edit/:id', component: RolesDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'operations', component: OperationsListComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'operation-add', component: OperationsDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'operation-edit/:id', component: OperationsDetailsComponent, pathMatch: 'full', canActivate: [AccessGuard] },
      { path: 'createAdmin', component: CreateAdminComponent, pathMatch: 'full' },
      { path: 'signIn', component: SignInComponent, pathMatch: 'full' },
      { path: 'unauthorized', component: UnauthorizedComponent, pathMatch: 'full' }
    ])
  ],
  providers: [
      { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
      AccessGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
