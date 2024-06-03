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
import { LoginGuard } from './guards/login.guard';
import { AuthGuard } from './guards/auth.guard';

export const AppRoutes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [LoginGuard] },
  { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
  { path: 'user', component: UserComponent, canActivate: [AuthGuard] },
  { path: 'table', component: TableComponent, canActivate: [AuthGuard] },
  { path: 'typography', component: TypographyComponent, canActivate: [AuthGuard] },
  { path: 'icons', component: IconsComponent, canActivate: [AuthGuard] },
  { path: 'notifications', component: NotificationsComponent, canActivate: [AuthGuard] },
  { path: 'timesheet', component: TimeSheetComponent, canActivate: [AuthGuard] },
  { path: 'otheruser', component: OtherUserComponent, canActivate: [AuthGuard] },
  { path: 'userinfos', component: UserInfosComponent, canActivate: [AuthGuard] },
  { path: 'devices', component: DevicesComponent, canActivate: [AuthGuard] },
  { path: 'newpost', component: NewPostComponent, canActivate: [AuthGuard] },
  { path: 'requests', component: RequestsComponent, canActivate: [AuthGuard] },
  { path: 'messages', component: MessagesComponent, canActivate: [AuthGuard] },
  { path: 'requestdetail/:requestID', component: RequestDetailComponent, canActivate: [AuthGuard] },
  { path: 'usertask', component: UserTaskComponent, canActivate: [AuthGuard] },
  { path: 'salary', component: SalaryComponent, canActivate: [AuthGuard] },
  { path: 'structure', component: StructureComponent, canActivate: [AuthGuard] }
]
