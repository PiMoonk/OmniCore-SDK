# OMNI CORE - Universal Asynchronous Layer-0 Hyper-Router
**HFT-Grade Networking Engine for High-Frequency Synchronization**

---

## ⚠️ Note for Enterprise & Studio Architects
This repository contains the **Open Source C++ SDK** and **Unity C# Clients** for the Omni Core architecture. 

If you require the pre-compiled, production-ready Linux server binary (`omni_core`) optimized for true sub-microsecond latency, zero-GC C# payload injection, and unrestricted commercial use, the **Enterprise Binary License** is available here:
👉 **[Acquire the Enterprise Binary via Gumroad]([https://gumroad.com/your-link-here](https://mariodreams46.gumroad.com/l/OmniCore))**

For B2B integrations, custom HFT parser development, or architecture consultation, connect with the lead architect:
👉 **[Márió Nádasi on LinkedIn](hwww.linkedin.com/in/márió-nádasi-70821a3a9)**

---

## 1. Executive Summary
Omni Core is a sub-microsecond latency networking engine designed for massive-scale, real-time synchronization. By utilizing a **Lock-Free Ring Buffer** architecture and **POSIX Raw Sockets** (AF_PACKET L2 Bypass), it circumvents standard kernel overhead, ensuring true **Zero-GC (Garbage Collection)** operation, even within managed environments like Unity.

* **Layer-0 Routing:** Direct hardware-to-application packet injection.
* **Zero-Copy Memory:** Payloads are processed without intermediate allocations or memory copying.
* **Asynchronous Pipeline:** Decoupled Producer/Consumer threads, explicitly pinned to dedicated CPU cores.

---

## 2. Open Source Package Structure
This public repository includes the developer tools necessary to build custom logic for the Omni Core engine:

* `Server_SDK/` : C++ headers (`omnicore_sdk.h`) and interfaces for building dynamic logic plugins.
* `Server_SDK/examples/` : Example C++ modules to demonstrate Lock-Free state management.
* `Unity_SDK/OmniClient.cs` : The high-performance, pure C# networking bridge for Unity3D.

---

## 3. Server Deployment (Commercial Binary)
*Note: This step requires the compiled `omni_core` Linux binary from the Enterprise package.*

Omni Core requires a Linux environment with elevated (**root**) privileges to access raw sockets.

1. **Set Permissions:**
```bash
chmod +x ./omni_core
Execution:
Run the binary using sudo to activate the AF_PACKET interceptor:

Bash
sudo ./omni_core
CLI Commands:

status : Displays real-time routing metrics and latency telemetry.

reload : Hot-swaps C++ logic modules on the fly with zero downtime.

exit : Initiates a graceful system shutdown.

4. Unity Integration
To integrate the Omni Core client into your Unity project:

Import the OmniClient.cs file into your Assets/Scripts/ directory.

Initialize the client (handling network logic on a dedicated thread is recommended):

C#
using OmniCore.SDK;

// Initialize connection to the bare-metal server
var client = new OmniClient("YOUR_SERVER_IP", 9000);
Transmit data within your Update() loop or during a dedicated synchronization step:

C#
client.SendHFT(0x01, payload, payload.Length);
5. Diagnostics and Telemetry
While the server is running, use the status command to monitor:

CPU Latency (Cycles): Packet processing speed measured in CPU clock cycles.

Packet Throughput: Total number of processed packets and bytes.

Ring Backlog: The saturation level of the internal buffer (optimally 0 under maximum stress).

6. Custom Plugin Development (Server SDK)
To create your own server-side game logic (e.g., Character Controller, Hit Registration, Trading Logic), you must compile a C++ shared library (.so) using the provided omnicore_sdk.h interface.

Write your logic: Use the provided examples/*.cpp files as a template.

Compile the plugin: Use the following g++ command on your Linux server to compile your custom C++ file into a shared object. (This ensures maximum optimization and ABI compatibility).

Bash
g++ -O3 -fPIC -shared -o Plugin_MyGameLogic.so MyGameLogic.cpp
7. Licensing
SDK & Clients (This Repository): Open Source (MIT License). You are free to study, modify, and build custom plugins.

Omni Core Engine (Binary): Requires a Commercial Enterprise License for production deployment.

Omni Core — Precision Engineering for the Future of Connectivity.
