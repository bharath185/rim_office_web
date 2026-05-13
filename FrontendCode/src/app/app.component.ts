// Angular Import
import { Component, HostListener, OnInit, TemplateRef } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { ApiService } from './demo/authentication/sign-in/api.service';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ToastService } from './toast-message/toast-service';

@Component({

  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  // constructor
  constructor(private router: Router) {}
  isTemplate(toast: { textOrTpl: any; }) { return toast.textOrTpl instanceof TemplateRef; }

  // life cycle event
  ngOnInit() {

    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });

  }

  // this is for hide inspect things
  // @HostListener('contextmenu', ['$event'])
  // onRightClick(event: { preventDefault: () => void; }) {
  //   event.preventDefault();
  // }
  // @HostListener('document:keydown', ['$event'])
  // onKeydown(event: KeyboardEvent) {
  //   if (
  //     (event.key === 'F12') ||
  //     (event.ctrlKey && event.shiftKey && event.key === 'I') ||
  //     (event.ctrlKey && event.shiftKey && event.key === 'J') ||
  //     (event.ctrlKey && event.key === 'U')
  //   ) {
  //     event.preventDefault();
  //   }
  // }
  // this is for hide inspect things

}




