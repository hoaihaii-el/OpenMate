import { Component, OnInit } from '@angular/core';
import { Task } from 'app/models/task.model';

@Component({
  moduleId: module.id,
  selector: 'fixedplugin-cmp',
  templateUrl: 'fixedplugin.component.html'
})

export class FixedPluginComponent implements OnInit {
  public tasks: Task[];
  ngOnInit() {
    this.tasks = [
      { name: 'Task1', time: '13:00', done: false },
      { name: 'Task2', time: '14:00', done: false },
      { name: 'Task3', time: '15:00', done: false }
    ]
  }

  addNewTask(nameInput: HTMLInputElement, timeInput: HTMLInputElement) {
    this.tasks.push({ name: nameInput.value, time: timeInput.value, done: false });
    nameInput.value = '';
    timeInput.value = '';
  }
}
