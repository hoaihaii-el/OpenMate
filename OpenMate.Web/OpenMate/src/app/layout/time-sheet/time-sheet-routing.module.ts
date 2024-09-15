import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TimeSheetComponent } from './time-sheet.component';

const routes: Routes = [
    {
        path: '',
        component: TimeSheetComponent
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TimeSheetRoutingModule { }