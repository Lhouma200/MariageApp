import { TestBed } from '@angular/core/testing';

import { ChargeGuard } from './charge.guard';

describe('ChargeGuard', () => {
  let guard: ChargeGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(ChargeGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
