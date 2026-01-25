import Navbar from '../../components/layout/Navbar'
import Sidenav from '../../components/layout/Sidenav'
import Footer from '../../components/layout/Footer'

export const metadata = {
  title: 'COLLATE - About Us'
}
export default function AboutPage(){
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

            <div className="container-xl px-4">
              <div className="row">
                <div className="col-lg-6">
                  <div className="card card-waves mb-4">
                    <div className="card-header mb-3 text-center">
                      <h1 className="m-0 font-weight-bold text-success text-center">ABOUT US</h1>
                    </div>
                    <div className="card-body">
                      <p className="text-center p-3">This team started during the year 1st of college composing of only 5 members until it grows and had a 2nd generation. We are mostly specialized with C# Language, and applications that can be made for game and web development such as ASP.Net Framework and Unity Engine. Consistent seeking for improvement, knowledge and skills are the best practices of our group.</p>
                      <div className="row text-center">
                        <div className="col-sm-12">
                          <img className="lift img-smlogo-modal rounded-circle mb-3" src="/VOLTEZ-4.0.png" alt="logo1" />
                          <img className="lift img-smlogo-modal mb-3" src="/img/logoscene.png" alt="scene" />
                          <img className="lift img-smlogo-modal rounded-circle mb-3" src="/pup-logo.png" alt="pup" />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="col-lg-6">
                  <div className="card mb-4">
                    <div className="card-header mb-3 text-center">
                      <h3 className="m-0 font-weight-bold text-success text-center">MEET THE TEAM</h3>
                    </div>
                    <div className="card-body">
                      <div className="card-body row text-center">
                        <div className="col-lg-4 col-sm-4 p-1">
                          <img className="lift shadow img-smlogo-modal rounded-circle mb-3" src="/about-us/tys.jpg" alt="tys" />
                          <h6 className="font-weight-bold">Reynaldo Cortez Jr.</h6>
                          <p className="small">Software Developer</p>
                        </div>
                        <div className="col-lg-4 col-sm-4 p-1">
                          <img className="lift shadow img-smlogo-modal rounded-circle mb-3" src="/about-us/jolee.jpeg" alt="jolee" />
                          <h6 className="font-weight-bold">Julie-An Adio</h6>
                          <p className="small">System Analyst</p>
                        </div>
                        <div className="col-lg-4 col-sm-4 p-1">
                          <img className="lift shadow img-smlogo-modal rounded-circle mb-3" src="/about-us/leix.jpeg" alt="leix" />
                          <h6 className="font-weight-bold">Leixander Gomez</h6>
                          <p className="small">UI/UX Designer</p>
                        </div>
                      </div>

                      <div className="text-center">
                        <h5 className="m-0 font-weight-bold text-success text-center">KEEP IN TOUCH</h5>
                      </div>
                      <div className="my-3 text-center">
                        <a href="#" className="btn btn-facebook btn-circle btn-sm"><i className="fab fa-facebook-f"></i></a>
                        <a href="#" className="btn btn-google btn-circle btn-sm"><i className="fab fa-google"></i></a>
                        <a href="#" className="btn btn-warning btn-circle btn-sm"><i className="fab fa-instagram"></i></a>
                        <a href="#" className="btn btn-primary btn-circle btn-sm"><i className="fab fa-linkedin"></i></a>
                        <a href="#" className="btn btn-info btn-circle btn-sm"><i className="fab fa-twitter"></i></a>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="text-center">
                  <button className="btn btn-primary mb-3" data-bs-toggle="modal" data-bs-target="#feedbackModal">Send us your Feedback!</button>
                </div>
              </div>
            </div>
          </main>
          <Footer />
        </div>
      </div>

      <div
        className="modal fade"
        id="feedbackModal"
        tabIndex={-1}
        role="dialog"
        aria-labelledby="exampleModalCenterTitle"
        aria-hidden="true"
      >

        <div className="modal-dialog modal-dialog-centered" role="document">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title" id="exampleModalCenterTitle">Send us your feedback</h5>
              <button className="btn-close" type="button" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="text-danger"></div>
            <form className="user" action="#" method="post">
              <div className="modal-body">
                <div className="row">
                  <div className="col-2">
                    <p className="my-1 text-center font-weight-bold">From:</p>
                  </div>
                  <div className="col-10">
                    <input name="FullName" className="form-control mb-2" placeholder="Enter your Name" />
                  </div>
                  <div className="col-12">
                    <textarea name="Feedback" className="form-control" rows={4} placeholder="Type your suggestions here..."></textarea>
                  </div>
                </div>
              </div>
              <div className="modal-footer">
                <button className="btn btn-secondary" type="button" data-bs-dismiss="modal">Close</button>
                <input type="submit" className="btn btn-primary" value="Submit" />
              </div>
            </form>
          </div>
        </div>
      </div>
    </>
  )
}
