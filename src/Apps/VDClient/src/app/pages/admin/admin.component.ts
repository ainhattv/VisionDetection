import { Component, OnInit } from '@angular/core';
import { MENU_ITEMS } from './admin-menu';
import { NbSidebarService } from '@nebular/theme';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {

  public menu = MENU_ITEMS;

  constructor(private sidebarService: NbSidebarService) {
  }

  toggle() {
    this.sidebarService.toggle(true);
    return false;
  }

  ngOnInit() {
  }

}
