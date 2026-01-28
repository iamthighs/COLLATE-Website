"use client";

import Navbar from '../../../components/layout/Navbar'
import Sidenav from '../../../components/layout/Sidenav'
import Footer from '../../../components/layout/Footer'
import MainHeaderAdmin from '../../../components/layout/MainHeaderAdmin'
import DataTable from '../../../components/layout/DataTable'
import { softwareProjects } from '../../mockData/mockData';

export default function ListSoftwareProjectsPage(){
  return (
    <>
      
      <Navbar />
      <div id="layoutSidenav">
        <Sidenav />
        <div id="layoutSidenav_content">
          <main>
            {/* ---------- Header ---------- */}
            <MainHeaderAdmin 
              title="Software Projects"
              icon="code"
              importAction={() => console.log("Import software projects")}
              createHref="/software-projects/create"/>
            {/* ---------- Content ---------- */}
            <div className="container-xl px-4">
              {/* Notification placeholder */}
              {/* <Notification /> */}
              <DataTable
                data={softwareProjects}
                categoryKey="header"
                titleKey="title"
                dateKey="postedDate"
                editUrlBase="/software-projects/edit" />
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
