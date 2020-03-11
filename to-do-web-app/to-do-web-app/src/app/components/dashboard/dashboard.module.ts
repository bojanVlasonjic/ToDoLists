import { NgModule } from '@angular/core';
import { DashboardComponent } from './dashboard.component';
import { ToDoPreviewComponent } from '../to-do-preview/to-do-preview.component';
import { BrowserModule } from '@angular/platform-browser';
import { DndModule } from 'ng2-dnd';
import { ToDoListShareComponent } from '../to-do-list-share/to-do-list-share.component';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
    declarations: [
        DashboardComponent,
        ToDoPreviewComponent,
        ToDoListShareComponent
    ],
    imports: [
        BrowserModule,
        DndModule,
        RouterModule,
        HttpClientModule
    ]
})

export class DashboardModule { }
