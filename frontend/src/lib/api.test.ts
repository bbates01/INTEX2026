import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { apiFetch, setAuthErrorHandlers } from './api';

function mockResponse(status: number, body: string) {
  return {
    ok: status >= 200 && status < 300,
    status,
    statusText: status === 401 ? 'Unauthorized' : status === 403 ? 'Forbidden' : 'OK',
    text: async () => body,
  };
}

describe('apiFetch — centralized 401/403', () => {
  beforeEach(() => {
    setAuthErrorHandlers({});
  });

  afterEach(() => {
    vi.unstubAllGlobals();
  });

  it('invokes onUnauthorized for 401 on a normal API path', async () => {
    const onUnauthorized = vi.fn();
    setAuthErrorHandlers({ onUnauthorized });
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue(mockResponse(401, 'Unauthorized')),
    );

    await expect(apiFetch('/api/dashboard/admin')).rejects.toThrow();
    expect(onUnauthorized).toHaveBeenCalledTimes(1);
  });

  it('does not invoke onUnauthorized for POST /api/auth/login 401 (wrong password)', async () => {
    const onUnauthorized = vi.fn();
    setAuthErrorHandlers({ onUnauthorized });
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue(mockResponse(401, '')),
    );

    await expect(
      apiFetch('/api/auth/login', {
        method: 'POST',
        body: JSON.stringify({ email: 'a@b.c', password: 'x' }),
      }),
    ).rejects.toThrow();
    expect(onUnauthorized).not.toHaveBeenCalled();
  });

  it('does not invoke onUnauthorized for POST /api/auth/mfa/validate 401', async () => {
    const onUnauthorized = vi.fn();
    setAuthErrorHandlers({ onUnauthorized });
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue(mockResponse(401, 'Invalid MFA code.')),
    );

    await expect(
      apiFetch('/api/auth/mfa/validate', {
        method: 'POST',
        body: JSON.stringify({ code: '000000' }),
      }),
    ).rejects.toThrow();
    expect(onUnauthorized).not.toHaveBeenCalled();
  });

  it('invokes onForbidden with body for 403', async () => {
    const onForbidden = vi.fn();
    setAuthErrorHandlers({ onForbidden });
    vi.stubGlobal(
      'fetch',
      vi.fn().mockResolvedValue(mockResponse(403, 'Forbidden: insufficient role')),
    );

    await expect(apiFetch('/api/dashboard/admin')).rejects.toThrow();
    expect(onForbidden).toHaveBeenCalledWith('Forbidden: insufficient role');
  });
});
