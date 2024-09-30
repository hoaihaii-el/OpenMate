import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from "ngx-toastr";
import { TimeSheet } from 'app/models/timesheet.model';
import { ChangeTimeReq } from 'app/models/changetimereq.model';

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
    public staffID: String;
    public sheets: TimeSheet[];
    public modalTitle: String = 'Chỉnh sửa';
    public showModal: Boolean = false;
    public h1: number = 1;
    public h2: number = 1;
    public m1: number = 1;
    public m2: number = 1;
    public wrkType: string = '';
    public off: string = '';
    public base64Image: string = '';
    public reason: string = '';
    public filename: string = '';
    public changeTimeReqs: ChangeTimeReq[] = [];
    private key: String = '';

    constructor(private httpClient: HttpClient, private toastr: ToastrService) {
        const now = new Date();
        this.currentMonth = now.getMonth() + 1;
        this.currentYear = now.getFullYear();
        this.staffID = localStorage.getItem('userID');

        this.getData();
        this.updateTable();
        this.getChangeTimeReqs();
    }

    getData(): void {
        const url2 = `https://localhost:7243/api/TimeSheet/get-detail?staffID=${this.staffID}&month=${this.currentMonth}&year=${this.currentYear}`;
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

    getChangeTimeReqs(): void {
        const url = `https://localhost:7243/api/Requests/get-changetime-reqs?staffID=${this.staffID}&month=${this.currentMonth}&year=${this.currentYear}`;
        this.httpClient.get(url)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.changeTimeReqs = res;
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
        this.getChangeTimeReqs();
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
        this.getChangeTimeReqs();
    }

    updateTable(): void {
        const apiUrl = `https://localhost:7243/api/TimeSheet/get-sheet-by-month?staffID=${this.staffID}&month=${this.currentMonth}&year=${this.currentYear}`;
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

        this.wrkType = sheet.workingType;
        this.off = sheet.off;
        this.base64Image = '';
        console.log(sheet);
        this.showModal = true;
    }

    closeModal() {
        this.showModal = false;
    }

    updateCheckIn(type: number) {
        if (type == 1) {
            if (this.h1 == 23) this.h1 = -1;
            this.h1++;
        }
        else if (type == 2) {
            if (this.h1 == 0) this.h1 = 24;
            this.h1--;
        }
        else if (type == 3) {
            if (this.m1 == 59) this.m1 = -1;
            this.m1++;
        }
        else if (type == 4) {
            if (this.m1 == 0) this.m1 = 60;
            this.m1--;
        }
    }

    updateCheckOut(type: number) {
        if (type == 1) {
            if (this.h2 == 23) this.h2 = -1;
            this.h2++;
        }
        else if (type == 2) {
            if (this.h2 == 0) this.h2 = 24;
            this.h2--;
        }
        else if (type == 3) {
            if (this.m2 == 59) this.m2 = -1;
            this.m2++;
        }
        else if (type == 4) {
            if (this.m2 == 0) this.m2 = 60;
            this.m2--;
        }
    }

    setDayOff(value: String, key: String) {
        const apiUrl = `https://localhost:7243/api/TimeSheet/update-data`;
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
        const apiUrl = `https://localhost:7243/api/TimeSheet/update-data`;
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
        // if (this.base64Image == '') {
        //     this.showAlert('Please provide an evidence!', 'info');
        //     return;
        // }

        const apiUrl = `https://localhost:7243/api/Requests/create-req-changetime`;
        const data = {
            staffID: this.staffID,
            date: this.key,
            h1: this.h1,
            m1: this.m1,
            h2: this.h2,
            m2: this.m2,
            wrkType: this.wrkType,
            off: this.off,
            reason: this.reason,
            evidence: this.base64Image
        };
        this.httpClient.post(apiUrl, data)
            .subscribe({
                next: (response: any) => {
                    console.log(response);
                    this.showAlert('A request change time was created!', 'success');
                    this.showModal = false;
                },
                error: (error: any) => {
                    console.log(error);
                    this.showAlert('An error occur!', 'info');
                }
            });

        this.getChangeTimeReqs();
    }

    onFileChange(event: any) {
        try {
            const file = event.target.files[0];
            if (file) {
                this.filename = "Evidence chooosen!"
                this.convertToBase64(file);
            }
        }
        catch (e) {
            console.log(e);
        }
    }

    convertToBase64(file: File) {
        const reader = new FileReader();
        reader.onload = (e: any) => {
            this.base64Image = e.target.result;
            console.log(this.base64Image); // Here you can handle the Base64 image as needed
        };
        reader.readAsDataURL(file);
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