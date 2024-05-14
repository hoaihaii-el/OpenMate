import { Component, OnInit } from '@angular/core';
import { ToastrService } from "ngx-toastr";
import { Notification } from 'app/models/notifications.model';

@Component({
  selector: 'notifications-cmp',
  moduleId: module.id,
  templateUrl: 'notifications.component.html',
  styleUrls: ['notifications.component.scss']
})

export class NotificationsComponent implements OnInit {
  constructor(private toastr: ToastrService) { }

  public currentNoti: Notification;
  public notifications: Notification[];

  ngOnInit() {
    this.notifications = [
      { id: 1, name: 'Thông báo thay đổi nhân sự team The First Shark', level: 'Must Read', date: '29/03/2024', content: 'There were two things that were important to Tracey. The first was her dog. Anyone that had ever met Tracey knew how much she loved her dog. Most would say that she treated it as her child. The dog went everywhere with her and it had been her best friend for the past five years. The second thing that was important to Tracey, however, would be a lot more surprising to most people.' },
      { id: 2, name: 'Thông báo thay đổi quy định quản lý cơ sở vật chất', level: 'Important', date: '28/03/2024', content: 'Spending time at national parks can be an exciting adventure, but this wasnt the type of excitement she was hoping to experience. As she contemplated the situation she found herself in, she knew shed gotten herself in a little more than she bargained for. It was not often that she found herself in a tree staring down at a pack of wolves that were looking to make her their next meal' },
      { id: 3, name: 'Thông báo về giải đấu VINASA Cup', level: 'Normal', date: '27/03/2024', content: 'Time is all relative based on age and experience. When you are a child an hour is a long time to wait but a very short time when that’s all the time you are allowed on your iPad. As a teenager time goes faster the more deadlines you have and the more you procrastinate. As a young adult, you think you have forever to live and don’t appreciate the time you spend with others. As a middle-aged adult, time flies by as you watch your children grow up. And finally, as you get old and you have fewer responsibilities and fewer demands on you, time slows. You appreciate each day and are thankful you are alive. An hour is the same amount of time for everyone yet it can feel so different in how it goes by.' },
      { id: 2, name: 'Thông báo thay đổi quy định quản lý cơ sở vật chất', level: 'Important', date: '28/03/2024', content: 'Spending time at national parks can be an exciting adventure, but this wasnt the type of excitement she was hoping to experience. As she contemplated the situation she found herself in, she knew shed gotten herself in a little more than she bargained for. It was not often that she found herself in a tree staring down at a pack of wolves that were looking to make her their next meal' },
      {
        id: 3, name: 'Thông báo về giải đấu VINASA Cup', level: 'Normal', date: '27/03/2024', content: 'Dave wasnt exactly sure how he had ended up in this predicament. He ran through all the events that had lead to this current situation and it still didnt make sense. He wanted to spend some time to try and make sense of it all, but he had higher priorities at the moment. The first was how to get out of his current situation of being naked in a tree with snow falling all around and no way for him to get down' +
          'Hopes and dreams were dashed that day. It should have been expected, but it still came as a shock. The warning signs had been ignored in favor of the possibility, however remote, that it could actually happen. That possibility had grown from hope to an undeniable belief it must be destiny. That was until it wasnt and the hopes and dreams came crashing down.'
      }
    ];

    this.currentNoti = this.notifications[0];
  }

  onSelect(notification: Notification): void {
    this.currentNoti = notification;
  }

  showNotification(from, align) {
    const color = Math.floor(Math.random() * 5 + 1);

    switch (color) {
      case 1:
        this.toastr.info(
          '<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">Welcome to <b>Paper Dashboard Angular</b> - a beautiful bootstrap dashboard for every web developer.</span>',
          "",
          {
            timeOut: 4000,
            closeButton: true,
            enableHtml: true,
            toastClass: "alert alert-info alert-with-icon",
            positionClass: "toast-" + from + "-" + align
          }
        );
        break;
      case 2:
        this.toastr.success(
          '<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">Welcome to <b>Paper Dashboard Angular</b> - a beautiful bootstrap dashboard for every web developer.</span>',
          "",
          {
            timeOut: 4000,
            closeButton: true,
            enableHtml: true,
            toastClass: "alert alert-success alert-with-icon",
            positionClass: "toast-" + from + "-" + align
          }
        );
        break;
      case 3:
        this.toastr.warning(
          '<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">Welcome to <b>Paper Dashboard Angular</b> - a beautiful bootstrap dashboard for every web developer.</span>',
          "",
          {
            timeOut: 4000,
            closeButton: true,
            enableHtml: true,
            toastClass: "alert alert-warning alert-with-icon",
            positionClass: "toast-" + from + "-" + align
          }
        );
        break;
      case 4:
        this.toastr.error(
          '<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">Welcome to <b>Paper Dashboard Angular</b> - a beautiful bootstrap dashboard for every web developer.</span>',
          "",
          {
            timeOut: 4000,
            enableHtml: true,
            closeButton: true,
            toastClass: "alert alert-danger alert-with-icon",
            positionClass: "toast-" + from + "-" + align
          }
        );
        break;
      case 5:
        this.toastr.show(
          '<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">Welcome to <b>Paper Dashboard Angular</b> - a beautiful bootstrap dashboard for every web developer.</span>',
          "",
          {
            timeOut: 4000,
            closeButton: true,
            enableHtml: true,
            toastClass: "alert alert-primary alert-with-icon",
            positionClass: "toast-" + from + "-" + align
          }
        );
        break;
      default:
        break;
    }
  }
}
