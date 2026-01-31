"use client";

import { useEffect, useState } from "react";
import Navbar from '../../../components/layout/Navbar';
import Sidenav from '../../../components/layout/Sidenav';
import Footer from '../../../components/layout/Footer';
import MainHeaderAdmin from '../../../components/layout/MainHeaderAdmin';
import DataTable from '../../../components/layout/DataTable';
import { getResearchPapers } from "../../../lib/supabase/researchPaper";

interface ResearchPaper {
  id: string;
  header: string;
  title: string;
  postedDate: string;
}

export default function ListResearchPapersPage() {
  const [data, setData] = useState<ResearchPaper[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
  async function fetchData() {
    console.log("[fetchData] Starting fetch...");

    const timeoutMs = 10000; // 10 seconds

    // Timeout promise
    const timeout = new Promise<{ data: null; error: string }>((_, reject) =>
      setTimeout(() => reject(new Error("Fetch timed out")), timeoutMs)
    );

    try {
      // Race the fetch against the timeout
      const { data: papers, error } = await Promise.race([
        getResearchPapers(),
        timeout,
      ]) as { data: any[] | null; error: any };

      if (error) {
        console.error("[fetchData] Supabase error:", error);
        setData([]);
      } else {
        console.log("[fetchData] Raw papers:", papers);

        const mappedData = papers?.map((paper: any) => ({
          id: paper.id,
          header: paper.header ?? "No header",
          title: paper.title ?? "No title",
          postedDate: paper.posted_date ?? new Date().toISOString(),
        })) ?? [];

        console.log("[fetchData] Mapped data:", mappedData);
        setData(mappedData);
      }
    } catch (err) {
      console.error("[fetchData] Timeout or unexpected error:", err);
      setData([]);
    } finally {
      console.log("[fetchData] Finished, setting loading=false");
      setLoading(false);
    }
  }

  fetchData();
}, []);


  console.log("[render] Data length:", data.length, "Loading:", loading);

  return (
    <>
      <Navbar />
      <div id="layoutSidenav">
        <Sidenav />
        <div id="layoutSidenav_content">
          <main>
            <MainHeaderAdmin 
              title="Research Papers"
              icon="file-text"
              importAction={() => console.log("Import research papers")}
              createHref="admin/research/create"
            />
            <div className="container-xl px-4">
              <DataTable
                data={data}
                loading={loading}
                categoryKey="header"
                titleKey="title"
                dateKey="postedDate"
                editUrlBase="/admin/research/edit"
              />
            </div>
          </main>
          <Footer />
        </div>
      </div>
    </>
  );
}
