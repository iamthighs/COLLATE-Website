"use client";
import { useEffect } from "react";
import { usePathname } from "next/navigation";

export default function FeatherInitializer() {
  const pathname = usePathname();

  useEffect(() => {
    if (!window.feather) return;


    const interval = setInterval(() => {
      const icons = document.querySelectorAll("i[data-feather]");

      if (icons.length > 0) {
        window.feather.replace();
        clearInterval(interval);
      }
    }, 50); 

    return () => clearInterval(interval);
  }, [pathname]);

  return null;
}
