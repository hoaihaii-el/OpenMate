import { Component } from '@angular/core';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';

@Component({
    selector: 'newpost-cmp',
    moduleId: module.id,
    templateUrl: 'newpost.component.html'
})

export class NewPostComponent {
    public Editor = ClassicEditor;
}