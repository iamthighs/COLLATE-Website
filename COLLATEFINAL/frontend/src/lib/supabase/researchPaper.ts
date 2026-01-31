import { supabase } from "./client";

export async function getResearchPapers() {
  return supabase
    .from("research_papers")
    .select("*")
    .order("posted_date", { ascending: false });
}

export async function createResearchPaper(payload: any) {
  return supabase.from("research_papers").insert(payload);
}

export async function updateResearchPaper(id: string, payload: any) {
  return supabase
    .from("research_papers")
    .update(payload)
    .eq("id", id);
}

export async function deleteResearchPaper(id: string) {
  return supabase
    .from("research_papers")
    .delete()
    .eq("id", id);
}

export async function deleteMultipleResearchPapers(ids: string[]) {
  if (ids.length === 0) return { data: [], error: null };

  return supabase
    .from("research_papers")
    .delete()
    .in("id", ids);
}