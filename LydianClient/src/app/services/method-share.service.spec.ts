import { TestBed } from '@angular/core/testing';

import { MethodShareService } from './method-share.service';

describe('MethodShareService', () => {
  let service: MethodShareService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MethodShareService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
