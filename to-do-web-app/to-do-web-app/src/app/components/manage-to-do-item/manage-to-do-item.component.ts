import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { ToDoItem } from '../../models/to-do-item.model';
import { ToDoItemService } from '../../services/to-do-item.service';
import { CommunicationService } from '../../services/communication.service';


@Component({
  selector: 'app-manage-to-do-item',
  templateUrl: './manage-to-do-item.component.html',
  styleUrls: ['./manage-to-do-item.component.css']
})
export class ManageToDoItemComponent implements OnInit, OnDestroy {

  @Input() editPage: boolean;
  @Input() item: ToDoItem;
  @Input() toDoListId: string;

  @Output() itemEvent = new EventEmitter<ToDoItem>();
  subscription: Subscription;

  constructor(private itemService: ToDoItemService, private commService: CommunicationService) { }

  ngOnInit() {

    if(this.item == undefined) {
      this.item = new ToDoItem();
    }

    this.subscription = this.commService.getToDoList().subscribe(
      data => {
        this.toDoListId = data.id;
        this.itemService.createToDoItem(this.toDoListId, this.item).subscribe(
          data => {
            this.itemEvent.emit(data); //update item collection
            this.item.content = "";
          }
        ); 
      }
    );
  }

  updateToDoItem(): void {
    
    this.itemService.updateToDoItem(this.toDoListId, this.item).subscribe(
      data => {
        this.item = data;        
      }
    )
  }

  removeToDoItem(): void {

    if (window.confirm('Are you sure you want do remove this item?')) {
      this.itemService.removeToDoItem(this.toDoListId, this.item.itemId).subscribe(
        () => {
          this.itemEvent.emit(this.item); //update item collection
        }
      ); 
    } 

  }

  createToDoItem(): void {
    
    if(this.item.content == undefined) {
      return;
    }

    //create the list first if it is undefined
    if(this.toDoListId == undefined) {
      this.commService.createEmptyList();
    } else {
      this.itemService.createToDoItem(this.toDoListId, this.item).subscribe(
        data => {
          this.itemEvent.emit(data); //update item collection
          this.item.content = "";
        }
      ); 
    }
    
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
