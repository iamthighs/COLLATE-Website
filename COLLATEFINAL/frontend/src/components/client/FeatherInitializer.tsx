"use client"
import { useEffect } from "react"
import { usePathname } from "next/navigation"

export default function FeatherInitializer() {
  const pathname = usePathname()

  useEffect(() => {
    if (typeof window === "undefined") return

    console.log("[Feather] Initializing for pathname:", pathname)

    let attempts = 0
    const maxAttempts = 20 // try up to ~1s
    const interval = setInterval(() => {
      const icons = document.querySelectorAll("i[data-feather]")
      attempts++

      if (icons.length > 0 && window.feather?.replace) {
        console.log("[Feather] Replacing icons now, found:", icons.length)
        window.feather.replace()
        clearInterval(interval)
      } else if (attempts >= maxAttempts) {
        console.log("[Feather] Max attempts reached, applying fallback")
        icons.forEach(el => {
          if (!el.innerHTML) {
            el.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16">
              <circle cx="8" cy="8" r="7" stroke="gray" stroke-width="2" fill="none"/>
            </svg>`
          }
        })
        clearInterval(interval)
      }
    }, 50)

    return () => clearInterval(interval)
  }, [pathname])
}
