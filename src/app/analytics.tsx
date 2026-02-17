"use client";
import { useEffect } from "react";
import Clarity from '@microsoft/clarity';

export default function Analytics(): null {
  useEffect(() => {
    try {
      const consent = localStorage.getItem("cookie-consent");
      if (!consent) return;
    } catch {
      // ignore
    }
    // Load GA5/GA4
    const ga = document.createElement("script");
    ga.async = true;
    ga.src = "https://www.googletagmanager.com/gtag/js?id=G-6F2WT9Z6VZ";
    document.head.appendChild(ga);
    const gadata = document.createElement("script");
    gadata.innerHTML =
      "window.dataLayer = window.dataLayer || []; function gtag(){dataLayer.push(arguments);} gtag('js', new Date()); gtag('config', 'G-6F2WT9Z6VZ', { 'page_path': window.location.pathname });";
    document.head.appendChild(gadata);

    // Clarity
    const projectId = "vij1v6lpkc"
    Clarity.init(projectId);
  }, []);
  return null;
}
