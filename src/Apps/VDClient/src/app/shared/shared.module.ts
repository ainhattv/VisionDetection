import { NgModule } from '@angular/core';

import {
    NbSidebarModule,
    NbMenuModule,
    NbLayoutModule,
    NbIconModule,
    NbSearchModule,
    NbActionsModule
} from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { HeaderComponent } from './components/header/header.component';

@NgModule({
    imports: [
        NbSidebarModule,
        NbLayoutModule,
        NbMenuModule,
        NbIconModule,
        NbEvaIconsModule,
        NbSearchModule,
        NbActionsModule
    ],
    providers: [],
    exports: [
        NbSidebarModule,
        NbLayoutModule,
        NbMenuModule,
        NbIconModule,
        NbEvaIconsModule,
        HeaderComponent,
        NbSearchModule,
        NbActionsModule
    ],
    declarations: [HeaderComponent]
})
export class SharedModule { }
