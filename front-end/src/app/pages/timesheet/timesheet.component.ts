import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from "ngx-toastr";
import { TimeSheet } from 'app/models/timesheet.model';

@Component({
    selector: 'timesheet-cmp',
    moduleId: module.id,
    templateUrl: 'timesheet.component.html',
    styleUrls: ['timesheet.component.scss']
})

export class TimeSheetComponent {
    public currentMonth: number;
    public currentYear: number;
    public workDays: number = 0;
    public wfh: number = 0;
    public total: String = "__";
    public avg: String = "__";
    public staffID: String = "24001";
    public sheets: TimeSheet[];
    public modalTitle: String = 'Chỉnh sửa';
    public showModal: Boolean = false;
    public h1: number = 1;
    public h2: number = 1;
    public m1: number = 1;
    public m2: number = 1;
    public wrkType: any = 1;
    public off: any = 1;
    private key: String = '';

    constructor(private httpClient: HttpClient, private toastr: ToastrService) {
        const now = new Date();
        this.currentMonth = now.getMonth() + 1;
        this.currentYear = now.getFullYear();

        this.getData();
        this.updateTable();
    }

    getData(): void {
        const url2 = `http://localhost:5299/api/TimeSheet/get-detail?staffID=${this.staffID}&month=${this.currentMonth}&year=${this.currentYear}`;
        this.httpClient.get(url2)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.workDays = res.workDays
                    this.wfh = res.wfh;
                    this.total = `${res.total}hrs`;
                    this.avg = `${res.avg}hrs`;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    lastMonth(): void {
        this.currentMonth--;
        if (this.currentMonth < 1) {
            this.currentMonth = 12;
            this.currentYear--;
        }

        this.getData();
        this.updateTable();
    }

    nextMonth(): void {
        const now = new Date();
        if (this.currentMonth == now.getMonth() + 1 && this.currentYear == now.getFullYear()) {
            console.log('123');
            return;
        }

        this.currentMonth++;

        if (this.currentMonth > 12) {
            this.currentMonth = 1;
            this.currentYear++;
        }

        this.getData();
        this.updateTable();
    }

    updateTable(): void {
        const apiUrl = `http://localhost:5299/api/TimeSheet/get-sheet-by-month?staffID=${this.staffID}&month=${this.currentMonth}&year=${this.currentYear}`;
        this.httpClient.get<TimeSheet[]>(apiUrl)
            .subscribe({
                next: (res: TimeSheet[]) => {
                    console.log(res);
                    this.sheets = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    openModal(sheet: TimeSheet) {
        this.modalTitle = sheet.date;
        this.key = sheet.key;
        const timeInStr = sheet.checkIn.split(":");
        this.h1 = parseInt(timeInStr[0], 10);
        this.m1 = parseInt(timeInStr[1], 10);
        const timeOutStr = sheet.checkOut.split(":");
        this.h2 = parseInt(timeOutStr[0], 10);
        this.m2 = parseInt(timeOutStr[1], 10);

        if (isNaN(this.h1)) this.h1 = 1;
        if (isNaN(this.h2)) this.h2 = 1;
        if (isNaN(this.m1)) this.m1 = 1;
        if (isNaN(this.m2)) this.m2 = 1;

        this.showModal = true;
    }

    closeModal() {
        this.showModal = false;
    }

    updateCheckIn(type: number) {
        if (type == 1) {
            if (this.h1 == 23) return;
            this.h1++;
        }
        else if (type == 2) {
            if (this.h1 == 0) return;
            this.h1--;
        }
        else if (type == 3) {
            if (this.m1 == 59) return;
            this.m1++;
        }
        else if (type == 4) {
            if (this.m1 == 0) return;
            this.m1--;
        }
    }

    updateCheckOut(type: number) {
        if (type == 1) {
            if (this.h2 == 23) return;
            this.h2++;
        }
        else if (type == 2) {
            if (this.h2 == 0) return;
            this.h2--;
        }
        else if (type == 3) {
            if (this.m2 == 59) return;
            this.m2++;
        }
        else if (type == 4) {
            if (this.m2 == 0) return;
            this.m2--;
        }
    }

    setDayOff(value: String, key: String) {
        const apiUrl = `http://localhost:5299/api/TimeSheet/update-data`;
        const data = {
            staffID: this.staffID,
            date: key,
            type: 'off',
            h1: this.h1,
            m1: this.m1,
            h2: this.h2,
            m2: this.m2,
            wrkType: 'value',
            off: value
        };
        this.httpClient.put(apiUrl, data)
            .subscribe({
                next: (response: any) => {
                    console.log(response);
                    this.getData();
                    this.updateTable();
                    this.showAlert('A request OFF was created! Please provide more information.', 'success');
                },
                error: (error: any) => {
                    console.log(error);
                    this.showAlert('An error occur!', 'info');
                }
            });
    }

    setWFH(value: String, key: String) {
        const apiUrl = `http://localhost:5299/api/TimeSheet/update-data`;
        const data = {
            staffID: this.staffID,
            date: key,
            type: 'working-type',
            h1: this.h1,
            m1: this.m1,
            h2: this.h2,
            m2: this.m2,
            wrkType: value,
            off: 'off'
        };
        this.httpClient.put(apiUrl, data)
            .subscribe({
                next: (response: any) => {
                    console.log(response);
                    this.getData();
                    this.updateTable();
                    this.showAlert('A request WFH was created! Please provide more information.', 'success');
                },
                error: (error: any) => {
                    console.log(error);
                    this.showAlert('An error occur!', 'info');
                }
            });
    }

    changeTime() {
        const apiUrl = `http://localhost:5299/api/TimeSheet/update-data`;
        const data = {
            staffID: this.staffID,
            date: this.key,
            type: 'all',
            h1: this.h1,
            m1: this.m1,
            h2: this.h2,
            m2: this.m2,
            wrkType: this.wrkType == 1 ? 'OFFICE' : 'WFH',
            off: this.off == 1 ? 'ALL' : this.off == 2 ? 'AM-OFF' : 'PM-OFF'
        };
        this.httpClient.put(apiUrl, data)
            .subscribe({
                next: (response: any) => {
                    console.log(response);
                    this.getData();
                    this.updateTable();
                    this.showAlert('A request CHANGE-TIME was created! Please provide more information.', 'success');
                },
                error: (error: any) => {
                    console.log(error);
                    this.showAlert('An error occur!', 'info');
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
                        toastClass: "alert alert-info alert-with-icon",
                        positionClass: "toast-" + 'top' + "-" + 'left'
                    }
                );
                break;
        }
    }
}