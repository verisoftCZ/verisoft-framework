import { MenuKeyboardFocusA11y } from '@verisoft/ui-core';

export function findGovButton(host: HTMLElement): HTMLElement | null {
  const direct = host.querySelector('gov-button');
  if (direct instanceof HTMLElement) {
    return direct;
  }

  const nested = host.getElementsByTagName('gov-button');
  return nested.length > 0 ? nested[0] : null;
}

export function readGovButtonFocusFromHost(
  host: HTMLElement,
): HTMLButtonElement | null {
  const govButton = findGovButton(host);
  if (!govButton) {
    return host instanceof HTMLButtonElement ? host : null;
  }

  const shadowRoot = govButton.shadowRoot;
  if (!shadowRoot) {
    return null;
  }

  const nativeButton =
    shadowRoot.querySelector('button.element') ??
    shadowRoot.querySelector('button[type="button"]') ??
    shadowRoot.querySelector('button');

  return nativeButton instanceof HTMLButtonElement ? nativeButton : null;
}

function applyAccessibility(
  focusElement: HTMLButtonElement,
  a11y: MenuKeyboardFocusA11y,
): void {
  if (a11y.role !== undefined) {
    focusElement.setAttribute('role', a11y.role);
  }

  if (a11y.ariaHaspopup !== undefined) {
    focusElement.setAttribute('aria-haspopup', a11y.ariaHaspopup);
  }

  if (a11y.ariaExpanded !== undefined) {
    focusElement.setAttribute('aria-expanded', a11y.ariaExpanded);
  }

  if (a11y.ariaControls !== undefined) {
    focusElement.setAttribute('aria-controls', a11y.ariaControls);
  }

  if (a11y.ariaDisabled !== undefined) {
    focusElement.setAttribute('aria-disabled', a11y.ariaDisabled);
  }

  if (a11y.tabIndex !== undefined) {
    focusElement.tabIndex = a11y.tabIndex;
  }
}

export function applyGovButtonAccessibility(
  host: HTMLElement,
  a11y: MenuKeyboardFocusA11y,
): void {
  const focusElement = readGovButtonFocusFromHost(host);
  if (!focusElement) {
    return;
  }

  applyAccessibility(focusElement, a11y);
}

export function focusGovButtonHost(
  host: HTMLElement,
  a11y?: MenuKeyboardFocusA11y,
): void {
  const focusElement = readGovButtonFocusFromHost(host);

  if (focusElement) {
    if (a11y) {
      applyAccessibility(focusElement, a11y);
    }

    focusElement.focus({ preventScroll: true });
    return;
  }

  if (host instanceof HTMLButtonElement) {
    host.focus({ preventScroll: true });
    return;
  }

  host.focus({ preventScroll: true });
}
