import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    declarations: [
    ],
    imports: [
        CommonModule,
        NgbModule,
        FormsModule, // Import FormsModule nếu bạn sử dụng template-driven forms
    ],
    providers: [], // Các services có thể được cung cấp tại đây
})
export class LoginModule { }