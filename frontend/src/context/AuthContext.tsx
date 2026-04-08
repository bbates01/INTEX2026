import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from 'react';
import type { ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiFetch, setAuthErrorHandlers } from '../lib/api';
import type { AuthUser } from '../types/auth';

type AuthContextValue = {
  user: AuthUser | null;
  loading: boolean;
  mfaRequired: boolean;
  login: (email: string, password: string) => Promise<void>;
  validateMfa: (code: string) => Promise<void>;
  logout: () => Promise<void>;
  refreshUser: () => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const navigate = useNavigate();
  const [user, setUser] = useState<AuthUser | null>(null);
  const [loading, setLoading] = useState(true);
  const [mfaRequired, setMfaRequired] = useState(false);

  function toAuthUser(raw: unknown): AuthUser | null {
    if (raw == null || typeof raw !== 'object') return null;
    const r = raw as Record<string, unknown>;
    if (r.isAuthenticated === false) return null;
    const id = (r.id ?? r.Id) as string | undefined;
    const email = (r.email ?? r.Email) as string | undefined;
    if (!id && !email) return null;

    const sh = r.safehouseId ?? r.SafehouseId;
    return {
      id: id ?? '',
      email: email ?? '',
      displayName: String(r.displayName ?? r.DisplayName ?? ''),
      roles: (r.roles ?? r.Roles ?? []) as AuthUser['roles'],
      privacyPolicyAccepted: Boolean(r.privacyPolicyAccepted ?? r.PrivacyPolicyAccepted),
      cookieConsentAccepted: Boolean(r.cookieConsentAccepted ?? r.CookieConsentAccepted),
      safehouseId: sh === undefined || sh === null ? undefined : Number(sh),
    };
  }

  const refreshUser = useCallback(async () => {
    try {
      const data = await apiFetch<unknown>('/api/auth/me');
      setUser(toAuthUser(data));
      setMfaRequired(false);
    } catch {
      setUser(null);
    }
  }, []);

  useEffect(() => {
    setAuthErrorHandlers({
      onUnauthorized: () => {
        setUser(null);
        setMfaRequired(false);
        const path = window.location.pathname;
        if (!path.startsWith('/login')) {
          navigate('/login?session=expired', { replace: true });
        }
      },
      onForbidden: () => {
        // Optional: replace with toast when available
        console.warn('Forbidden (403)');
      },
    });
    return () => setAuthErrorHandlers({});
  }, [navigate]);

  useEffect(() => {
    void refreshUser().finally(() => setLoading(false));
  }, [refreshUser]);

  useEffect(() => {
    const onFocus = () => {
      void refreshUser();
    };
    window.addEventListener('focus', onFocus);
    return () => window.removeEventListener('focus', onFocus);
  }, [refreshUser]);

  const login = useCallback(async (email: string, password: string) => {
    setLoading(true);
    try {
      const data = await apiFetch<Record<string, unknown>>('/api/auth/login', {
        method: 'POST',
        body: JSON.stringify({ email, password }),
      });
      if (data.requiresMfa) {
        setMfaRequired(true);
        return;
      }
      setUser(toAuthUser(data));
      setMfaRequired(false);
    } finally {
      setLoading(false);
    }
  }, []);

  const validateMfa = useCallback(async (code: string) => {
    setLoading(true);
    try {
      const data = await apiFetch<unknown>('/api/auth/mfa/validate', {
        method: 'POST',
        body: JSON.stringify({ code }),
      });
      setUser(toAuthUser(data));
      setMfaRequired(false);
    } finally {
      setLoading(false);
    }
  }, []);

  const logout = useCallback(async () => {
    await apiFetch('/api/auth/logout', { method: 'POST' });
    setUser(null);
    setMfaRequired(false);
  }, []);

  const value = useMemo(
    () => ({
      user,
      loading,
      mfaRequired,
      login,
      validateMfa,
      logout,
      refreshUser,
    }),
    [user, loading, mfaRequired, login, validateMfa, logout, refreshUser],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within AuthProvider');
  }
  return context;
}
