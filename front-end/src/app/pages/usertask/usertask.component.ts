import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TaskService } from 'app/hubs/task.service';
import { Task } from 'app/models/task.model';

@Component({
    selector: 'usertask-cmp',
    moduleId: module.id,
    templateUrl: 'usertask.component.html',
    styleUrls: ['usertask.component.scss']
})

export class UserTaskComponent implements OnInit {
    public showModal: Boolean = false;
    public tasks: Task[] = [];
    public currentDate: string;
    public currentStaffID: string;

    constructor(private httpClient: HttpClient, private taskService: TaskService) {
        this.currentStaffID = localStorage.getItem('userID');
        this.currentDate = this.dateToString(new Date());
        this.getStaffsTasks();
    }

    ngOnInit(): void {
        this.taskService.startConnection();
        this.taskService.addListener((message: string) => {
            console.log(message);
            this.getStaffsTasks();
        });
    }

    getStaffsTasks() {
        var date = this.currentDate.replace('/', '%2F');
        date = date.replace('/', '%2F');
        this.httpClient.get(`https://localhost:7243/api/TaskDetail/manager-get?date=${date}&managerID=${this.currentStaffID}`)
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

    updateTask(task: Task) {
        this.httpClient.post(`https://localhost:7243/api/TaskDetail/new-task`, {
            date: task.date,
            staffID: task.staffID,
            staffName: task.staffName,
            order: Number(task.order),
            taskName: task.taskName,
            status: task.status,
            estimate: Number(task.estimate),
            note: task.note,
            evaluate: Number(task.evaluate),
            feedback: task.feedback,
            startTime: task.startTime,
            endTime: task.endTime
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
        this.getStaffsTasks();
    }

    previousDay() {
        var date = this.stringToDate(this.currentDate);
        date.setDate(date.getDate() - 1);
        this.currentDate = this.dateToString(date);
        this.getStaffsTasks();
    }

    nextDay() {
        var date = this.stringToDate(this.currentDate);
        const curDate = new Date();
        if (date.getDate() == curDate.getDate() && date.getMonth() == curDate.getMonth() && date.getFullYear() == curDate.getFullYear()) {
            return;
        }
        date.setDate(date.getDate() + 1);
        this.currentDate = this.dateToString(date);
        this.getStaffsTasks();
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