import { Component } from '@angular/core';

@Component({
    selector: 'devices-cmp',
    moduleId: module.id,
    templateUrl: 'devices.component.html'
})

export class DevicesComponent {
    public modalTitle: String = 'Thêm thiết bị';
    openModal() {
        document.getElementById('exampleModal').style.display = 'block';
    }

    closeModal() {
        document.getElementById('exampleModal').style.display = 'none';
    }
}