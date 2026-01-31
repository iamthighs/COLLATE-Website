"use client";
import { useEffect } from "react";

export default function FeatherInitializer() {
  useEffect(() => {
    if (!window.feather) return;

    // Replace any existing icons immediately
    window.feather.replace();

    // Observe DOM changes
    const observer = new MutationObserver((mutations) => {
      for (const mutation of mutations) {
        if (mutation.addedNodes.length > 0) {
          window.feather.replace();
        }
      }
    });

    observer.observe(document.body, {
      childList: true,
      subtree: true,
    });

    return () => observer.disconnect();
  }, []);

  return null;
}
