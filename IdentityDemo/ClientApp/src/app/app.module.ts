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
import { MatNativeDateModule, MatGridListModule, MatFormFieldModule, MatListModule, MatCardModule, MatTableModule, MatIconModule, MatInputModule, MatButtonModule, MatDatepickerModule, MatSelectModule, MatCheckboxModule } from '@angular/material';
import { MatMomentDateModule, MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    PlayersListComponent,
    PlayersDetailsComponent,
    TeamsListComponent,
    TeamsDetailsComponent
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
    BrowserAnimationsModule,
    RouterModule.forRoot([
      { path: '', component: PlayersListComponent, pathMatch: 'full' },
      { path: 'players', component: PlayersListComponent, pathMatch: 'full' },
      { path: 'player-add', component: PlayersDetailsComponent, pathMatch: 'full' },
      { path: 'player-edit/:id', component: PlayersDetailsComponent, pathMatch: 'full' },
      { path: 'teams', component: TeamsListComponent, pathMatch: 'full' },
      { path: 'team-add', component: TeamsDetailsComponent, pathMatch: 'full' },
      { path: 'team-edit/:id', component: TeamsDetailsComponent, pathMatch: 'full' }
    ])
  ],
  providers: [
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
