import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ChatService } from 'app/hubs/chat.service';
import { Message } from 'app/models/message.model';
import { Room } from 'app/models/room.model';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'messages-cmp',
    moduleId: module.id,
    templateUrl: 'messages.component.html',
    styleUrls: ['messages.component.scss']
})

export class MessagesComponent implements OnInit {
    public isShowAddRoom: boolean = false;
    public users: string;
    public staffID: string;
    public rooms: Room[] = [];
    public messages: Message[] = [];
    public content: string = '';
    public currentRoom: Room = { roomID: '', roomName: '' };

    constructor(private httpClient: HttpClient, private toastr: ToastrService, private chatService: ChatService) {
        this.staffID = localStorage.getItem('userID');
        this.getRooms();
    }

    ngOnInit(): void {
        this.chatService.startConnection();
        this.chatService.addListener((message: string) => {
            console.log(message);
            this.getRooms();
            this.getMessages();
        });
    }

    public addNewRoom() {
        this.isShowAddRoom = !this.isShowAddRoom;
    }

    changeRoom(room: Room) {
        this.currentRoom = room;
        this.getMessages();
    }

    createNewRoom() {
        const url = 'https://localhost:7243/api/Chats/add-new-room?users=' + this.users + "," + this.staffID;
        this.httpClient.post(url, null)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.showAlert('Create room successfully', 'success');
                    this.getRooms();
                },
                error: (error: any) => {
                    console.log(error)
                    this.showAlert(error.error, 'info');
                }
            });
    }

    getRooms() {
        const url = 'https://localhost:7243/api/Chats/get-all-rooms?staffID=' + this.staffID;
        this.httpClient.get(url)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.rooms = res;
                    if (this.currentRoom.roomID === '') {
                        this.currentRoom = this.rooms[0];
                        this.getMessages();
                    }
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    getMessages() {
        const url = 'https://localhost:7243/api/Chats/get-messages?roomID=' + this.currentRoom.roomID;
        this.httpClient.get(url)
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.messages = res;
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
    }

    sendMessage() {
        console.log(this.content);
        if (this.content === '') return;

        const url = 'https://localhost:7243/api/Chats/send-message';
        this.httpClient.post(url, {
            roomID: this.currentRoom.roomID,
            content: this.content,
            messageType: 'Text',
            resourceURL: '',
            senderID: this.staffID,
        })
            .subscribe({
                next: (res: any) => {
                    console.log(res);
                    this.getMessages();
                },
                error: (error: any) => {
                    console.log(error)
                }
            });
        this.content = '';
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