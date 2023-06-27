import { TestBed } from '@angular/core/testing';

import { MessagesGuard } from './messages.guard';

describe('MessagesGuard', () => {
  let guard: MessagesGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(MessagesGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
