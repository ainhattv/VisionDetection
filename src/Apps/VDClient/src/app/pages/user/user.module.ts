import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserComponent } from './user.component';
import { IndexComponent } from './index/index.component';
import { UserRoutingModule } from './user-routing.module';

import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [UserComponent, IndexComponent],
  imports: [
    CommonModule,
    UserRoutingModule,
    SharedModule
  ],
})
export class UserModule { }
