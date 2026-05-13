import { CommonModule } from '@angular/common';
import { Component, ElementRef, HostListener, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { HrmsServiceService } from '../../hrms-service.service';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { SettingsService } from '../../service/settings.service';
import { RouterModule } from '@angular/router';
import { Modal } from 'bootstrap';
import { Dropdown } from 'bootstrap';
import { ShiftsMappingComponent } from './shifts-mapping/shifts-mapping.component';
import { AddShiftsSettingsComponent } from './add-shifts-settings/add-shifts-settings.component';
import { EmployeeShiftMappingComponent } from './employee-shift-mapping/employee-shift-mapping.component';
import { EntityStateService } from '../../service/entity-state.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-shifts',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, NgxPaginationModule, RouterModule, FormsModule,
    ShiftsMappingComponent,
    AddShiftsSettingsComponent,
    EmployeeShiftMappingComponent
  ],
  templateUrl: './shifts.component.html',
  styleUrl: './shifts.component.scss'
})
export class ShiftsComponent {

  entitySubscription!: Subscription;
  currentEntityId: number | null = null;
  
  constructor(private entityStateService: EntityStateService) {
    this.entitySubscription = this.entityStateService.selectedEntityId$
      .subscribe((newEntityId) => {
        // Ignore initial null
        if (!newEntityId) return;

        // Only reset if entity actually changed
        if (this.currentEntityId && this.currentEntityId !== newEntityId) {
          console.log('Entity changed → resetting filter form');
        }

        this.currentEntityId = newEntityId;
      });
  }
  tabs = [
    { key: 'Shifts_Mapping', label: 'Shifts Mapping', icon: 'feather icon-refresh-ccw' },
    { key: 'Add_Shifts', label: 'Add Shifts', icon: 'feather icon-plus-circle' },
    { key: 'Employee_Shifts_Mapping', label: 'Employee Shifts Mapping', icon: 'feather icon-users' }
  ];


  selectedTab = 'Shifts_Mapping';

  selectTab(tabKey: string) {
    this.selectedTab = tabKey;
  }

}
