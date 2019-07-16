import { Component, OnInit } from '@angular/core';
import { MENU_ITEMS } from './user-menu';
import { NbSidebarService } from '@nebular/theme';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent {

  public menu = MENU_ITEMS;

  constructor(private sidebarService: NbSidebarService) {
  }

  toggle() {
    this.sidebarService.toggle(true);
    return false;
  }

}
