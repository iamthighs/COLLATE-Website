import Script from 'next/script'
import FeatherInitializer from '../components/FeatherInitializer'
import SidebarToggleInitializer from '../components/SidebarToggleInitializer'

export const metadata = {
  title: 'COLLATE - Home Page'
}

export default function RootLayout({ children }) {
  return (
    <html lang="en">
      <head>
        <meta charSet="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <link rel="icon" type="image/x-icon" href="/Logo PNG1.png" />
        <link href="/new/css/styles.css" rel="stylesheet" />
        <link href="https://cdn.jsdelivr.net/npm/simple-datatables@latest/dist/style.css" rel="stylesheet" />
        <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" />
      </head>
      <body className="nav-fixed">
        {children}
        <FeatherInitializer />
        <SidebarToggleInitializer />

        {/* Load globals in order — keep jQuery before legacy plugins */}
        <Script src="https://code.jquery.com/jquery-3.7.1.min.js" strategy="beforeInteractive" />
        <Script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js" strategy="beforeInteractive" />
        <Script src="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.1.1/js/all.min.js" strategy="afterInteractive" />
        <Script src="https://cdnjs.cloudflare.com/ajax/libs/feather-icons/4.28.0/feather.min.js" strategy="afterInteractive" />
        <Script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/3.11.174/pdf.min.js" strategy="afterInteractive" />

        {/* Local theme scripts copied from the ASP.NET app */}
        <Script src="/new/js/scripts.js" strategy="afterInteractive" />
        <Script src="/new/js/datatables/datatables-simple-demo.js" strategy="afterInteractive" />

        {/* Optional libraries loaded from CDN or local public files */}
        <Script src="https://cdn.jsdelivr.net/npm/simple-datatables@latest" strategy="afterInteractive" />
        <Script src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js" strategy="afterInteractive" />

      </body>
    </html>
  )
}
