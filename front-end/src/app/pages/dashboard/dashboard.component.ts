import { Component, OnChanges, OnInit } from '@angular/core';
import { Notification } from 'app/models/notifications.model';

@Component({
  selector: 'dashboard-cmp',
  moduleId: module.id,
  templateUrl: 'dashboard.component.html'
})

export class DashboardComponent implements OnInit {
  public checkIn: String = "__:__";
  public checkOut: String = "__:__";
  public notifications: Notification[];

  ngOnInit() {
    this.notifications = [
      { id: 1, name: 'Thông báo thay đổi nhân sự team The First Shark', level: 'Must Read', date: '29/03/2024', content: '123' },
      { id: 2, name: 'Thông báo thay đổi quy định quản lý cơ sở vật chất', level: 'Important', date: '28/03/2024', content: '123' },
      { id: 3, name: 'Thông báo về giải đấu VINASA Cup', level: 'Normal', date: '27/03/2024', content: '123' }
    ];

    this.checkIn = "__:__";
    this.checkOut = "18:00";
  }

  onSelect(notification: Notification): void {
    console.log(notification.name);
  }

  doCheckIn(): void {
    this.checkIn = "09:00";
  }

  doCheckOut(): void {
    this.checkOut = "18:00";
  }
}
