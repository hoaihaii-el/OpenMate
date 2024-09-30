import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class IpService {

    private apiUrl = 'https://api.ipify.org?format=json';

    constructor(private http: HttpClient) { }

    getIpAddress(): Observable<any> {
        return this.http.get(this.apiUrl);
    }
}
