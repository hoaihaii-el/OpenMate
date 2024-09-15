import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TimeSheetComponent } from './time-sheet.component';
import { TimeSheetRoutingModule } from './time-sheet-routing.module';



@NgModule({
  declarations: [
    TimeSheetComponent
  ],
  imports: [
    CommonModule,
    TimeSheetRoutingModule
  ]
})
export class TimeSheetModule { }
