import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Staff } from 'app/models/staff.model';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'user-cmp',
    moduleId: module.id,
    templateUrl: 'user.component.html',
    styleUrls: ['user.component.scss']
})

export class UserComponent {
    public staff: Staff;
    public staffID: string;
    constructor(private httpClient: HttpClient, private toastr: ToastrService) {
        this.staffID = localStorage.getItem('userID');
        this.getStaffInfo();
    }

    getStaffInfo() {
        const apiUrl = `http://localhost:5299/api/Staff/detail?staffID=${this.staffID}`;
        this.httpClient.get(apiUrl)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.staff = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    stringToDate(dateStr: string): Date {
        return new Date(dateStr);
    }

    updateInfo() {
        const apiUrl = `http://localhost:5299/api/Staff/user-update`;
        this.httpClient.put(apiUrl, {
            staffID: this.staff.staffID,
            staffName: this.staff.staffName,
            dateBirth: this.staff.dateBirth,
            gender: this.staff.gender,
            address: this.staff.address,
            phone: this.staff.phone,
            personalEmail: this.staff.personalEmail,
            companyEmail: this.staff.companyEmail,
            bankAccount: this.staff.bankAccount,
            bankName: this.staff.bankName,
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.showAlert('Cập nhật thông tin thành công', 'success');
                },
                error: (error: any) => {
                    console.log(error)
                    this.showAlert('Cập nhật thông tin thất bại', 'info');
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
