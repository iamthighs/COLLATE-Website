import Link from 'next/link'

export default function Footer(){
  const year = new Date().getFullYear();
  return (
    <footer className="footer-admin mt-auto footer-light">
      <div className="container-xl px-4">
        <div className="row">
          <div className="col-md-6 small">Copyright © COLLATE {year}</div>
          <div className="col-md-6 text-md-end small">
            <Link href="/privacy">Privacy Policy</Link>
            &nbsp;&middot;&nbsp;
            <Link href="/terms">Terms &amp; Conditions</Link>
          </div>
        </div>
      </div>
    </footer>
  )
}
