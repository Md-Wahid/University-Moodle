import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cencelRegister = new EventEmitter();
  model: any = {};

  constructor(private accoutnService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    this.accoutnService.register(this.model).subscribe( response => {
      console.log(response);
      this.cencel();
    }, error => {
      console.log(error);
    })
  }

  cencel() {
    this.cencelRegister.emit(false);
  }

}
