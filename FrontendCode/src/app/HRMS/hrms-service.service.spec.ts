import { TestBed } from '@angular/core/testing';

import { HrmsServiceService } from './hrms-service.service';

describe('HrmsServiceService', () => {
  let service: HrmsServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HrmsServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
