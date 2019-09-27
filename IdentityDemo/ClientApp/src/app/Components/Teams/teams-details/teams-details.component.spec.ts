import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsDetailsComponent } from './teams-details.component';

describe('TeamsDetailsComponent', () => {
  let component: TeamsDetailsComponent;
  let fixture: ComponentFixture<TeamsDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TeamsDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamsDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
