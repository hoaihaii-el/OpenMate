import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'requests-cmp',
    moduleId: module.id,
    templateUrl: 'requests.component.html',
    styleUrls: ['requests.component.scss']
})

export class RequestsComponent implements OnInit {
    public currentAction: string;

    ngOnInit(): void {
        this.currentAction = 'All';
    }

    changeAction(action: string) {
        this.currentAction = action;
    }
}