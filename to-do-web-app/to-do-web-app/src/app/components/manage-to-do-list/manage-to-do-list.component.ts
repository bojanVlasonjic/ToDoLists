import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { ToDoListService } from '../../services/to-do-list.service';
import { CommunicationService } from '../../services/communication.service';

import { ToDoList } from '../../models/to-do-list.model';
import { ToDoItem } from '../../models/to-do-item.model';

@Component({
  selector: 'app-manage-to-do-list',
  templateUrl: './manage-to-do-list.component.html',
  styleUrls: ['./manage-to-do-list.component.css']
})
export class ManageToDoListComponent implements OnInit, OnDestroy {

  editPage: boolean;
  toDoList: ToDoList;
  subscription: Subscription;

  constructor(private route: ActivatedRoute, private listService: ToDoListService, private commService: CommunicationService) { }

  ngOnInit() {

    this.toDoList = new ToDoList();
    this.toDoList.dtoItems = [];

    this.route.params.subscribe(
      params => {
        if (params['id'] !== undefined) {
          this.loadList(params['id']);
          this.editPage = true;
        } else {
          this.editPage = false;
        }
      }
    );
      
    this.subscription = this.commService.getToDoList().subscribe(data => {
      this.toDoList = data;
    });


  }

  loadList(listId: string): void {

    this.listService.getToDoListById(listId).subscribe(
      data => {
        this.toDoList = data;

      // if the user opened an expired list, remove it's date and update i t
      if(this.toDoList.endDate != null) {
        this.toDoList.endDate = null; 
        this.executeApiCall(true);
      }

      },
      error => {
        console.log(error);
      }
    );
  }


  executeApiCall(isTitleValid: boolean): void {

    if(!isTitleValid) {
      return;
    }

    //to avoid server side bad requests, I must explicitly set the null value to the date(instead of "")
    if (!this.toDoList.endDate){
      this.toDoList.endDate = null;
    }
    
    if (this.toDoList.id !== undefined) { // if the id is defined, the user is updating the list
      this.listService.updateToDoList(this.toDoList).subscribe(data => {
        this.toDoList = data;
      });
    } else { //the user is creating the list
      this.listService.createToDoList(this.toDoList).subscribe(data => {
        this.toDoList = data;
      }); 
    }

  }

  updatePosition(item, position){

    this.commService.updatePosition(this.toDoList.id, item.itemId, position).subscribe(
      data => {
        this.toDoList.dtoItems.splice(this.toDoList.dtoItems.indexOf(item), 1); //remove item from old position
        item = data; //update item
        this.toDoList.dtoItems.splice(position, 0, item); //insert item in new position
      }
    );

  }

  get filterCompletedItems(): Array<ToDoItem> {
    return this.toDoList.dtoItems.filter(item => item.isCompleted);
  }

  get filterIncompletedItems(): Array<ToDoItem> {
    return this.toDoList.dtoItems.filter(item => !item.isCompleted);
  }

  getToday(): string {
    let strDateTime = new Date().toISOString().split('.')[0];
    return strDateTime.substr(0, strDateTime.lastIndexOf(':')); //removing seconds
  }

  addItemToList($event: ToDoItem): void {
    this.toDoList.dtoItems.push($event);
  }

  removeItemFromList($event: ToDoItem): void {
    this.toDoList.dtoItems.splice(this.toDoList.dtoItems.indexOf($event), 1);
  }


  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
