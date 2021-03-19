import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'University Moodle';
  users: any = {};
  readonly baseUrl = 'https://localhost:5001/api/users/';

  constructor(private http: HttpClient) {}
  ngOnInit() {
    this.getUsers();
  }

  getUsers(){
    this.http.get(this.baseUrl).subscribe(response => {
      this.users = response;
    }, error => {
      console.error('Error: '+error);
    });
  }
}
