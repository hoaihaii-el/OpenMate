import { Component } from '@angular/core';

@Component({
    selector: 'usertask-cmp',
    moduleId: module.id,
    templateUrl: 'usertask.component.html',
    styleUrls: ['usertask.component.scss']
})

export class UserTaskComponent {
    public showModal: Boolean = false;

    openModal(): void {
        this.showModal = true;
    }

    closeModal(): void {
        this.showModal = false;
    }
}