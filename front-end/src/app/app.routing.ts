import { Routes } from '@angular/router';

import { LoginComponent } from './layouts/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { UserComponent } from './pages/user/user.component';
import { TableComponent } from './pages/table/table.component';
import { TypographyComponent } from './pages/typography/typography.component';
import { IconsComponent } from './pages/icons/icons.component';
import { NotificationsComponent } from './pages/notifications/notifications.component';
import { OtherUserComponent } from './pages/otheruser/otheruser.component';
import { TimeSheetComponent } from './pages/timesheet/timesheet.component';
import { UserInfosComponent } from './pages/userinfos/userinfos.component';
import { DevicesComponent } from './pages/devices/devices.component';
import { NewPostComponent } from './pages/newpost/newpost.component';
import { RequestsComponent } from './pages/requests/requests.component';
import { MessagesComponent } from './pages/messages/messages.component';
import { RequestDetailComponent } from './pages/requestdetail/requestdetail.component';
import { UserTaskComponent } from './pages/usertask/usertask.component';
import { SalaryComponent } from './pages/salary/salary.component';
import { StructureComponent } from './pages/structure/structure.component';

export const AppRoutes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
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
  { path: 'requests', component: RequestsComponent },
  { path: 'messages', component: MessagesComponent },
  { path: 'requestdetail/:requestID', component: RequestDetailComponent },
  { path: 'usertask', component: UserTaskComponent },
  { path: 'salary', component: SalaryComponent },
  { path: 'structure', component: StructureComponent }
]
