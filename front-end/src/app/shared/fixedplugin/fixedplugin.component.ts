import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Task } from 'app/models/task.model';
import { get } from 'http';

@Component({
  moduleId: module.id,
  selector: 'fixedplugin-cmp',
  templateUrl: 'fixedplugin.component.html',
  styleUrls: ['fixedplugin.component.scss']
})

export class FixedPluginComponent {
  public tasks: Task[] = [];
  public newTask: Task;

  constructor(private httpClient: HttpClient) {
    this.getUserTasks();
    this.newTask = {
      date: this.dateToString(new Date()).replace('%2F', '/'),
      staffID: "24002",
      staffName: "",
      order: 0,
      taskName: "",
      status: "",
      estimate: "",
      note: "",
      evaluate: "",
      feedback: "",
      startTime: "",
      endTime: ""
    }
  }

  getUserTasks() {
    var date = this.dateToString(new Date());
    this.httpClient.get(`https://localhost:7243/api/TaskDetail/user-get?date=${date}&staffID=24002`)
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

  addNewTask() {
    if (this.newTask.taskName == "" || this.newTask.estimate == "") {
      return;
    }

    const now = new Date();
    this.httpClient.post(`https://localhost:7243/api/TaskDetail/new-task`, {
      date: this.newTask.date,
      staffID: this.newTask.staffID,
      staffName: this.newTask.staffName,
      order: this.tasks.length + 1,
      taskName: this.newTask.taskName,
      status: "In Progress",
      estimate: Number(this.newTask.estimate),
      note: this.newTask.note,
      evaluate: 50,
      feedback: this.newTask.feedback,
      startTime: `${now.getHours()}h${now.getMinutes()}`,
      endTime: this.newTask.endTime
    })
      .subscribe({
        next: (res: any) => {
          console.log(res);
        },
        error: (error: any) => {
          console.log(error)
        }
      });
    this.getUserTasks();
    this.newTask.taskName = "";
    this.newTask.estimate = "";
  }

  dateToString(date: Date): string {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const year = date.getFullYear().toString();
    return `${day}%2F${month}%2F${year}`;
  }

  updateTask(task: Task, isTimeChanged: boolean) {
    const now = new Date();
    console.log(task);
    this.httpClient.post(`https://localhost:7243/api/TaskDetail/new-task`, {
      date: task.date,
      staffID: task.staffID,
      staffName: task.staffName,
      order: Number(task.order),
      taskName: task.taskName,
      status: task.status,
      estimate: Number(task.estimate),
      note: task.note,
      evaluate: 50,
      feedback: task.feedback,
      startTime: task.startTime,
      endTime: task.status == "Done" && isTimeChanged ? `${now.getHours()}h${now.getMinutes()}` : task.endTime
    })
      .subscribe({
        next: (res: any) => {
          console.log(res);
        },
        error: (error: any) => {
          console.log(error)
        }
      });
    this.getUserTasks();
  }
}
