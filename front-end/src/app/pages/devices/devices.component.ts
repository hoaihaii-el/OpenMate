import { Component } from '@angular/core';

@Component({
    selector: 'devices-cmp',
    moduleId: module.id,
    templateUrl: 'devices.component.html'
})

export class DevicesComponent {
    public modalTitle: String = 'Thêm thiết bị';
    public showModal: Boolean = false;
    openModal() {
        this.showModal = true;
    }

    closeModal() {
        this.showModal = false;
    }
}