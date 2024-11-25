This is a quick guide to help you get started with profiling RimWorld using a custom build of the Mono runtime matching its Unity version.

This approach leverages the fact that Linux has a powerful kernel-level profiling tool (`perf`) and that Mono, the .NET runtime used by Unity/RimWorld, already implements the necessary scaffolding that allows `perf` to find the symbols for the JITed managed code. All that is needed is a tiny patch to allow toggling the JIT symbol map generation.

1. Check out this repository and branch somewhere convenient.
2. Get the [build dependencies](https://www.mono-project.com/docs/compiling-mono/linux/).
3. Set appropriate compiler flags to get a build that will be suitable for profiling:
```sh
# PIC is needed to successfully link against dependent libraries
export LDFLAGS="-fpic"
# Preserve frame pointers and debug symbols.
export CFLAGS="-O2 -fPIC -fno-omit-frame-pointer -g -pipe
```
4. Build Mono:
```
$ ./autogen.sh
$ make -j$(nproc)
```
Note: On recent systems, the build may fail when attempting to compile the C# standard libraries shipped by Mono. This can be safely ignored, since we do not need those to profile RimWorld—we only need the runtime itself, which will have already been compiled by then.

5. Back up the original Mono runtime bundled with RimWorld and copy over the new one:
```
$ cp ~/.local/share/Steam/steamapps/common/RimWorld/RimWorldLinux_Data/MonoBleedingEdge/x86_64/libmonobdwgc-2.0.so /somewhere/safe
$ cp mono/mini/.libs/libmonobdwgc-2.0.so.1.0.0 ~/.local/share/Steam/steamapps/common/RimWorld/RimWorldLinux_Data/MonoBleedingEdge/x86_64/libmonobdwgc-2.0.so
```
6. Configure RimWorld to start with the environment variable `MONO_DEBUG=disable_omit_fp`, which will disable frame pointers in JIT-ed code and activate JIT map creation for `perf` at the same time. This is easiest via Steam launch options.
7. Start RimWorld to check if the Mono runtime is working as intended. There should be a `/tmp/perf-PID.map` file created with RimWorld's PID if everything went well.
8. Clone https://github.com/brendangregg/FlameGraph somewhere.
9. Profile RimWorld to your liking, and use Brendan Gregg's scripts or `perf` itself to make sense of the output. See [Brendan Gregg's site](https://brendangregg.com/perf.html) for many useful examples.
For instance, to capture stack samples of a running RimWorld for 60 seconds at 99 Hz, and turn the results into a flamegraph:
```
# perf record -F 99 -p $(pidof RimWorldLinux) -g -- sleep 60
# perf script | /path/to/FlameGraph/stackcollapse-perf.pl | /path/to/FlameGraph/flamegraph.pl > graph.svg
```
