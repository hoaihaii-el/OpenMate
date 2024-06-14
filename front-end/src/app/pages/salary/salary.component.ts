import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Salary } from 'app/models/salary.model';

@Component({
    selector: 'salary-cmp',
    moduleId: module.id,
    templateUrl: 'salary.component.html',
    styleUrls: ['salary.component.scss']
})

export class SalaryComponent {
    public salary: Salary[] = [];
    public month: number = 6;
    public year: number = 2024;

    constructor(private httpClient: HttpClient) {
        this.getSalary();
    }

    getSalary() {
        const url = `https://localhost:7243/api/Salaries/get-salary?month=${this.month}&year=${this.year}`;
        this.httpClient.get(url)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.salary = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    previousMonth() {
        this.month--;
        this.getSalary();
    }

    nextMonth() {
        if (this.month == 6) return;
        this.month++;
        this.getSalary();
    }
}