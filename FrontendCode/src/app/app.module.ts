// Angular Import
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {HashLocationStrategy,LocationStrategy} from '@angular/common'

// project import
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminComponent } from './theme/layout/admin/admin.component';
import { GuestComponent } from './theme/layout/guest/guest.component';
import { ConfigurationComponent } from './theme/layout/admin/configuration/configuration.component';
import { NavBarComponent } from './theme/layout/admin/nav-bar/nav-bar.component';
import { NavigationComponent } from './theme/layout/admin/navigation/navigation.component';
import { NavLeftComponent } from './theme/layout/admin/nav-bar/nav-left/nav-left.component';
import { NavRightComponent } from './theme/layout/admin/nav-bar/nav-right/nav-right.component';
import { NavSearchComponent } from './theme/layout/admin/nav-bar/nav-left/nav-search/nav-search.component';
import { ChatMsgComponent } from './theme/layout/admin/nav-bar/nav-right/chat-msg/chat-msg.component';
import { ChatUserListComponent } from './theme/layout/admin/nav-bar/nav-right/chat-user-list/chat-user-list.component';
import { FriendComponent } from './theme/layout/admin/nav-bar/nav-right/chat-user-list/friend/friend.component';
import { NavContentComponent } from './theme/layout/admin/navigation/nav-content/nav-content.component';
import { NavCollapseComponent } from './theme/layout/admin/navigation/nav-content/nav-collapse/nav-collapse.component';
import { NavGroupComponent } from './theme/layout/admin/navigation/nav-content/nav-group/nav-group.component';
import { NavItemComponent } from './theme/layout/admin/navigation/nav-content/nav-item/nav-item.component';
import { MobileBottomNavComponent } from './theme/layout/admin/mobile-bottom-nav/mobile-bottom-nav.component';
import { SharedModule } from './theme/shared/shared.module';
import { ResizableModule } from 'angular-resizable-element';
import { ToastMessageComponent } from './toast-message/toast-message.component';
import { AccessPolicyRoutingModule } from './HRMS/access-policy/access-policy-routing.module';
import {WebcamModule} from 'ngx-webcam';
import { AttendenceManagementModule } from './HRMS/attendence-management/attendence-management.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './shared/auth-interceptor';
import { PerformancePortalModule } from './HRMS/performance-portal/performance-portal.module';
import { SettingsModule } from './HRMS/settings/settings.module';
import { PayrollModule } from './HRMS/payroll/payroll.module';
import { NgxEchartsModule } from 'ngx-echarts';


@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    GuestComponent,
    ConfigurationComponent,
    NavBarComponent,
    NavigationComponent,
    NavLeftComponent,
    NavRightComponent,
    NavSearchComponent,
    ChatMsgComponent,
    ChatUserListComponent,
    FriendComponent,
    NavContentComponent,
    NavItemComponent,
    NavCollapseComponent,
    NavGroupComponent,
    MobileBottomNavComponent
  ],
  imports: [BrowserModule, 
    AppRoutingModule,SharedModule, 
    FormsModule, ReactiveFormsModule, 
    BrowserAnimationsModule,HttpClientModule,ResizableModule,
    ToastMessageComponent,
    AccessPolicyRoutingModule,
    WebcamModule,
    AttendenceManagementModule,
    PerformancePortalModule,
    SettingsModule,
    PayrollModule,
     NgxEchartsModule.forRoot({ echarts: () => import('echarts') })
    ],
  providers: [{provide:LocationStrategy,useClass:HashLocationStrategy},
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
