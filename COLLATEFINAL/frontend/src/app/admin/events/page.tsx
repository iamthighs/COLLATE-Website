"use client";

import Navbar from '../../../components/layout/Navbar'
import Sidenav from '../../../components/layout/Sidenav'
import Footer from '../../../components/layout/Footer'
import DataTable from '../../../components/layout/DataTable'
import MainHeaderAdmin from '../../../components/layout/MainHeaderAdmin'
import { events, researchPapers } from '../../mockData/mockData';

export default function ListEventsPage(){
  return (
    <>
      
      <Navbar />
      <div id="layoutSidenav">
        <Sidenav />
        <div id="layoutSidenav_content">
          <main>
            {/* ---------- Header ---------- */}
            <MainHeaderAdmin
              title="Events"
              icon="calendar"
              importAction={() => console.log("Import events")}
              createHref="/events/create" />
            {/* ---------- Content ---------- */}
            <div className="container-xl px-4">
              {/* Notification placeholder */}
              {/* <Notification /> */}
              <DataTable
                data={events}
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
