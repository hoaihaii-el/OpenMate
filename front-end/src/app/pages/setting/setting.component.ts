import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Setting } from 'app/models/setting.model';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'structure-cmp',
    moduleId: module.id,
    templateUrl: 'setting.component.html',
    styleUrls: ['setting.component.scss']
})

export class SettingComponent {
    public isAddNewSetting: boolean = false;
    public settings: Setting[] = [];
    public newKey: string = '';
    public newValue: string = '';
    public newType: string = 'Quy định';

    constructor(private httpClient: HttpClient, private toastr: ToastrService) {
        this.getSettings();
    }

    public addNewSetting() {
        this.isAddNewSetting = !this.isAddNewSetting;
    }

    getSettings() {
        const url = 'https://localhost:7243/api/Settings/get-all';
        this.httpClient.get(url)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.settings = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    updateSetting() {
        if (this.newKey !== '' || this.newValue !== '') {
            this.settings.push({ key: this.newKey, value: this.newValue, type: this.newType });
        }

        const url = 'https://localhost:7243/api/Settings/update-all';
        this.httpClient.post(url, this.settings)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.getSettings();
                    this.showAlert('Update successfully', 'success');
                    this.newKey = '';
                    this.newValue = '';
                    this.newType = 'Quy định';
                    this.isAddNewSetting = false;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    showAlert(content: string, type: string): void {
        switch (type) {
            case 'info':
                this.toastr.info(
                    `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
                    "",
                    {
                        timeOut: 4000,
                        closeButton: true,
                        enableHtml: true,
                        toastClass: "alert alert-info alert-with-icon",
                        positionClass: "toast-" + 'top' + "-" + 'left'
                    }
                );
                break;
            case 'success':
                this.toastr.success(
                    `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
                    "",
                    {
                        timeOut: 4000,
                        closeButton: true,
                        enableHtml: true,
                        toastClass: "alert alert-success alert-with-icon",
                        positionClass: "toast-" + 'top' + "-" + 'left'
                    }
                );
                break;
        }
    }
}