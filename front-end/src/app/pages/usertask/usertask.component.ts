import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Task } from 'app/models/task.model';

@Component({
    selector: 'usertask-cmp',
    moduleId: module.id,
    templateUrl: 'usertask.component.html',
    styleUrls: ['usertask.component.scss']
})

export class UserTaskComponent {
    public showModal: Boolean = false;
    public tasks: Task[] = [];
    public currentDate: string;

    constructor(private httpClient: HttpClient) {
        this.currentDate = this.dateToString(new Date());
    }

    openModal(): void {
        this.showModal = true;
    }

    closeModal(): void {
        this.showModal = false;
    }

    getUserTasks() {
        var date = this.dateToString(new Date());
        this.httpClient.get(`http://localhost:5299/api/TaskDetail/user-get?date=${date}&staffID=24002`)
          .subscribe({
            next: (res: any) => {
              console.log(res);
              this.tasks = res;
            },
            error: (error: any) => {
              console.log(error)
            }
          });
      }

    previousDay() {
        var date = this.stringToDate(this.currentDate);
        date.setDate(date.getDate() - 1);
        this.currentDate = this.dateToString(date);
    }

    nextDay() {
        var date = this.stringToDate(this.currentDate);
        const curDate = new Date();
        if (date.getDate() == curDate.getDate() && date.getMonth() == curDate.getMonth() && date.getFullYear() == curDate.getFullYear()) {
            return;
        }
        date.setDate(date.getDate() + 1);
        this.currentDate = this.dateToString(date);
    }

    dateToString(date: Date): string {
        const day = date.getDate().toString().padStart(2, '0');
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const year = date.getFullYear().toString();
        return `${day}/${month}/${year}`;
    }

    stringToDate(dateString: string): Date {
        const [day, month, year] = dateString.split('/').map(Number);
        return new Date(year, month - 1, day);
    }
}