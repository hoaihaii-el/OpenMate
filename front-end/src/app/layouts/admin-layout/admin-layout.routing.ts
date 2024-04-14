import { Routes } from '@angular/router';

import { DashboardComponent } from '../../pages/dashboard/dashboard.component';
import { UserComponent } from '../../pages/user/user.component';
import { TableComponent } from '../../pages/table/table.component';
import { TypographyComponent } from '../../pages/typography/typography.component';
import { IconsComponent } from '../../pages/icons/icons.component';
import { NotificationsComponent } from '../../pages/notifications/notifications.component';
import { TimeSheetComponent } from 'app/pages/timesheet/timesheet.component';
import { OtherUserComponent } from 'app/pages/otheruser/otheruser.component';
import { UserInfosComponent } from 'app/pages/userinfos/userinfos.component';
import { DevicesComponent } from 'app/pages/devices/devices.component';
import { NewPostComponent } from 'app/pages/newpost/newpost.component';
import { RequestsComponent } from 'app/pages/requests/requests.component';

export const AdminLayoutRoutes: Routes = [
    { path: 'dashboard', component: DashboardComponent },
    { path: 'user', component: UserComponent },
    { path: 'table', component: TableComponent },
    { path: 'typography', component: TypographyComponent },
    { path: 'icons', component: IconsComponent },
    { path: 'notifications', component: NotificationsComponent },
    { path: 'timesheet', component: TimeSheetComponent },
    { path: 'otheruser', component: OtherUserComponent },
    { path: 'userinfos', component: UserInfosComponent },
    { path: 'devices', component: DevicesComponent },
    { path: 'newpost', component: NewPostComponent },
    { path: 'requests', component: RequestsComponent }
];
