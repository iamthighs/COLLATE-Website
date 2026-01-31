type Loader = () => Promise<unknown>;

export async function loadWithProgress(
  loaders: Loader[],
  onProgress: (p: number) => void,
  overallTimeoutMs: number = 5000 // max 20s for all loaders
) {
  const total = loaders.length;
  let done = 0;

  console.group("[AppBootstrap] Resource loading started");
  console.log(`Total loaders: ${total}`);

  const tick = () => {
    done += 1;
    const progress = Math.round((done / total) * 100);
    console.log(`[AppBootstrap] Loader completed (${done}/${total}) → ${progress}%`);
    onProgress(progress);
  };

  // Wrap all loaders with individual timeouts
  const wrappedLoaders = loaders.map((loader, index) =>
    (async () => {
      try {
        console.log(`[AppBootstrap] Loader ${index + 1} started`);

        await Promise.race([
          loader(),
          new Promise<void>((resolve) => setTimeout(() => {
            console.warn(`[AppBootstrap] Loader ${index + 1} timed out`);
            resolve();
          }, 10000)) // 10s per loader
        ]);

        console.log(`[AppBootstrap] Loader ${index + 1} finished`);
      } catch (err) {
        console.error(`[AppBootstrap] Loader ${index + 1} failed`, err);
      } finally {
        tick();
      }
    })()
  );

  // Overall timeout for all loaders
  await Promise.race([
    Promise.all(wrappedLoaders),
    new Promise<void>((resolve) => setTimeout(() => {
      console.warn("[AppBootstrap] Overall loader timeout reached");
      onProgress(100); // force progress to 100%
      resolve();
    }, overallTimeoutMs))
  ]);

  console.log("[AppBootstrap] All loaders resolved or timed out");
  console.groupEnd();
}
