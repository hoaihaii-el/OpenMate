import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
    providedIn: 'root'
})
export class ChatService {
    private hubConnection: signalR.HubConnection;

    constructor() {
        this.hubConnection = new signalR.HubConnectionBuilder()
            .withUrl('https://localhost:7243/chathub')
            .build();
    }

    public startConnection(): void {
        this.hubConnection
            .start()
            .then(() => console.log('SignalR connection started'))
            .catch(err => console.log('Error while starting SignalR connection: ' + err));
    }

    public addListener(callback: (message: string) => void): void {
        this.hubConnection.on('ReceiveMessage', callback);
    }
}
