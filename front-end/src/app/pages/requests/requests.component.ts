import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RequestCreate } from 'app/models/requestcreate.model';
import { RequestType } from 'app/models/requesttype.model';

@Component({
    selector: 'requests-cmp',
    moduleId: module.id,
    templateUrl: 'requests.component.html',
    styleUrls: ['requests.component.scss']
})

export class RequestsComponent {
    public currentAction: string;
    public requestsType: RequestType[] = [];
    public yourRequests: RequestCreate[] = [];
    public needToAcceptRequests: RequestCreate[] = [];
    public staffID: string;

    constructor(private httpClient: HttpClient) {
        this.currentAction = 'All';
        this.staffID = localStorage.getItem('userID');
        this.getRequestsType();
        this.getYourRequests();
        this.getNeedToAcceptRequests();
    }

    changeAction(action: string) {
        this.currentAction = action;
    }

    getRequestsType() {
        this.httpClient.get('https://localhost:7243/api/Requests/get-all')
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.requestsType = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    getYourRequests() {
        this.httpClient.get(`https://localhost:7243/api/Requests/get-your-request?staffID=${this.staffID}`)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.yourRequests = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    getNeedToAcceptRequests() {
        this.httpClient.get(`https://localhost:7243/api/Requests/get-need-to-accept-request?managerID=${this.staffID}`)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.needToAcceptRequests = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }
}