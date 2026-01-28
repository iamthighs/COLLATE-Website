"use client";

import Navbar from '../../../components/layout/Navbar'
import Sidenav from '../../../components/layout/Sidenav'
import Footer from '../../../components/layout/Footer'
import MainHeaderAdmin from '../../../components/layout/MainHeaderAdmin'
import DataTable from '../../../components/layout/DataTable'
import { subjects } from '../../mockData/mockData';

export default function ListSubjectsPage(){
  return (
    <>
      
      <Navbar />
      <div id="layoutSidenav">
        <Sidenav />
        <div id="layoutSidenav_content">
          <main>
            {/* ---------- Header ---------- */}
            <MainHeaderAdmin
              title="Subjects"
              icon="folder"
              importAction={() => console.log("Import subjects")}
              createHref="/subjects/create" />
            {/* ---------- Content ---------- */}
            <div className="container-xl px-4">
              {/* Notification placeholder */}
              {/* <Notification /> */}
              <DataTable
                data={subjects}
                categoryKey="header"
                titleKey="title"
                dateKey="postedDate"
                editUrlBase="/events/edit" />
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
