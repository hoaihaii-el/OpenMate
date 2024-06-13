import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  public userName: string = '';
  public password: string = '';

  constructor(private router: Router, private httpClient: HttpClient, private toastr: ToastrService) { }

  checkLogin(): void {
    if (this.userName === '' || this.password === '') {
      this.showAlert('Please enter username and password', 'info');
      return;
    }

    this.httpClient.post('https://localhost:7243/api/Account/login', { userID: this.userName, password: this.password })
      .subscribe({
        next: (res: any) => {
          if (res) {
            localStorage.setItem('token', res.accessToken);
            localStorage.setItem('userID', res.staffID);
            localStorage.setItem('roles', res.roles);
            localStorage.setItem('userName', res.staffName);
            this.router.navigate(['/dashboard']);
          }
        },
        error: (error: any) => {
          this.showAlert(error.error, 'error');
        }
      });

    this.router.navigate(['/dashboard']);
  }

  showAlert(content: string, type: string): void {
    switch (type) {
      case 'info':
        this.toastr.info(
          `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
          "",
          {
            timeOut: 4000,
            closeButton: true,
            enableHtml: true,
            toastClass: "alert alert-info alert-with-icon",
            positionClass: "toast-" + 'top' + "-" + 'left'
          }
        );
        break;
      case 'success':
        this.toastr.success(
          `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
          "",
          {
            timeOut: 4000,
            closeButton: true,
            enableHtml: true,
            toastClass: "alert alert-success alert-with-icon",
            positionClass: "toast-" + 'top' + "-" + 'left'
          }
        );
        break;
      case 'error':
        this.toastr.success(
          `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
          "",
          {
            timeOut: 4000,
            closeButton: true,
            enableHtml: true,
            toastClass: "alert alert-danger alert-with-icon",
            positionClass: "toast-" + 'top' + "-" + 'left'
          }
        );
        break;
    }
  }
}
