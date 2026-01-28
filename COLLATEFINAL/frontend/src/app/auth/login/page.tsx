"use client";

import { useState } from "react";
import { supabase } from "../../../lib/supabase/supabaseClient";
import { useRouter } from "next/navigation";

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    const { data, error: loginError } = await supabase.auth.signInWithPassword({
      email,
      password,
    });

    setLoading(false);

    if (loginError) {
      setError(loginError.message);
      return;
    }

    // Redirect after successful login
    router.push("/dashboard"); // replace with your protected route
  };

  return (
    <div
      className="bg-img-cover"
      style={{ backgroundImage: "url('/new/assets/img/bg-scene.svg')" }}
    >
      <div id="layoutAuthentication">
        <div id="layoutAuthentication_content">
          <main>
            <div className="container-xl px-4">
              <div className="row justify-content-center">
                <div className="col-xl-5 col-lg-6 col-md-8 col-sm-11">
                  <div className="card my-5 rounded-xl">
                    {/* Header */}
                    <div className="card-body text-center">
                      <a href="/">
                        <img
                          className="mb-2"
                          src="/Logo PNG.png"
                          style={{ height: 50 }}
                          alt="Logo"
                        />
                      </a>

                      <div className="h3 fw-light mb-3">Sign In</div>

                      {/* Social buttons (optional) */}
                      <div>
                        <p>
                          <button
                            type="button"
                            className="btn btn-icon btn-google mx-1"
                            aria-label="Login with Google"
                          >
                            <i className="fab fa-google fa-fw fa-sm" />
                          </button>

                          <button
                            type="button"
                            className="btn btn-icon btn-facebook mx-1"
                            aria-label="Login with Facebook"
                          >
                            <i className="fab fa-facebook-f fa-fw fa-sm" />
                          </button>
                        </p>
                      </div>
                    </div>

                    <hr className="my-0" />

                    {/* Login form */}
                    <div className="card-body p-5">
                      <form onSubmit={handleLogin}>
                        {error && (
                          <div className="alert alert-danger">{error}</div>
                        )}

                        <div className="mb-3">
                          <label className="text-gray-600 small">
                            Email address
                          </label>
                          <input
                            type="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            maxLength={100}
                            className="form-control form-control-solid"
                            placeholder="Enter your Email"
                            required
                          />
                        </div>

                        <div className="mb-3">
                          <label className="text-gray-600 small">
                            Password
                          </label>
                          <input
                            type="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            maxLength={100}
                            className="form-control form-control-solid"
                            placeholder="Enter your Password"
                            required
                          />
                        </div>

                        <div className="mb-3">
                          <a className="small" href="/forgot-password">
                            Forgot your password?
                          </a>
                        </div>

                        <div className="d-flex align-items-center justify-content-between mb-0">
                          <div className="form-check">
                            <input
                              className="form-check-input"
                              type="checkbox"
                              id="rememberMe"
                            />
                            <label
                              className="form-check-label"
                              htmlFor="rememberMe"
                            >
                              Remember password
                            </label>
                          </div>

                          <button
                            type="submit"
                            className="btn btn-primary"
                            disabled={loading}
                          >
                            {loading ? "Logging in..." : "Login"}
                          </button>
                        </div>
                      </form>
                    </div>

                    <hr className="my-0" />

                    {/* Footer */}
                    <div className="card-body px-5 py-4">
                      <div className="small text-center">
                        New user? <a href="/auth/register">Create an account!</a>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </main>
        </div>
      </div>
    </div>
  );
}
