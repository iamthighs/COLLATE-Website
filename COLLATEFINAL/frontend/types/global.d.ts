// types/global.d.ts
export {}

declare global {
  interface Window {
    feather?: {
      replace: () => void
    }

    toastr?: {
      options?: {
        closeButton?: boolean
        [key: string]: unknown
      }
      success?: (msg: string, title?: string) => void
      error?: (msg: string, title?: string) => void
      info?: (msg: string, title?: string) => void
      warning?: (msg: string, title?: string) => void
    }
  }
}
