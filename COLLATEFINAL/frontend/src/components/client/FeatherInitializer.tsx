"use client"
import { useEffect } from 'react'
import { usePathname } from 'next/navigation'

export default function FeatherInitializer(){
  const pathname = usePathname()

  useEffect(()=>{
    function replaceOnce(){
      try{
        if(typeof window !== 'undefined' && window.feather && typeof window.feather.replace === 'function'){
          window.feather.replace()
        }
      }catch(e){/* ignore */}
    }

    // If feather is already loaded, run immediately; otherwise poll briefly until available.
    if(typeof window !== 'undefined'){
      if(window.feather && typeof window.feather.replace === 'function'){
        replaceOnce()
      }else{
        const id = setInterval(()=>{
          if(window.feather && typeof window.feather.replace === 'function'){
            replaceOnce()
            clearInterval(id)
          }
        }, 50)
        return ()=>clearInterval(id)
      }
    }
  }, [pathname])

  return null
}
