import { Component, OnInit } from '@angular/core';
import { MENU_ITEMS } from './user-menu';
import { Router, ActivatedRoute } from '@angular/router';
import { NbSidebarService } from '@nebular/theme';
import { first } from 'rxjs/operators';

import { WorkplaceService } from '../../core/services/workplace.service';
import { AuthenticationService } from '../../core/services/authentication.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  public menu = MENU_ITEMS;

  constructor(
    private sidebarService: NbSidebarService,
    private workplaceService: WorkplaceService,
    private authService: AuthenticationService,
    private router: Router,
  ) {
  }

  ngOnInit() {
    this.workplaceService.loadWorkPlace(this.authService.currentUserValue.id)
      .subscribe(
        next => {
          this.checkValidWorkPlace();
        },
        error => {
          this.router.navigate(['login']);
        }
      );
  }

  checkValidWorkPlace() {
    if (this.workplaceService.workplace === undefined || this.workplaceService.workplace === null) {
      this.router.navigate(['user/createworkplace']);
      console.log('createworkplace');
    } else {
      this.router.navigate(['user/index']);
    }
  }

  toggle() {
    this.sidebarService.toggle(true);
    return false;
  }

}
