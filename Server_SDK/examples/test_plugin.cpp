// ============================================================================
// OMNI CORE - EXAMPLE PLUGIN (DIAGNOSTIC MODULE)
// UNIVERSAL ASYNCHRONOUS SUPERCOMPUTING FRAMEWORK
// ============================================================================

#include "../include/omnicore_sdk.h"
#include <iostream>
#include <atomic>
#include <cstdint>
#include <cstddef>

namespace omni {
    namespace plugins {

        class DiagnosticTentacle : public core::IOmniModule {
        private:
            // Cache-line optimization:
            // Telemetry data is forced into a single 64-byte aligned block 
            // (typical L1 cache line size). This allows the CPU to fetch both 
            // variables in a single instruction, avoiding cache misses.
            struct alignas(64) TelemetryData {
                std::atomic<uint64_t> payloads_processed{ 0 };
                std::atomic<uint64_t> total_bytes{ 0 };
            };

            TelemetryData m_telemetry;

        public:
            DiagnosticTentacle() = default;
            ~DiagnosticTentacle() override = default;

            // 1. Module Identity (Used by Dashboard and CLI)
            const char* get_module_name() const noexcept override {
                return "Diagnostic_Tentacle_v1.1_Optimized";
            }

            // 2. O(1) Router Index (Target OpCode 0x00 - 0xFF)
            uint8_t get_target_opcode() const noexcept override {
                return 0x01; // This module processes packets with OpCode 0x01
            }

            // 3. HOT PATH: Zero-Copy payload ingestion
            // Note: The 'buffer' parameter is unnamed to avoid -Wunused-parameter warnings.
            // WARNING: Strictly avoid blocking calls (e.g., std::cout, file I/O) in this method,
            // as it executes on the High-Frequency Trading (HFT) core routing thread.
            void on_data_received(const void* /*buffer*/, size_t size) noexcept override {
                m_telemetry.payloads_processed.fetch_add(1, std::memory_order_relaxed);
                m_telemetry.total_bytes.fetch_add(size, std::memory_order_relaxed);
            }

            // 4. Telemetry Reporting (Triggered by CLI 'status' command)
            void on_heartbeat() noexcept override {
                uint64_t processed = m_telemetry.payloads_processed.load(std::memory_order_relaxed);
                uint64_t bytes = m_telemetry.total_bytes.load(std::memory_order_relaxed);

                std::cout << "         [\033[1;36mDIAGNOSTICS\033[0m] OpCode [0x01] | Payloads processed: "
                    << processed << " | Bytes: " << bytes << "\n";
            }
        };

    } // namespace plugins
} // namespace omni

// ============================================================================
// EXPORTED ENTRY POINT (Required for POSIX dlopen dynamic loading)
// ============================================================================
extern "C" OMNICORE_API omni::core::IOmniModule* omnicore_create_module() {
    return new omni::plugins::DiagnosticTentacle();
}