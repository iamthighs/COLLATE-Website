"use client";

import Link from "next/link";
import { useEffect, useState, useRef } from "react";
import { supabase } from "../../lib/supabase/supabaseClient";

export default function Navbar() {
  const [user, setUser] = useState(null);
  const [profile, setProfile] = useState(null);
  const [showDropdown, setShowDropdown] = useState(false);
  const dropdownRef = useRef(null);

  useEffect(() => {
    const fetchUser = async () => {
      const { data: { session } } = await supabase.auth.getSession();
      if (!session?.user) return;

      setUser(session.user);

      const { data: profileData } = await supabase
        .from("profiles")
        .select("*")
        .eq("id", session.user.id)
        .single();

      setProfile(profileData);
    };

    fetchUser();

    const { data: listener } = supabase.auth.onAuthStateChange((_event, session) => {
      if (!session?.user) {
        setUser(null);
        setProfile(null);
      } else {
        supabase
          .from("profiles")
          .select("*")
          .eq("id", session.user.id)
          .single()
          .then(({ data }) => setProfile(data));
      }
    });

    // Close dropdown if click outside
    const handleClickOutside = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setShowDropdown(false);
      }
    };
    document.addEventListener("mousedown", handleClickOutside);

    return () => {
      listener.subscription.unsubscribe();
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleLogout = async () => {
    await supabase.auth.signOut();
    setUser(null);
    setProfile(null);
  };

  return (
    <nav className="topnav navbar navbar-expand shadow justify-content-between justify-content-sm-start navbar-light bg-white">
      <button className="btn btn-icon btn-transparent-dark me-2" id="sidebarToggle">
        <i data-feather="menu"></i>
      </button>

      <Link href="/" className="pe-3 ps-4 ps-lg-2">
        <img src="/Logo PNG1.svg" style={{ height: 100, width: 100 }} alt="logo" />
      </Link>

      <ul className="navbar-nav align-items-center ms-auto">
        {user && profile ? (
          <li
            className="nav-item dropdown no-caret dropdown-user me-3 me-lg-4"
            ref={dropdownRef}
            style={{ position: "relative" }}
          >
            <button
              className="btn btn-icon btn-transparent-dark dropdown-toggle"
              onClick={() => setShowDropdown(!showDropdown)}
              aria-expanded={showDropdown}
            >
              <img
                className="img-fluid"
                src={profile.avatar_url || "/default-user.png"}
                alt="User"
              />
            </button>

            <div
              className={`dropdown-menu dropdown-menu-end border-0 shadow animated--fade-in-up ${
                showDropdown ? "show" : ""
              }`}
            >
              <h6 className="dropdown-header d-flex align-items-center">
                <img
                  className="dropdown-user-img"
                  src={profile.avatar_url || "/default-user.png"}
                  alt="User"
                />
                <div className="dropdown-user-details">
                  <div className="dropdown-user-details-name">
                    {profile.first_name} {profile.last_name}
                  </div>
                  <div className="dropdown-user-details-email">{user.email}</div>
                </div>
              </h6>

              <div className="dropdown-divider"></div>

              <Link href="/account" className="dropdown-item">
                <div className="dropdown-item-icon">
                  <i data-feather="settings"></i>
                </div>
                Account
              </Link>

              <button className="dropdown-item" onClick={handleLogout}>
                <div className="dropdown-item-icon">
                  <i data-feather="log-out"></i>
                </div>
                Logout
              </button>
            </div>
          </li>
        ) : (
          <>
            <li className="nav-item me-3 me-lg-4">
              <Link href="/register" className="nav-link">Register</Link>
            </li>
            <li className="nav-item me-3 me-lg-4">
              <Link href="/login" className="nav-link">Login</Link>
            </li>
          </>
        )}
      </ul>
    </nav>
  );
}
