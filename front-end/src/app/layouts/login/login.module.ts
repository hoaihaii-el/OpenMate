import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@NgModule({
    declarations: [
    ],
    imports: [
        CommonModule,
        FormsModule, // Import FormsModule nếu bạn sử dụng template-driven forms
    ],
    providers: [], // Các services có thể được cung cấp tại đây
})
export class LoginModule { }