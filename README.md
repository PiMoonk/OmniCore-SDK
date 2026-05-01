\# OMNI CORE

\### Universal Asynchronous Layer-0 Hyper-Router

\*\*HFT-Grade Networking Engine for High-Frequency Synchronization\*\*



\---



\## 1. Executive Summary

Omni Core is a sub-microsecond latency networking engine designed for massive-scale, real-time synchronization. By utilizing a \*\*Lock-Free Ring Buffer\*\* architecture and \*\*POSIX Raw Sockets\*\* (AF\_PACKET L2 Bypass), it circumvents standard kernel overhead, ensuring true \*\*Zero-GC (Garbage Collection)\*\* operation, even within managed environments like Unity.



\*   \*\*Layer-0 Routing:\*\* Direct hardware-to-application packet injection.

\*   \*\*Zero-Copy Memory:\*\* Payloads are processed without intermediate allocations or memory copying.

\*   \*\*Asynchronous Pipeline:\*\* Decoupled Producer/Consumer threads, explicitly pinned to dedicated CPU cores.



\---



\## 2. Package Structure

This release edition includes the following components:



\*   `Server/omni\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_core` : The central High-Performance Binary.

\*   `Server/plugins/` : Dynamic C++ modules (e.g., Diagnostic Tentacle v1.1).

\*   `Unity\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\_SDK/OmniClient.cs` : The high-performance, pure C# networking bridge.

\*   `README.md` : Integration and deployment guide.



\---



\## 3. Server Deployment (Linux)

Omni Core requires a Linux environment with elevated (\*\*root\*\*) privileges to access raw sockets.



1\.  \*\*Set Permissions:\*\*

&#x20;   ```bash

&#x20;   chmod +x ./Server/omni\_core

&#x20;   ```

2\.  \*\*Execution:\*\*

&#x20;   Run the binary using `sudo` to activate the AF\_PACKET interceptor:

&#x20;   ```bash

&#x20;   sudo ./Server/omni\_core

&#x20;   ```

3\.  \*\*CLI Commands:\*\*

&#x20;   \*   `status` : Displays real-time routing metrics and latency telemetry.

&#x20;   \*   `reload` : Hot-swaps modules on the fly with zero downtime.

&#x20;   \*   `exit`   : Initiates a graceful system shutdown.



\---



\## 4. Unity Integration

To integrate Omni Core into your Unity project:



1\.  Import the `OmniClient.cs` file into your `Assets/Scripts/` directory.

2\.  Initialize the client (handling network logic on a dedicated thread is recommended):

&#x20;   ```csharp

&#x20;   using OmniCore.SDK;

&#x20;   var client = new OmniClient("YOUR\_SERVER\_IP", 9000);

&#x20;   ```

3\.  Transmit data within your `Update()` loop or during a dedicated synchronization step:

&#x20;

```csharp

\\\\\\\\\\\\\\\&#x20;   client.SendHFT(0x01, payload, payload.Length);

\\\\\\\\\\\\\\\&#x20;   ```



\\\\\\\\\\\\\\\\---



\\\\\\\\\\\\\\\\## 5. Diagnostics and Telemetry

While the server is running, use the `status` command to monitor:

\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*CPU Latency (Cycles):\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\* Packet processing speed measured in CPU clock cycles.

\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*Packet Throughput:\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\* Total number of processed packets and bytes.

\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*   \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*Ring Backlog:\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\* The saturation level of the internal buffer (optimally 0).



\\\\\\\\\\\\\\\\---



\\\\\\\\\\\\\\\\## 6. Support and Licensing

This software is provided under a \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*Commercial Partner License\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\* strictly for the \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*Interstate 2026\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\* project. For technical support, custom module development, or scaling consultations, please contact the lead architect via your established communication channels.






\\\\\\\\\\\\\\\\---






\\\\\\\\## 7. Custom Plugin Development (Server\\\\\\\\\\\\\\\_SDK)

To create your own server-side game logic (e.g., Character Controller, Hit Registration), you must compile a C++ shared library (`.so`) using the provided `omnicore\\\\\\\\\\\\\\\_sdk.h` interface.



1\\\\\\\\. \\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\*Write your logic:\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\* Use the provided `examples/\\\\\\\\\\\\\\\*.cpp` files as a template.

2\\\\\\\\. \\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\*Compile the plugin:\\\\\\\\\\\\\\\*\\\\\\\\\\\\\\\* Use the following `g++` command on your Linux server to compile your custom C++ file into a shared object. (This ensures maximum optimization and ABI compatibility).



\\\\\\\&#x20;  ```bash

\\\\\\\&#x20;  g++ -O3 -fPIC -shared -o Plugin\\\\\\\\\\\\\\\_MyGameLogic.so MyGameLogic.cpp



\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*Omni Core — Precision Engineering for the Future of Connectivity.\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\*





