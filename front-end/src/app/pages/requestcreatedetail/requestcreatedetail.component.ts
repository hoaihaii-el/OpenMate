import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestCreateDetail } from 'app/models/createdetail.model';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'requestcreatedetail-cmp',
    moduleId: module.id,
    templateUrl: 'requestcreatedetail.component.html',
    styleUrls: ['requestcreatedetail.component.scss']
})

export class RequestCreateDetailComponent {
    public requestDetail: RequestCreateDetail;
    public createID: string = '1101';
    public isOpenByManager: boolean = false;
    public activities: string[] = [];
    public rules: any;
    public content1: any;
    public content2: any;
    public content3: any;
    public isTakeAction: boolean = false;

    constructor(private httpClient: HttpClient, private sanitizer: DomSanitizer, private route: ActivatedRoute, private toastr: ToastrService, private router: Router) {
        this.route.params.subscribe(params => {
            this.createID = params['createID'];
            this.isOpenByManager = params['isOpenByManager'] === 'true';
        });
        this.getRequestDetail();
    }

    getRequestDetail() {
        this.httpClient.get(`http://localhost:5299/api/Requests/get-req-create-detail?createID=${this.createID}`)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.requestDetail = res;
                    this.activities = this.requestDetail.activities.split('_');
                    const userName = localStorage.getItem('userName');
                    this.isTakeAction = !this.requestDetail.activities.includes(userName + ' has not');
                    this.rules = this.sanitizer.bypassSecurityTrustHtml(this.requestDetail.rules);
                    if (this.requestDetail.answer1Type === 'Editor') {
                        this.content1 = this.sanitizer.bypassSecurityTrustHtml(this.requestDetail.content1);
                    }
                    if (this.requestDetail.answer2Type === 'Editor') {
                        this.content2 = this.sanitizer.bypassSecurityTrustHtml(this.requestDetail.content2);
                    }
                    if (this.requestDetail.answer3Type === 'Editor') {
                        this.content3 = this.sanitizer.bypassSecurityTrustHtml(this.requestDetail.content3);
                    }
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    considerRequest(action: string) {
        const managerID = localStorage.getItem('userID');
        this.httpClient.post(`http://localhost:5299/api/Requests/consider-request`, {
            createID: this.createID,
            managerID: managerID,
            action: action
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.showAlert(`You have ${action.toLowerCase()}ed the request`, 'success');
                    this.router.navigate(['/requests']);
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
            case 'error':
                this.toastr.success(
                    `<span data-notify="icon" class="nc-icon nc-bell-55"></span><span data-notify="message">${content}</span>`,
                    "",
                    {
                        timeOut: 4000,
                        closeButton: true,
                        enableHtml: true,
                        toastClass: "alert alert-danger alert-with-icon",
                        positionClass: "toast-" + 'top' + "-" + 'left'
                    }
                );
                break;
        }
    }
}