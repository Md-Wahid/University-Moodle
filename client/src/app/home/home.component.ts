import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  // @Input() usersFromHomeComponent: any;
  registerMode = false;

  constructor() { }

  ngOnInit(): void {
  }

  registerToogle(){
    this.registerMode = !this.registerMode;
  }

  cencelRegisterMode(event: boolean) {
    this.registerMode = event;
  }

}
