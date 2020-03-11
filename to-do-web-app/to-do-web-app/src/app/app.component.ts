import { Component, OnInit } from '@angular/core';
import { CommunicationService } from './services/communication.service';
import { AuthService } from './services/auth.service';
import { fromEventPattern } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {

  title = 'to-do-web-app';
  searchValue: string;
  username: string;

  constructor(private commService: CommunicationService, private auth: AuthService) {
    this.auth.handleLoginCallback(); 
  }

  ngOnInit() {
  }

  searchToDoLists() {

    if(this.searchValue == undefined) {
      return;
    }

    if(this.searchValue == "") {
      this.commService.getToDoLists();
    } else {
      this.commService.searchToDoLists(this.searchValue);
    }

  }

  get getUsername():string {
    return this.auth.getUserName();
  }

  clearSearchInput() {
    this.searchValue = "";
  }


}
