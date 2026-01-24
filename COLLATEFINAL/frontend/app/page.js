import Navbar from '../components/Navbar'
import Sidenav from '../components/Sidenav'
import Footer from '../components/Footer'

export default function HomePage(){
  return (
    <>
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

            <div className="container-xl px-4">
              <div className="card shadow-none rounded-xl card-waves mb-4 mt-5">
                <div className="card-body p-5">
                  <div className="row align-items-center justify-content-between">
                    <div className="col-8">
                      <h2 className="text-green fw-bolder">Students of Computer Engineering with Natural Excellence (SCENE)</h2>
                      <p className="text-gray-700">SCENE is an organization in PUP-Paranaque that serves as the representatives of computer engineering students.</p>
                      <p className="text-gray-700">It was formed way back in 2013 with its  first president, Mr. John Michael Dabu. Currently, SCENE has 200 students and managed by Prof. Engr. Marvin De Pedro, the Bachelor of Science in Computer Engineering Coordinator.</p>
                    </div>
                    <div className="col-4 d-none d-lg-block mt-xxl-n4"><img className="img-fluid px-xl-4 text-center mt-xxl-n5" src="/img/logoscene.png" style={{maxHeight:300}} alt="SCENE logo"/></div>
                  </div>
                </div>
              </div>

              <div className="row">
                <div className="col-lg-5 mb-4">
                  <div className="card shadow-none rounded-xl mb-4">
                    <div className="card-header text-center p-3">
                      <h4 className="text-green fw-bolder">Vision</h4>
                    </div>
                    <div className="card-body text-center">
                      <p className="mb-4">Students of Computer Engineering with Natural Excellence (SCENE) serves as beacon for Computer Engineering Students of PUP Paranaque, supports its members in improving their skills, experience, and knowledge as the age of technology continous to grow into the next generation.</p>
                    </div>
                  </div>
                </div>
                <div className="col-lg-7 mb-4">
                  <div className="card shadow-none rounded-xl">
                    <div className="card-header text-center p-3">
                      <h4 className="text-green fw-bolder">Philosophy</h4>
                    </div>
                    <div className="card-body">
                      <div className="timeline timeline-xs">
                        {["To represent the Computer Engineering Students of Polytechnic University of the Philippines - Paranaque Campus.","To promote general welfare of its members.","To act as liaison between the Computer Engineering Students on one hand and the campus (and the university as a whole) on the other hand and shall cooperate with the other organization such as Central Student Council and other Organizations.","To promote educational, physical and social well being of its members.","To acquaint its members on the new emerging technologies.","To strengthen SCENE's linkages to other organization, not only in the University but also in the nation as a whole."].map((text, idx)=>(
                          <div className="timeline-item" key={idx}>
                            <div className="timeline-item-marker">
                              <div className="timeline-item-marker-indicator bg-green"></div>
                            </div>
                            <div className="timeline-item-content">{text}</div>
                          </div>
                        ))}
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </main>
          <Footer />
        </div>
      </div>
    </>
  )
}
