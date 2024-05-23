import { Component, OnInit } from '@angular/core';
import { Notification } from 'app/models/notifications.model';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'notifications-cmp',
  moduleId: module.id,
  templateUrl: 'notifications.component.html',
  styleUrls: ['notifications.component.scss']
})

export class NotificationsComponent implements OnInit {
  constructor(private httpClient: HttpClient, private sanitizer: DomSanitizer) { }

  public currentNoti: Notification;
  public notifications: Notification[];
  public sanitizerHtml: any;

  ngOnInit() {
    this.getNotifications();
  }

  onSelect(notification: Notification): void {
    this.currentNoti = notification;
    this.sanitizerHtml = this.sanitizer.bypassSecurityTrustHtml(this.currentNoti.content);
  }

  getNotifications(): void {
    const apiUrl = `http://localhost:5299/api/Notification/get-all`;
    this.httpClient.get(apiUrl)
      .subscribe({
        next: (response: any) => {
          console.log(response);
          this.notifications = response;
          this.currentNoti = this.notifications[0];
          this.sanitizerHtml = this.sanitizer.bypassSecurityTrustHtml(this.currentNoti.content);
          console.log(this.notifications);
        },
        error: (error: any) => {
          console.log(error);
        }
      });
  }
}
