import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-timepicker',
  standalone: true,
  imports: [CommonModule, FormsModule],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: TimepickerComponent,
      multi: true
    }
  ],
  templateUrl: './timepicker.component.html',
  styleUrls: ['./timepicker.component.scss']
})
export class TimepickerComponent {
  @Output() close = new EventEmitter();
  @Output() onChange = new EventEmitter<Date>();  // Emit the selected time to the parent

  auto = true;
  hhmm = 'hh';
  ampm = 'am';
  dial: any = [];
  hour = '12';
  minute = '00';

  private date = new Date();
  private onTouched = () => { };

  constructor() {
    const j = 84;
    for (let min = 1; min <= 12; min++) {
      const hh = String(min);
      const mm = String('00' + ((min * 5) % 60)).slice(-2);
      const x = 1 + Math.sin(Math.PI * 2 * (min / 12));
      const y = 1 - Math.cos(Math.PI * 2 * (min / 12));
      this.dial.push({ top: j * y + 'px', left: j * x + 'px', hh, mm });
    }
  }

  public writeValue(v: Date) {
    this.date = v || new Date();
    const hh = this.date.getHours();
    const mm = this.date.getMinutes();
    this.ampm = hh < 12 ? 'am' : 'pm';
    this.hour = String(hh % 12 || 12);
    this.minute = String('00' + (mm - (mm % 5))).slice(-2);
  }

  public registerOnChange = (fn: any) => { };

  public registerOnTouched = (fn: any) => { };

  timeChange($event: string) {
    if (this.hhmm === 'hh') {
      this.hour = $event;
      if (this.auto) {
        this.hhmm = 'mm';
      }
    } else {
      this.minute = $event;
    }
  }

  rotateHand() {
    const deg = this.hhmm === 'hh' ? +this.hour * 5 : +this.minute;
    return `rotate(${deg * 6}deg)`;
  }

  cancel = () => this.close.emit();  // Emit the close event

  ok() {
    let hh = +this.hour + (this.ampm === 'pm' ? 12 : 0);
    if ((this.ampm === 'am' && hh === 12) || hh === 24) {
      hh -= 12;
    }
    this.date.setHours(hh);
    this.date.setMinutes(+this.minute);

    console.log('Emitting Time:', this.date);  // Log to verify the Date object

    this.onChange.emit(this.date);  // Emit the selected Date object to the parent
    this.close.emit();  // Close the time picker
  }
}
