type Props = {
  progress: number;
  error?: string | null;
};

export default function SplashScreen({ progress, error }: Props) {
  const spinnerClass = error
    ? "spinner-border spinner-border-sm text-danger mb-2"
    : "spinner-border spinner-border-sm text-success mb-2";

  const progressBarClass = error
    ? "progress-bar bg-danger progress-bar-striped progress-bar-animated"
    : "progress-bar bg-success progress-bar-striped progress-bar-animated";

  return (
    <div className="position-fixed top-0 start-0 w-100 h-100 bg-light d-flex align-items-center justify-content-center z-50">
      <div
        className="card shadow-none border-0 text-center"
        style={{ width: 320 }}
      >
        <div className="card-body py-4 px-4">
          {/* Logo */}
          <div className="mb-3">
            <img
              src="/Logo PNG1.svg"
              style={{ height: 100, width: 100 }}
              alt="logo"
            />
            <h5 className="fw-bold text-success mb-0">COLLATE</h5>
            <small className="text-muted">
              Collection of Latest Laboratory Activities, Trainings &amp; Engagements
            </small>
          </div>

          {/* Loader / Error */}
          <div className="mb-3">
            <div className={spinnerClass} />
            <div className="text-gray-600 small">
              {error ? `Error: ${error}` : "Initializing application…"}
            </div>
          </div>

          {/* Progress */}
          <div className="progress mb-1" style={{ height: 6 }}>
            <div
              className={progressBarClass}
              style={{ width: `${progress}%` }}
            />
          </div>

          <small className="text-muted">{progress}%</small>
        </div>
      </div>
    </div>
  );
}
