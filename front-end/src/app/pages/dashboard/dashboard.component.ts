import { Component, OnChanges, OnInit } from '@angular/core';
import { Notification } from 'app/models/notifications.model';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from "ngx-toastr";
import { TimeSheet } from 'app/models/timesheet.model';

@Component({
  selector: 'dashboard-cmp',
  moduleId: module.id,
  templateUrl: 'dashboard.component.html',
  styleUrls: ['dashboard.component.scss']
})

export class DashboardComponent {
  public staffID: String = "24001";
  public checkIn: String = "__:__";
  public checkOut: String = "__:__";
  public total: String = "__";
  public avg: String = "__";
  public notifications: Notification[];

  constructor(private httpClient: HttpClient, private toastr: ToastrService) {
    this.checkIn = "__:__";
    this.checkOut = "__:__"
    const now = new Date();
    const apiUrl = `http://localhost:5299/api/TimeSheet/get-sheet-by-day?staffID=${this.staffID}&day=${now.getDate()}&month=${now.getMonth() + 1}&year=${now.getFullYear()}`;
    this.httpClient.get(apiUrl)
      .subscribe({
        next: (res: any) => {
          console.log(res);
          const checkInDate = new Date(res.checkIn);
          const checkOutDate = new Date(res.checkOut);
          console.log(checkInDate);
          console.log(checkOutDate);
          if (checkInDate.getDate() == now.getDate() && checkInDate.getMonth() == now.getMonth() && checkInDate.getFullYear() == now.getFullYear()) {
            this.checkIn = `${checkInDate.getHours().toString().padStart(2, '0')}:${checkInDate.getMinutes().toString().padStart(2, '0')}`
          }

          if (checkOutDate.getDate() == now.getDate() && checkOutDate.getMonth() == now.getMonth() && checkOutDate.getFullYear() == now.getFullYear()) {
            this.checkOut = `${checkOutDate.getHours().toString().padStart(2, '0')}:${checkOutDate.getMinutes().toString().padStart(2, '0')}`
          }

          if (res.total > 0) {
            this.total = `${res.total}hrs`
          }
        },
        error: (error: any) => {
          console.log(error)
        }
      });

    const url2 = `http://localhost:5299/api/TimeSheet/get-avg-by-month/${now.getMonth() + 1}/${now.getFullYear()}?staffID=${this.staffID}`;
    this.httpClient.get(url2)
      .subscribe({
        next: (res: any) => {
          console.log(res);
          if (res > 0) {
            this.avg = `${res}hrs`
          }
        },
        error: (error: any) => {
          console.log(error)
        }
      });
    this.getNotifications();
  }

  getNotifications(): void {
    const apiUrl = `http://localhost:5299/api/Notification/get-all`;
    this.httpClient.get(apiUrl)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          this.notifications = response;
          console.log(this.notifications);
        },
        error: (error: any) => {
          console.log(error);
        }
      });
  }

  doCheckIn(): void {
    const apiUrl = `http://localhost:5299/api/TimeSheet/check-in?staffID=${this.staffID}`;
    this.httpClient.post(apiUrl, null)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          const now = new Date();
          this.checkIn = now.getHours().toString().padStart(2, '0') + ':' + now.getMinutes().toString().padStart(2, '0');
          this.showAlert('Check-in success!', 'success');
        },
        error: (error: any) => {
          console.log(error);
          this.showAlert('You have already checked-in!', 'info');
        }
      });
  }

  doCheckOut(): void {
    const apiUrl = `http://localhost:5299/api/TimeSheet/check-out?staffID=${this.staffID}`;
    this.httpClient.post(apiUrl, null)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          const now = new Date();
          this.checkOut = now.getHours().toString().padStart(2, '0') + ':' + now.getMinutes().toString().padStart(2, '0');
          this.showAlert('Check-out success!', 'success');
        },
        error: (error: any) => {
          console.log(error);
          this.showAlert(error.error, 'info');
        }
      });
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
    }
  }
}
