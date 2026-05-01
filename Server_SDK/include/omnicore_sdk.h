// ============================================================================
// OMNI CORE - DEVELOPMENT KIT (SDK) v1.0
// Universal Asynchronous Supercomputing Framework
// ============================================================================

#pragma once

#include <cstdint>
#include <cstddef>

// Dynamic library export macros for cross-platform ABI compatibility
#if defined(_WIN32)
#define OMNICORE_API __declspec(dllexport)
#else
#define OMNICORE_API __attribute__((visibility("default")))
#endif

namespace omni {
    namespace core {

        /// @brief Base interface for all dynamically loaded Omni Core plugins.
        /// Guaranteed 64-byte alignment ensures the object fits perfectly into 
        /// a single L1 CPU cache line, preventing fatal 'False Sharing' degradation.
        class alignas(64) IOmniModule {
        public:
            virtual ~IOmniModule() = default;

            /// @return A null-terminated string representing the module's identity.
            virtual const char* get_module_name() const noexcept = 0;

            /// @return The specific OpCode (0x00 - 0xFF) this module routes.
            virtual uint8_t get_target_opcode() const noexcept = 0;

            /// @brief Hot-path execution method for incoming network payloads.
            /// The 'noexcept' specifier is critical to prevent unhandled exceptions 
            /// from unwinding the stack and crashing the L0 router.
            /// @param buffer Pointer to the zero-copy memory segment.
            /// @param size The length of the payload in bytes.
            virtual void on_data_received(const void* buffer, size_t size) noexcept = 0;

            /// @brief Periodic telemetry callback triggered by the core router.
            virtual void on_heartbeat() noexcept = 0;
        };
    }
}

// C-Linkage export required to prevent C++ name mangling, 
// ensuring reliable POSIX dlopen() resolution.
extern "C" OMNICORE_API omni::core::IOmniModule* omnicore_create_module();