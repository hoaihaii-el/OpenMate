import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { RequestDetail } from 'app/models/requestdetail.model';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'requestdetail-cmp',
    moduleId: module.id,
    templateUrl: 'requestdetail.component.html',
    styleUrls: ['requestdetail.component.scss']
})

export class RequestDetailComponent {
    public requestDetail: RequestDetail;
    public requestID: string = '1101';
    public accepters: string[] = [];
    public sanitizerHtml: any;
    public Editor = ClassicEditor;
    public model1 = {
        editorData: '<p>Input here!</p>'
    };
    public model2 = {
        editorData: '<p>Input here!</p>'
    };
    public model3 = {
        editorData: '<p>Input here!</p>'
    };
    public content1: String = '';
    public content2: String = '';
    public content3: String = '';

    constructor(private httpClient: HttpClient, private sanitizer: DomSanitizer, private route: ActivatedRoute, private toastr: ToastrService) {
        this.route.params.subscribe(params => {
            this.requestID = params['requestID'];
        });
        this.getRequestDetail();
    }

    getRequestDetail() {
        const staffID = localStorage.getItem('userID');
        this.httpClient.get(`https://localhost:7243/api/Requests/get-request-detail?requestID=${this.requestID}&staffID=${staffID}`)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.requestDetail = res;
                    this.accepters = this.requestDetail.accepters.split('_');
                    this.sanitizerHtml = this.sanitizer.bypassSecurityTrustHtml(this.requestDetail.rules);
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    createRequest() {
        if (this.content1 === '' && this.requestDetail.answer1Type != 'Editor') {
            this.showAlert('Please input all field!', 'error');
            return;
        }

        if (this.content2 === '' && this.requestDetail.answer2Type != 'Editor') {
            this.showAlert('Please input all field!', 'error');
            return;
        }

        if (this.content3 === '' && this.requestDetail.answer3Type != 'Editor') {
            this.showAlert('Please input all field!', 'error');
            return;
        }

        if (this.model1.editorData === '' && this.requestDetail.answer1Type === 'Editor') {
            this.showAlert('Please input all field!', 'error');
            return;
        }

        if (this.model2.editorData === '' && this.requestDetail.answer2Type === 'Editor') {
            this.showAlert('Please input all field!', 'error');
            return;
        }

        if (this.model2.editorData === '' && this.requestDetail.answer2Type === 'Editor') {
            this.showAlert('Please input all field!', 'error');
            return;
        }

        const staffID = localStorage.getItem('userID');
        const answer1 = this.requestDetail.answer1Type === 'Editor' ? this.model1.editorData : this.content1;
        const answer2 = this.requestDetail.answer2Type === 'Editor' ? this.model2.editorData : this.content2;
        const answer3 = this.requestDetail.answer3Type === 'Editor' ? this.model3.editorData : this.content3;
        const url = `https://localhost:7243/api/Requests/create-request`;
        this.httpClient.post(url, {
            requestID: this.requestID,
            staffID: staffID,
            content1: answer1,
            content2: answer2,
            content3: answer3
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.showAlert('Request created successfully!', 'success');
                },
                error: (error: any) => {
                    console.log(error)
                    this.showAlert(error.error, 'error');
                }
            })
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