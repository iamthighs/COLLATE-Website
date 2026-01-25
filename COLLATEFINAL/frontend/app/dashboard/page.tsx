import Navbar from '../../components/Navbar'
import Sidenav from '../../components/Sidenav'
import Footer from '../../components/Footer'

export const metadata = {
  title: 'COLLATE - Dashboard'
}
export default function DashboardPage(){
  return (
    <>
      <style>{`.img-smlogo-modal{height:5rem;width:5rem;}`}</style>
      <Navbar />
      <div id="layoutSidenav">
        <Sidenav />
        <div id="layoutSidenav_content">
          <main>
            <header className="py-10 mb-4 bg-img-cover" style={{backgroundImage: "url('/new/assets/img/bg-scene.svg')"}}>
              <div className="container-xl px-4">
                <div className="text-center">
                  <img src="/Logo PNG1.png" style={{height:100}} alt="logo" />
                  <h1 className="text-white fw-bolder">Collection of Latest Laboratory Activities, Trainings &amp; Engagements</h1>
                  <p className="lead mb-0 text-white">A project website of SCENE organization designed to provide secure and efficient data management for Computer Engineering students.</p>
                </div>
              </div>
            </header>

            
          </main>
          <Footer />
        </div>
      </div>

    </>
  )
}
