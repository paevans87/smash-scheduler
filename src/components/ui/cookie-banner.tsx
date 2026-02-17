"use client";
import React, { useEffect, useState } from "react";

export default function CookieBanner(): React.ReactNode {
  const [visible, setVisible] = useState(false);

  useEffect(() => {
    try {
      const consent = localStorage.getItem("cookie-consent");
      if (!consent) {
        setVisible(true);
      }
    } catch {
      // ignore
    }
  }, []);

  const accept = () => {
    try {
      localStorage.setItem("cookie-consent", "true");
    } catch {
      // ignore
    }
    setVisible(false);
  };

  if (!visible) return null;

  return (
    <div
      aria-live="polite"
      className="fixed bottom-0 left-0 right-0 md:left-8 md:right-8 bg-white border border-gray-200 dark:bg-slate-900 dark:border-gray-700 rounded shadow-lg z-50 p-4 md:p-6 flex flex-col md:flex-row items-center justify-between gap-4 dark:text-slate-100"
    >
      <div className="text-sm text-muted-foreground dark:text-slate-100">
        We use cookies to improve your experience. By continuing you agree to our use of cookies.
      </div>
      <div className="flex gap-2">
        <button className="px-4 py-2 rounded bg-primary text-primary-foreground hover:bg-primary-600 dark:hover:bg-primary-700" onClick={accept}>
          Accept
        </button>       
      </div>
    </div>
  );
}
