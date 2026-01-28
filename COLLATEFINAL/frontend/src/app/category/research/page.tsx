import Navbar from '../../../components/layout/Navbar'
import Sidenav from '../../../components/layout/Sidenav'
import Footer from '../../../components/layout/Footer'
import MainHeader from '../../../components/layout/MainHeader'
export const metadata = {
  title: 'COLLATE - Research Papers'
}
export default function ResearchPage(){
  return (
    <>
      
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
