import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { ManageToDoListComponent } from './manage-to-do-list.component';
import { ManageToDoItemComponent } from '../manage-to-do-item/manage-to-do-item.component';
import { DndModule } from 'ng2-dnd';


@NgModule({
    declarations: [
        ManageToDoListComponent,
        ManageToDoItemComponent
    ],
    imports: [
        BrowserModule,
        FormsModule,
        DndModule
    ],
    exports: []
})


export class ManageToDoListModule {

}
