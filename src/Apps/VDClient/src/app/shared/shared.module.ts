import { NgModule } from '@angular/core';

import {
    NbSidebarModule,
    NbMenuModule,
    NbLayoutModule,
    NbIconModule,
    NbSearchModule,
    NbActionsModule,
    NbButtonModule,
    NbCardModule,
    NbInputModule,
    NbToastrModule
} from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { HeaderComponent } from './components/header/header.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
    imports: [
        NbSidebarModule,
        NbLayoutModule,
        NbMenuModule,
        NbIconModule,
        NbEvaIconsModule,
        NbSearchModule,
        NbActionsModule,
        NbButtonModule,
        NbCardModule,
        NbInputModule,
        NbToastrModule,
        FormsModule,
        ReactiveFormsModule
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
        NbActionsModule,
        NbButtonModule,
        NbCardModule,
        NbInputModule,
        NbToastrModule,
        FormsModule,
        ReactiveFormsModule
    ],
    declarations: [HeaderComponent]
})
export class SharedModule { }
