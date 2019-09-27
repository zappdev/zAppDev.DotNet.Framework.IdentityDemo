import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersDetailsComponent } from './players-details.component';

describe('PlayersDetailsComponent', () => {
  let component: PlayersDetailsComponent;
  let fixture: ComponentFixture<PlayersDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayersDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
