import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { ToDoList } from '../../models/to-do-list.model';
import { ToDoListService } from '../../services/to-do-list.service';
import { CommunicationService } from 'src/app/services/communication.service';
import { ToastService } from '../../services/toast.service';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {

  toDoLists: ToDoList[];
  subscription: Subscription;

  constructor(
    private toastService: ToastService,
    private listService: ToDoListService,
    private commService: CommunicationService
    ) { }

  ngOnInit() {

    (document.getElementById('search-field') as HTMLElement).style.visibility = 'visible';

    this.getToDoLists();
    this.subscription = this.commService.getSearchedLists().subscribe(
      data => {
        this.toDoLists = data;
      }
    );

    
  }

  getToDoLists(): void {
    this.listService.getToDoLists()
      .subscribe(
        data => this.toDoLists = data
      );
  }

  removeToDoList($event: ToDoList): void {
    this.listService.removeToDoList($event.id).subscribe(
      () => {
        this.toDoLists.splice(this.toDoLists.indexOf($event), 1);
      }
    );
  }

  updatePosition(list: ToDoList, position: number) {

    if((document.getElementById('search-field') as HTMLInputElement).value != '') {
      this.toastService.showMessage('Swaping positions not allowed during search');
      return;
    }

    this.listService.updatePosition(list.id, position).subscribe(
      data => {
        this.toDoLists.splice(this.toDoLists.indexOf(list), 1); //remove item from current position
        list = data;
        this.toDoLists.splice(this.toDoLists.length-position, 0, list); //insert item in new, reversed order, position
      },
      error => {
        console.log(error);
      }
    )
  }

  ngOnDestroy() {
    (document.getElementById('search-field') as HTMLElement).style.visibility = 'hidden';

    if(this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
    
  }

}
