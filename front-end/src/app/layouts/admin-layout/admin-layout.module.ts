import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { AdminLayoutRoutes } from './admin-layout.routing';

import { DashboardComponent } from '../../pages/dashboard/dashboard.component';
import { UserComponent } from '../../pages/user/user.component';
import { TableComponent } from '../../pages/table/table.component';
import { TypographyComponent } from '../../pages/typography/typography.component';
import { IconsComponent } from '../../pages/icons/icons.component';
import { NotificationsComponent } from '../../pages/notifications/notifications.component';
import { TimeSheetComponent } from '../../pages/timesheet/timesheet.component';
import { OtherUserComponent } from '../../pages/otheruser/otheruser.component';
import { UserInfosComponent } from 'app/pages/userinfos/userinfos.component';
import { DevicesComponent } from 'app/pages/devices/devices.component';
import { NewPostComponent } from 'app/pages/newpost/newpost.component';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    NgbModule
  ],
  declarations: [
    DashboardComponent,
    UserComponent,
    TableComponent,
    TypographyComponent,
    IconsComponent,
    NotificationsComponent,
    TimeSheetComponent,
    OtherUserComponent,
    UserInfosComponent,
    DevicesComponent,
    NewPostComponent
  ]
})

export class AdminLayoutModule { }
