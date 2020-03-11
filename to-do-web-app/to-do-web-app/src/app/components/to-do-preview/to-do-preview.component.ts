import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

import { ToDoList } from '../../models/to-do-list.model';
import { environment } from 'src/environments/environment';
import { ToDoListService } from 'src/app/services/to-do-list.service';
import { ToastService } from 'src/app/services/toast.service';


@Component({
  selector: 'app-to-do-preview',
  templateUrl: './to-do-preview.component.html',
  styleUrls: ['./to-do-preview.component.css']
})
export class ToDoPreviewComponent implements OnInit {

  @Input() toDoList: ToDoList;
  @Output() removeListEvent = new EventEmitter<ToDoList>();

  constructor(private router: Router, private service: ToDoListService, private toastService: ToastService) { }

  ngOnInit() {
  }

  removeToDoList(): void {

    if (window.confirm('Are you sure you want do remove this list?')) {
      this.removeListEvent.emit(this.toDoList);
    }

  }

  redirectToEditPage(listId: string): void {
    this.router.navigateByUrl(`/to-do-list/${listId}`);
  }

  shareToDoList() {

    this.service.createSharedList(this.toDoList.id).subscribe(
      data => {
        let linkToList = `${environment.domainUrl}/share/${data}`;
        navigator.clipboard.writeText(linkToList); //copy link to shared list to clipboard
      },
      error => {
        console.log(error.error);
      }
    )
    
  }

}
