/**
 * Construct the API base URL from environment variables
 * In production, this will use the Azure-hosted backend API
 * In development, defaults to localhost:5000
 */
const getApiBase = (): string => {
  const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

  if (apiBaseUrl) {
    return apiBaseUrl;
  }

  const apiHost = import.meta.env.VITE_API_HOST || 'localhost';
  const apiPort = import.meta.env.VITE_API_PORT || '5000';

  return `http://${apiHost}:${apiPort}/api`;
};

const API_BASE = getApiBase();

export type AuthErrorHandlers = {
  onUnauthorized?: () => void;
  onForbidden?: (message?: string) => void;
};

let authHandlers: AuthErrorHandlers = {};

/** Register once from AuthProvider — single interception point for 401/403 (see auth guidelines). */
export function setAuthErrorHandlers(handlers: AuthErrorHandlers) {
  authHandlers = handlers;
}

/** POST /api/auth/login and POST /api/auth/mfa/validate return 401 for invalid credentials — not session expiry. */
function shouldSkipGlobal401(path: string, init?: RequestInit): boolean {
  const method = (init?.method ?? 'GET').toUpperCase();
  if (method !== 'POST') return false;
  const lower = path.toLowerCase();
  return lower.includes('/api/auth/login') || lower.includes('/api/auth/mfa/validate');
}

export async function apiFetch<T>(path: string, init?: RequestInit): Promise<T> {
  const normalizedPath =
    API_BASE.endsWith('/api') && path.startsWith('/api/')
      ? path.replace(/^\/api/, '')
      : path;

  const response = await fetch(`${API_BASE}${normalizedPath}`, {
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
      ...(init?.headers ?? {}),
    },
    ...init,
  });

  const text = await response.text();

  if (!response.ok) {
    if (response.status === 401 && !shouldSkipGlobal401(path, init)) {
      authHandlers.onUnauthorized?.();
    } else if (response.status === 403) {
      authHandlers.onForbidden?.(text || undefined);
    }
    throw new Error(text || `Request failed: ${response.status}`);
  }

  if (response.status === 204) {
    return undefined as T;
  }

  if (!text) {
    return undefined as T;
  }

  return JSON.parse(text) as T;
}
