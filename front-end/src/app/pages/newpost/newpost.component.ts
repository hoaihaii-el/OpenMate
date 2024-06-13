import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { ToastrService } from "ngx-toastr";

@Component({
    selector: 'newpost-cmp',
    moduleId: module.id,
    templateUrl: 'newpost.component.html'
})

export class NewPostComponent {
    public Editor = ClassicEditor;
    public title: String = 'New Post';
    public level: number = 1;
    public model = {
        editorData: '<p>New post!</p>'
    };

    constructor(private httpClient: HttpClient, private toastr: ToastrService) { }

    uplodaNewpost(): void {
        var levelStr = this.level == 1 ? "Must Read" : this.level == 2 ? "Important" : "Normal";
        console.log(this.model.editorData);
        const apiUrl = `https://localhost:7243/api/Notification/new-noti`;
        const data = {
            notiName: this.title,
            content: this.model.editorData,
            level: levelStr
        };
        this.httpClient.post(apiUrl, data)
            .subscribe({
                next: (response: any) => {
                    this.showAlert("New post has been created successfully", "success");
                    this.model.editorData = '<p>New post!</p>';
                    this.title = 'New Post';
                },
                error: (error: any) => {
                    console.log(error);
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
        }
    }
}

