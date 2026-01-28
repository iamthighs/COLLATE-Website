"use client";

import Navbar from '../../../components/layout/Navbar'
import Sidenav from '../../../components/layout/Sidenav'
import Footer from '../../../components/layout/Footer'
import MainHeaderAdmin from '../../../components/layout/MainHeaderAdmin'
import DataTable from '../../../components/layout/DataTable'
import { researchPapers } from '../../mockData/mockData';


export default function ListResearchPapersPage(){
  return (
    <>
      
      <Navbar />
      <div id="layoutSidenav">
        <Sidenav />
        <div id="layoutSidenav_content">
          <main>
            {/* ---------- Header ---------- */}
            <MainHeaderAdmin 
              title="Research Papers"
              icon="file-text"
              importAction={() => console.log("Import research papers")}
              createHref="/research-papers/create"/>
            {/* ---------- Content ---------- */}
            <div className="container-xl px-4">
              {/* Notification placeholder */}
              {/* <Notification /> */}
              <DataTable
              data={researchPapers}
              categoryKey="header"
              titleKey="title"
              dateKey="postedDate"
              editUrlBase="/research-papers/edit" />
            </div>
            {/* ---------- Modals (placeholders) ---------- */}
            {/* <DeleteModal /> */}
            {/* <BulkImportModal /> */}
          </main>
          <Footer />
        </div>
      </div>
    </>
  )
}
