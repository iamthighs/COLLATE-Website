import Navbar from '../../components/layout/Navbar'
import Sidenav from '../../components/layout/Sidenav'
import Footer from '../../components/layout/Footer'
import MainHeader from '../../components/layout/MainHeader'

export const metadata = {
  title: 'COLLATE - Terms & Conditions'
}
export default function TermsPage(){
  return (
    <>
      <style>{`.img-smlogo-modal{height:5rem;width:5rem;}`}</style>
      <Navbar />
      <div id="layoutSidenav">
        <Sidenav />
        <div id="layoutSidenav_content">
          <main>
            <MainHeader/>
          </main>
          <Footer />
        </div>
      </div>

    </>
  )
}
