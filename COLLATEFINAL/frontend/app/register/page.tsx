"use client";
import { useState } from "react";
import { supabase } from "../../lib/supabaseClient";
import { useRouter } from "next/navigation";

export default function RegisterPage() {
  const router = useRouter();

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleRegister = async (e) => {
    e.preventDefault();
    if (loading) return;
    setError(null);

    if (password !== confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    setLoading(true);

    try {
      // 1️⃣ Sign up the user
      const { data, error: signUpError } = await supabase.auth.signUp({
        email,
        password,
      });
      console.log("signUpError", signUpError);
      if (signUpError) throw signUpError;

      const userId = data.user.id;
      console.log(userId)
      // 2️⃣ Insert user profile (without image)
      const { error: profileError } = await supabase.from("profiles").insert([
        {
          id: userId,
          first_name: firstName,
          last_name: lastName,
          email,
          avatar_url: null, // no image upload
        },
      ]);
      console.log("profileError", profileError);
      if (profileError) throw profileError;

      // 3️⃣ Redirect to login
      router.push("/login");
    } catch (err) {
      setError(err.message || "Something went wrong. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="bg-img-cover" style={{ backgroundImage: "url('/new/assets/img/bg-scene.svg')" }}>
      <div id="layoutAuthentication">
        <div id="layoutAuthentication_content">
          <main>
            <div className="container-xl px-4">
              <div className="row justify-content-center">
                <div className="col-xl-8 col-lg-9">
                  <div className="card my-5 rounded-xl">
                    {/* Header */}
                    <div className="card-body p-5 text-center">
                      <div className="text-center mx-3">
                        <a href="/">
                          <img className="mb-2" src="/Logo PNG.png" style={{ height: 50 }} alt="Logo" />
                        </a>
                      </div>

                      <div className="h3 fw-light mb-3">Create an Account</div>
                      <div className="small text-muted mb-2">Sign in using...</div>

                      <div>
                        <p>
                          <button type="button" className="btn btn-icon btn-google mx-1">
                            <i className="fab fa-google fa-fw fa-sm" />
                          </button>
                          <button type="button" className="btn btn-icon btn-facebook mx-1">
                            <i className="fab fa-facebook-f fa-fw fa-sm" />
                          </button>
                        </p>
                      </div>
                    </div>

                    <hr className="my-0" />

                    {/* Form */}
                    <div className="card-body p-5">
                      <div className="text-center small text-muted mb-4">...or enter your information below.</div>

                      <form onSubmit={handleRegister}>
                        {/* Name row */}
                        <div className="row gx-3">
                          <div className="col-md-6">
                            <div className="mb-3">
                              <label className="text-gray-600 small">First name</label>
                              <input
                                type="text"
                                className="form-control form-control-solid"
                                placeholder="Enter your First Name"
                                value={firstName}
                                onChange={(e) => setFirstName(e.target.value)}
                                required
                              />
                            </div>
                          </div>

                          <div className="col-md-6">
                            <div className="mb-3">
                              <label className="text-gray-600 small">Last name</label>
                              <input
                                type="text"
                                className="form-control form-control-solid"
                                placeholder="Enter your Last Name"
                                value={lastName}
                                onChange={(e) => setLastName(e.target.value)}
                                required
                              />
                            </div>
                          </div>
                        </div>

                        {/* Email */}
                        <div className="mb-3">
                          <label className="text-gray-600 small">Email address</label>
                          <input
                            type="email"
                            className="form-control form-control-solid"
                            placeholder="Enter Email Address"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                          />
                        </div>

                        {/* Password row */}
                        <div className="row gx-3">
                          <div className="col-md-6">
                            <div className="mb-3">
                              <label className="text-gray-600 small">Password</label>
                              <input
                                type="password"
                                className="form-control form-control-solid"
                                placeholder="Enter strong password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                              />
                            </div>
                          </div>

                          <div className="col-md-6">
                            <div className="mb-3">
                              <label className="text-gray-600 small">Confirm Password</label>
                              <input
                                type="password"
                                className="form-control form-control-solid"
                                placeholder="Repeat Password"
                                value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)}
                                required
                              />
                            </div>
                          </div>
                        </div>

                        {/* Error */}
                        {error && <p className="text-danger">{error}</p>}

                        {/* Submit */}
                        <div className="d-flex align-items-center justify-content-between">
                          <div className="form-check">
                            <input className="form-check-input" id="checkTerms" type="checkbox" required />
                            <label className="form-check-label" htmlFor="checkTerms">
                              I accept the <a href="#!">terms &amp; conditions</a>.
                            </label>
                          </div>

                          <button className="btn btn-primary" type="submit" disabled={loading}>
                            {loading ? "Creating..." : "Create Account"}
                          </button>
                        </div>
                      </form>
                    </div>

                    <hr className="my-0" />

                    {/* Footer */}
                    <div className="card-body px-5 py-4">
                      <div className="small text-center">
                        Have an account? <a href="/login">Sign in!</a>
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
