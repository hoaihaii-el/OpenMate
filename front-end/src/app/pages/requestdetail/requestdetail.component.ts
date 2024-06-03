import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { RequestDetail } from 'app/models/requestdetail.model';

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

    constructor(private httpClient: HttpClient, private sanitizer: DomSanitizer, private route: ActivatedRoute) {
        this.route.params.subscribe(params => {
            this.requestID = params['requestID'];
        });
        this.getRequestDetail();
    }

    getRequestDetail() {
        const staffID = localStorage.getItem('userID');
        this.httpClient.get(`http://localhost:5299/api/Requests/get-request-detail?requestID=${this.requestID}&staffID=${staffID}`)
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
}