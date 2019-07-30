import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-createworkplace',
  templateUrl: './createworkplace.component.html',
  styleUrls: ['./createworkplace.component.scss']
})
export class CreateworkplaceComponent implements OnInit {

  public wpName: string = '';

  constructor() { }

  ngOnInit() {
    this.wpName = '';
  }

}
