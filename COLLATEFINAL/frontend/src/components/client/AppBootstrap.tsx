// components/client/AppBootstrap.tsx
"use client";

import { useEffect, useState, type ReactNode } from "react";
import { useAuth } from "../../context/AuthContext";
import { loadWithProgress } from "../../lib/loader/resourceLoader";
import SplashScreen from "./SplashScreen";

interface AppBootstrapProps {
  children: ReactNode;
}
const loaders = [
  // Fonts loader with timeout
  () =>
    Promise.race([
      document.fonts?.ready ?? Promise.resolve(),
      new Promise<void>((resolve) => setTimeout(resolve, 3000)),
    ]),

  // Window load
  () =>
    new Promise<void>((resolve) => {
      if (document.readyState === "complete") {
        resolve();
        return;
      }
      const onLoad = () => resolve();
      window.addEventListener("load", onLoad, { once: true });
    }),

  // Optional: Check if page is reachable
  () =>
    fetch(window.location.href, { method: "HEAD" }).then((res) => {
      if (!res.ok) {
        console.error(`[AppBootstrap] Page returned status ${res.status}`);
        // you can throw to stop bootstrap, or just log
        // throw new Error(`Page load failed with status ${res.status}`);
      }
    }).catch((err) => {
      console.error("[AppBootstrap] Network error:", err);
    }),
];

export default function AppBootstrap({ children }: AppBootstrapProps) {
  const { loading: authLoading } = useAuth();
  const [progress, setProgress] = useState<number>(0);
  const [ready, setReady] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  
  useEffect(() => {
    let mounted = true;

    const bootstrap = async () => {
      try {
        await loadWithProgress(loaders, (p) => mounted && setProgress(p));
        if (mounted) setReady(true);
      } catch (err: any) {
        if (mounted) setError(err.message || "Unknown error");
      }
    };

    void bootstrap();

    return () => {
      mounted = false;
    };
  }, []);

  if (authLoading || !ready) {
    return <SplashScreen progress={progress} error={error} />;
  }
  return <>{children}</>;
}
