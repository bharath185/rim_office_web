import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { SharedModule } from 'src/app/theme/shared/shared.module';
import { ToastMessageComponent } from 'src/app/toast-message/toast-message.component';
import { AccessPolicyStoreService } from '../../service/accessPolicayApi.service';

@Component({
  selector: 'app-insurance-details',
  standalone: true,
  imports: [SharedModule, CommonModule, ReactiveFormsModule, ToastMessageComponent,],
  templateUrl: './insurance-details.component.html',
  styleUrl: './insurance-details.component.scss'
})
export class InsuranceDetailsComponent implements OnInit {
  @ViewChild(ToastMessageComponent) toastMessageComponent!: ToastMessageComponent;

  insuranceForm: any = FormGroup;
  isSpinner: boolean = false;
  isFormSubmitted: boolean = false;
  minStartDate!: string;
  maxStartDate!: string;
  employeeDetails;
  accessPolicy:any;
  controlAccessPage:any;


  constructor(private readonly fb: FormBuilder,
    private accessPolicyStoreService: AccessPolicyStoreService) {
    const storedEmployeeDetails = sessionStorage.getItem('employeeDetails');
    this.employeeDetails = storedEmployeeDetails ? JSON.parse(storedEmployeeDetails) : null;

    this.accessPolicyStoreService.getAccessPolicy().subscribe(policy => {
      if (!policy || policy.length === 0) return;
      this.accessPolicy = policy;
      this.controlAccessPage = this.accessPolicy.find(
        (item: any) => item.PageName === 'Financial Details'
      );
    });
  }

  ngOnInit(): void {
    this.insuranceForm = this.fb.group({
      insuranceProvider: ['', [Validators.required]],
      policyType: ['', [Validators.required]],
      policyNumber: ['', [Validators.required]],
      currency: ['INR'],
      coverageAmount: ['', [Validators.required]],
      date_from: ['', [Validators.required]],
      date_to: ['', [Validators.required]],
    });
    const currentDate = new Date();
    this.minStartDate = this.formatDate(new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, currentDate.getDate()));
    const nextYear = new Date(currentDate.getFullYear() + 1, currentDate.getMonth(), currentDate.getDate());
    this.maxStartDate = this.formatDate(nextYear);
  }

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const dateFrom = group.get('date_from')?.value;
    const dateTo = group.get('date_to')?.value;
    if (dateFrom && dateTo && new Date(dateTo) < new Date(dateFrom)) {
      return { dateRange: true };
    }
    return null;
  }
  onFromDate(): void {
    const fromDate = this.insuranceForm.get('date_from')?.value;
    if (fromDate) {
      this.minStartDate = fromDate;
    }
  }
  onToDate(): void {
    const toDate = this.insuranceForm.get('date_to')?.value;
    if (toDate) {
      this.maxStartDate = toDate;
    }
  }
  isFromDateInvalid(): boolean {
    const fromDate = this.insuranceForm.get('date_from');
    return fromDate?.invalid && (fromDate?.touched || this.isFormSubmitted);
  }
  isToDateInvalid(): boolean {
    const toDate = this.insuranceForm.get('date_to');
    return toDate?.invalid && (toDate?.touched || this.isFormSubmitted);
  }
  get dateRangeError(): boolean {
    return this.insuranceForm.hasError('dateRange');
  }

  submitForm() {
    if (this.insuranceForm.valid) {

    } else {
      this.isFormSubmitted = true;
    }
  }

  resetData() {
    this.insuranceForm.patchValue({ currency: 'INR' });
    this.insuranceForm.reset();
    this.isFormSubmitted = false;
    this.maxStartDate = '';
    this.minStartDate = '';
  }

  handleAlphaChar(event: any) {
    if (
      (event.charCode > 32 && event.charCode < 48) ||
      (event.charCode > 57 && event.charCode < 127)
    ) {
      event.preventDefault();
    }
  }
  triggerToast(header: any, body: any, mess: any) {
    this.toastMessageComponent.showToast(header, body, mess);
  }


  currencies = [
    { code: 'AED', name: 'United Arab Emirates Dirham', symbol: 'د.إ' },
    { code: 'AFN', name: 'Afghan Afghani', symbol: '؋' },
    { code: 'ALL', name: 'Albanian Lek', symbol: 'L' },
    { code: 'AMD', name: 'Armenian Dram', symbol: '֏' },
    { code: 'ANG', name: 'Netherlands Antillean Guilder', symbol: 'ƒ' },
    { code: 'AOA', name: 'Angolan Kwanza', symbol: 'Kz' },
    { code: 'ARS', name: 'Argentine Peso', symbol: '$' },
    { code: 'AUD', name: 'Australian Dollar', symbol: 'A$' },
    { code: 'AWG', name: 'Aruban Florin', symbol: 'ƒ' },
    { code: 'AZN', name: 'Azerbaijani Manat', symbol: '₼' },
    { code: 'BAM', name: 'Bosnia-Herzegovina Convertible Mark', symbol: 'KM' },
    { code: 'BBD', name: 'Barbadian Dollar', symbol: 'Bds$' },
    { code: 'BDT', name: 'Bangladeshi Taka', symbol: '৳' },
    { code: 'BGN', name: 'Bulgarian Lev', symbol: 'лв' },
    { code: 'BHD', name: 'Bahraini Dinar', symbol: '.د.ب' },
    { code: 'BIF', name: 'Burundian Franc', symbol: 'FBu' },
    { code: 'BMD', name: 'Bermudian Dollar', symbol: '$' },
    { code: 'BND', name: 'Brunei Dollar', symbol: 'B$' },
    { code: 'BOB', name: 'Bolivian Boliviano', symbol: 'Bs.' },
    { code: 'BRL', name: 'Brazilian Real', symbol: 'R$' },
    { code: 'BSD', name: 'Bahamian Dollar', symbol: 'B$' },
    { code: 'BTC', name: 'Bitcoin', symbol: '₿' },
    { code: 'BTN', name: 'Bhutanese Ngultrum', symbol: 'Nu.' },
    { code: 'BWP', name: 'Botswana Pula', symbol: 'P' },
    { code: 'BYN', name: 'Belarusian Ruble', symbol: 'Br' },
    { code: 'BZD', name: 'Belize Dollar', symbol: 'BZ$' },
    { code: 'CAD', name: 'Canadian Dollar', symbol: 'C$' },
    { code: 'CDF', name: 'Congolese Franc', symbol: 'FC' },
    { code: 'CHF', name: 'Swiss Franc', symbol: 'Fr' },
    { code: 'CLP', name: 'Chilean Peso', symbol: '$' },
    { code: 'CNY', name: 'Chinese Yuan', symbol: '¥' },
    { code: 'COP', name: 'Colombian Peso', symbol: '$' },
    { code: 'CRC', name: 'Costa Rican Colón', symbol: '₡' },
    { code: 'CUC', name: 'Cuban Convertible Peso', symbol: '$' },
    { code: 'CUP', name: 'Cuban Peso', symbol: '$' },
    { code: 'CVE', name: 'Cape Verdean Escudo', symbol: 'Esc' },
    { code: 'CZK', name: 'Czech Koruna', symbol: 'Kč' },
    { code: 'DJF', name: 'Djiboutian Franc', symbol: 'Fdj' },
    { code: 'DKK', name: 'Danish Krone', symbol: 'kr' },
    { code: 'DOP', name: 'Dominican Peso', symbol: 'RD$' },
    { code: 'DZD', name: 'Algerian Dinar', symbol: 'دج' },
    { code: 'EGP', name: 'Egyptian Pound', symbol: '£' },
    { code: 'ERN', name: 'Eritrean Nakfa', symbol: 'Nfk' },
    { code: 'ETB', name: 'Ethiopian Birr', symbol: 'Br' },
    { code: 'EUR', name: 'Euro', symbol: '€' },
    { code: 'FJD', name: 'Fijian Dollar', symbol: 'FJ$' },
    { code: 'FKP', name: 'Falkland Islands Pound', symbol: '£' },
    { code: 'GBP', name: 'British Pound Sterling', symbol: '£' },
    { code: 'GEL', name: 'Georgian Lari', symbol: '₾' },
    { code: 'GHS', name: 'Ghanaian Cedi', symbol: '₵' },
    { code: 'GIP', name: 'Gibraltar Pound', symbol: '£' },
    { code: 'GMD', name: 'Gambian Dalasi', symbol: 'D' },
    { code: 'GNF', name: 'Guinean Franc', symbol: 'FG' },
    { code: 'GTQ', name: 'Guatemalan Quetzal', symbol: 'Q' },
    { code: 'GYD', name: 'Guyanese Dollar', symbol: 'GY$' },
    { code: 'HKD', name: 'Hong Kong Dollar', symbol: 'HK$' },
    { code: 'HNL', name: 'Honduran Lempira', symbol: 'L' },
    { code: 'HRK', name: 'Croatian Kuna', symbol: 'kn' },
    { code: 'HTG', name: 'Haitian Gourde', symbol: 'G' },
    { code: 'HUF', name: 'Hungarian Forint', symbol: 'Ft' },
    { code: 'IDR', name: 'Indonesian Rupiah', symbol: 'Rp' },
    { code: 'ILS', name: 'Israeli New Shekel', symbol: '₪' },
    { code: 'INR', name: 'Indian Rupee', symbol: '₹' },
    { code: 'IQD', name: 'Iraqi Dinar', symbol: 'ع.د' },
    { code: 'IRR', name: 'Iranian Rial', symbol: '﷼' },
    { code: 'ISK', name: 'Icelandic Króna', symbol: 'kr' },
    { code: 'JMD', name: 'Jamaican Dollar', symbol: 'J$' },
    { code: 'JOD', name: 'Jordanian Dinar', symbol: 'JD' },
    { code: 'JPY', name: 'Japanese Yen', symbol: '¥' },
    { code: 'KES', name: 'Kenyan Shilling', symbol: 'KSh' },
    { code: 'KGS', name: 'Kyrgyzstani Som', symbol: 'сом' },
    { code: 'KHR', name: 'Cambodian Riel', symbol: '៛' },
    { code: 'KMF', name: 'Comorian Franc', symbol: 'CF' },
    { code: 'KPW', name: 'North Korean Won', symbol: '₩' },
    { code: 'KRW', name: 'South Korean Won', symbol: '₩' },
    { code: 'KWD', name: 'Kuwaiti Dinar', symbol: 'د.ك' },
    { code: 'KYD', name: 'Cayman Islands Dollar', symbol: 'KY$' },
    { code: 'KZT', name: 'Kazakhstani Tenge', symbol: '₸' },
    { code: 'LAK', name: 'Lao Kip', symbol: '₭' },
    { code: 'LBP', name: 'Lebanese Pound', symbol: 'ل.ل' },
    { code: 'LKR', name: 'Sri Lankan Rupee', symbol: 'Rs' },
    { code: 'LRD', name: 'Liberian Dollar', symbol: 'L$' },
    { code: 'LSL', name: 'Lesotho Loti', symbol: 'M' },
    { code: 'LYD', name: 'Libyan Dinar', symbol: 'ل.د' },
    { code: 'MAD', name: 'Moroccan Dirham', symbol: 'د.م.' },
    { code: 'MDL', name: 'Moldovan Leu', symbol: 'L' },
    { code: 'MGA', name: 'Malagasy Ariary', symbol: 'Ar' },
    { code: 'MKD', name: 'Macedonian Denar', symbol: 'ден' },
    { code: 'MMK', name: 'Myanma Kyat', symbol: 'K' },
    { code: 'MNT', name: 'Mongolian Tögrög', symbol: '₮' },
    { code: 'MOP', name: 'Macanese Pataca', symbol: 'P' },
    { code: 'MRU', name: 'Mauritanian Ouguiya', symbol: 'UM' },
    { code: 'MUR', name: 'Mauritian Rupee', symbol: 'Rs' },
    { code: 'MVR', name: 'Maldivian Rufiyaa', symbol: 'Rf' },
    { code: 'MWK', name: 'Malawian Kwacha', symbol: 'MK' },
    { code: 'MXN', name: 'Mexican Peso', symbol: '$' },
    { code: 'MYR', name: 'Malaysian Ringgit', symbol: 'RM' },
    { code: 'MZN', name: 'Mozambican Metical', symbol: 'MT' },
    { code: 'NAD', name: 'Namibian Dollar', symbol: 'N$' },
    { code: 'NGN', name: 'Nigerian Naira', symbol: '₦' },
    { code: 'NIO', name: 'Nicaraguan Córdoba', symbol: 'C$' },
    { code: 'NOK', name: 'Norwegian Krone', symbol: 'kr' },
    { code: 'NPR', name: 'Nepalese Rupee', symbol: 'Rs' },
    { code: 'NZD', name: 'New Zealand Dollar', symbol: 'NZ$' },
    { code: 'OMR', name: 'Omani Rial', symbol: 'ر.ع.' },
    { code: 'PAB', name: 'Panamanian Balboa', symbol: 'B./' },
    { code: 'PEN', name: 'Peruvian Sol', symbol: 'S/.' },
    { code: 'PGK', name: 'Papua New Guinean Kina', symbol: 'K' },
    { code: 'PHP', name: 'Philippine Peso', symbol: '₱' },
    { code: 'PKR', name: 'Pakistani Rupee', symbol: '₨' },
    { code: 'PLN', name: 'Polish Złoty', symbol: 'zł' },
    { code: 'PYG', name: 'Paraguayan Guaraní', symbol: '₲' },
    { code: 'QAR', name: 'Qatari Riyal', symbol: 'ر.ق' },
    { code: 'RON', name: 'Romanian Leu', symbol: 'L' },
    { code: 'RSD', name: 'Serbian Dinar', symbol: 'дин.' },
    { code: 'RUB', name: 'Russian Ruble', symbol: '₽' },
    { code: 'RWF', name: 'Rwandan Franc', symbol: 'FRw' },
    { code: 'SAR', name: 'Saudi Riyal', symbol: 'ر.س' },
    { code: 'SBD', name: 'Solomon Islands Dollar', symbol: 'SI$' },
    { code: 'SCR', name: 'Seychellois Rupee', symbol: 'SR' },
    { code: 'SDG', name: 'Sudanese Pound', symbol: '£' },
    { code: 'SEK', name: 'Swedish Krona', symbol: 'kr' },
    { code: 'SGD', name: 'Singapore Dollar', symbol: 'S$' },
    { code: 'SHP', name: 'Saint Helena Pound', symbol: '£' },
    { code: 'SLL', name: 'Sierra Leonean Leone', symbol: 'Le' },
    { code: 'SOS', name: 'Somali Shilling', symbol: 'S' },
    { code: 'SRD', name: 'Surinamese Dollar', symbol: '$' },
    { code: 'STN', name: 'São Tomé and Príncipe Dobra', symbol: 'Db' },
    { code: 'SYP', name: 'Syrian Pound', symbol: '£' },
    { code: 'SZL', name: 'Swazi Lilangeni', symbol: 'E' },
    { code: 'THB', name: 'Thai Baht', symbol: '฿' },
    { code: 'TJS', name: 'Tajikistani Somoni', symbol: 'ЅМ' },
    { code: 'TMT', name: 'Turkmenistani Manat', symbol: 'm' },
    { code: 'TND', name: 'Tunisian Dinar', symbol: 'د.ت' },
    { code: 'TOP', name: 'Tongan Paʻanga', symbol: 'T$' },
    { code: 'TRY', name: 'Turkish Lira', symbol: '₺' },
    { code: 'TTD', name: 'Trinidad and Tobago Dollar', symbol: 'TT$' },
    { code: 'TWD', name: 'New Taiwan Dollar', symbol: 'NT$' },
    { code: 'TZS', name: 'Tanzanian Shilling', symbol: 'TZS' },
    { code: 'UAH', name: 'Ukrainian Hryvnia', symbol: '₴' },
    { code: 'UGX', name: 'Ugandan Shilling', symbol: 'USh' },
    { code: 'USD', name: 'United States Dollar', symbol: '$' },
    { code: 'UYU', name: 'Uruguayan Peso', symbol: '$U' },
    { code: 'UZS', name: 'Uzbekistani Som', symbol: "so'm" },
    { code: 'VEF', name: 'Venezuelan Bolívar', symbol: 'Bs' },
    { code: 'VND', name: 'Vietnamese Dong', symbol: '₫' },
    { code: 'VUV', name: 'Vanuatu Vatu', symbol: 'VT' },
    { code: 'WST', name: 'Samoan Tala', symbol: 'WS$' },
    { code: 'XAF', name: 'Central African CFA Franc', symbol: 'FCFA' },
    { code: 'XCD', name: 'East Caribbean Dollar', symbol: 'EC$' },
    { code: 'XOF', name: 'West African CFA Franc', symbol: 'CFA' },
    { code: 'XPF', name: 'CFP Franc', symbol: '₣' },
    { code: 'YER', name: 'Yemeni Rial', symbol: '﷼' },
    { code: 'ZAR', name: 'South African Rand', symbol: 'R' },
    { code: 'ZMW', name: 'Zambian Kwacha', symbol: 'ZK' },
    { code: 'ZWL', name: 'Zimbabwean Dollar', symbol: 'Z$' }
  ];


}
