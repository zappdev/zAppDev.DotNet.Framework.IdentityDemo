import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionsDetailsComponent } from './permissions-details.component';

describe('PermissionsDetailsComponent', () => {
  let component: PermissionsDetailsComponent;
  let fixture: ComponentFixture<PermissionsDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PermissionsDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PermissionsDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
