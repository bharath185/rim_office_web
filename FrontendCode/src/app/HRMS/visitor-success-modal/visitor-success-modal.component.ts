import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule } from 'src/app/theme/shared/shared.module';

@Component({
  selector: 'app-visitor-success-modal',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './visitor-success-modal.component.html',
  styleUrl: './visitor-success-modal.component.scss'
})
export class VisitorSuccessModalComponent implements OnInit {
  successMessage: string = 'Process Completed Successfully!';
  constructor(private router: Router) { }

  ngOnInit(): void {
    if (history.state?.message) {
      this.successMessage = history.state.message;
    }
  }
}
