import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToDoListService } from 'src/app/services/to-do-list.service';
import { ToDoList } from 'src/app/models/to-do-list.model';

@Component({
  selector: 'app-to-do-list-share',
  templateUrl: './to-do-list-share.component.html',
  styleUrls: ['./to-do-list-share.component.css']
})

export class ToDoListShareComponent implements OnInit {

  sharedList: ToDoList;

  constructor(private route: ActivatedRoute, private service: ToDoListService) { }

  ngOnInit() {

    this.route.params.subscribe(
      params => {
        if (params['id'] !== undefined) {
          this.loadSharedList(params['id']);
        } 
      }
    );
  }


  loadSharedList(shareId: string) {

    this.service.getSharedList(shareId).subscribe(
      data => {
        this.sharedList = data;
      },
      error => {
        console.log(error.error);
      }
    )

  }

}
