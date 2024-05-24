import { Component } from '@angular/core';

@Component({
    selector: 'userinfos-cmp',
    moduleId: module.id,
    templateUrl: 'userinfos.component.html',
    styleUrls: ['userinfos.component.scss']
})

export class UserInfosComponent {
    public modalTitle: String = 'Thêm nhân viên';
    public showModal: Boolean = false;
    openModal() {
        this.showModal = true;
    }

    closeModal() {
        this.showModal = false;
    }
}