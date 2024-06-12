import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ToastrModule } from "ngx-toastr";

import { SidebarModule } from './sidebar/sidebar.module';
import { FooterModule } from './shared/footer/footer.module';
import { NavbarModule } from './shared/navbar/navbar.module';
import { FixedPluginModule } from './shared/fixedplugin/fixedplugin.module';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { AppRoutes } from './app.routing';
import { HttpClientModule } from '@angular/common/http';

import { LoginComponent } from "./layouts/login/login.component";
import { DashboardComponent } from "./pages/dashboard/dashboard.component";
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
import { MessagesComponent } from "./pages/messages/messages.component";
import { RequestDetailComponent } from "./pages/requestdetail/requestdetail.component";
import { UserTaskComponent } from "./pages/usertask/usertask.component";
import { SalaryComponent } from "./pages/salary/salary.component";
import { StructureComponent } from "./pages/structure/structure.component";
import { FormsModule } from "@angular/forms";
import { RequestCreateDetailComponent } from "./pages/requestcreatedetail/requestcreatedetail.component";


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
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
    NewPostComponent,
    RequestsComponent,
    MessagesComponent,
    RequestDetailComponent,
    UserTaskComponent,
    SalaryComponent,
    StructureComponent,
    RequestCreateDetailComponent
  ],
  imports: [
    BrowserAnimationsModule,
    RouterModule.forRoot(AppRoutes, {
      useHash: true
    }),
    SidebarModule,
    NavbarModule,
    ToastrModule.forRoot(),
    FooterModule,
    FixedPluginModule,
    CKEditorModule,
    HttpClientModule,
    NgbModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
