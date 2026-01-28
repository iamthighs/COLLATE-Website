"use client";

import Navbar from '../../../components/layout/Navbar'
import Sidenav from '../../../components/layout/Sidenav'
import Footer from '../../../components/layout/Footer'
import MainHeaderAdmin from '../../../components/layout/MainHeaderAdmin'
import DataTable from '../../../components/layout/DataTable'
import { roles } from '../../mockData/mockData';

export default function ListRolesPage(){
  return (
    <>
      
      <Navbar />
      <div id="layoutSidenav">
        <Sidenav />
        <div id="layoutSidenav_content">
          <main>
            {/* ---------- Header ---------- */}
            <MainHeaderAdmin 
              title="Roles"
              icon="settings"
              importAction={() => console.log("Import roles")}
              createHref="/roles/create"/>
            {/* ---------- Content ---------- */}
            <div className="container-xl px-4">
              {/* Notification placeholder */}
              {/* <Notification /> */}
              <DataTable
                data={roles}
                categoryKey="header"
                titleKey="title"
                dateKey="postedDate"
                editUrlBase="/roles/edit" />
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
