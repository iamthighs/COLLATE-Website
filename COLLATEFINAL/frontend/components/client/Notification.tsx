"use client"
import { useEffect } from 'react'

export default function Notification() {
  useEffect(()=>{
    // Example: configure toastr defaults if available
    if(typeof window !== 'undefined' && window.toastr){
      window.toastr.options = window.toastr.options || {};
      window.toastr.options.closeButton = true;
    }
  },[])

  return null
}
