import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ToastrModule } from "ngx-toastr";

import { SidebarModule } from './sidebar/sidebar.module';
import { FooterModule } from './shared/footer/footer.module';
import { NavbarModule } from './shared/navbar/navbar.module';
import { FixedPluginModule } from './shared/fixedplugin/fixedplugin.module';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';

import { AppComponent } from './app.component';
import { AppRoutes } from './app.routing';

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
    MessagesComponent
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
    CKEditorModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
