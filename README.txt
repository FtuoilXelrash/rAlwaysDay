===============================================================================
                                rAlwaysDay
===============================================================================

An ultra-lightweight Rust server plugin for Oxide/uMod that automatically 
skips nights during a configured time window. Pure simplicity with zero 
complexity.

===============================================================================
FEATURES
===============================================================================

* Automatic Night Skipping - No player interaction required
* Configurable Time Window - Set when auto-skip should activate  
* Instant Skip Only - Immediately jumps to morning time
* Ultra-Lightweight - Minimal server performance impact
* Zero Dependencies - No external plugins required
* Crystal Clear Configuration - Only 3 simple settings

===============================================================================
INSTALLATION
===============================================================================

1. Download rAlwaysDay.cs 
2. Place it in your server's oxide/plugins/ directory
3. Restart your server or use "oxide.reload rAlwaysDay"

===============================================================================
CONFIGURATION
===============================================================================

The plugin generates a configuration file at oxide/config/rAlwaysDay.json:

{
  "Auto-Skip Settings": {
    "Auto-skip start time": "20:50",
    "Auto-skip end time": "21:00",
    "Time to set after skip": "07:00"
  }
}

Configuration Options:

Setting                    | Description                        | Default
---------------------------|------------------------------------|---------
Auto-skip start time       | When auto-skip window begins      | "20:50"
Auto-skip end time         | When auto-skip window ends        | "21:00"
Time to set after skip     | Time to jump to when skipping     | "07:00"

===============================================================================
HOW IT WORKS
===============================================================================

The plugin automatically monitors server time and skips nights during the 
configured window:

* Between 20:50-21:00 (default): Plugin checks for night skip
* Instant Skip: Immediately jumps server time to 07:00 AM
* Simple Logic: No complex calculations or modes - just pure time jumping

IMPORTANT TIME WINDOW CONFIGURATION:

WARNING: The plugin currently does not support time windows that cross 
midnight (e.g., start time "23:00" and end time "01:00"). 

Supported: "20:50" to "21:00" (WORKS)
Not Supported: "23:00" to "01:00" (DOES NOT WORK)

If you need a time window that crosses midnight, ensure both times are on 
the same day. This limitation may be addressed in future versions.

===============================================================================
SIMPLE OPERATION
===============================================================================

The plugin uses instant skip only:
* Immediately jumps server time to configured morning hour (07:00 AM)
* Most efficient for maintaining constant daylight
* Zero complexity - no modes to choose from

===============================================================================
SERVER COMPATIBILITY
===============================================================================

* Rust Server: Any version
* Framework: Oxide/uMod required
* Dependencies: None
* Performance: Minimal CPU/memory usage

===============================================================================
VERSION HISTORY
===============================================================================

v1.0.35
* Ultra-Simplified Configuration: Only 3 settings remain
* Default Time Changes: Skip window now 20:50-21:00, sets time to 07:00 AM
* Instant Skip Only: Removed all fast night mode complexity
* Pure Auto-Skip: Eliminated all unnecessary day/night cycle management

v1.0.30
* Major Cleanup: Removed ForceSkip option and fast night code
* Always Instant: Plugin now always uses instant skip mode
* Simplified Logic: Cleaner, more reliable operation

v1.0.25
* Configuration Overhaul: Replaced complex time settings with simple auto-skip config
* Removed Day/Night Management: Eliminated unnecessary complexity
* Pure Auto-Skip Focus: Plugin now does one thing perfectly

v1.0.20 - v1.0.10
* Performance optimizations and smart scheduling (later simplified)
* Various stability improvements and bug fixes

v1.0.0
* Initial production release

===============================================================================
DEVELOPER INFORMATION
===============================================================================

* Author: Ftuoil Xelrash
* Framework: Oxide/uMod for Rust
* Language: C#
* License: Not specified

===============================================================================
SUPPORT
===============================================================================

For issues, questions, or contributions, please refer to the plugin's 
repository or Rust modding communities.

===============================================================================
TECHNICAL DETAILS
===============================================================================

Ultra-Simple Architecture:
* Always-On OnMinute Hook: Checks every minute for skip window (simple and reliable)
* Instant Time Jump: Direct manipulation of server time via TOD system
* Zero Complexity: No scheduling, no modes, no state management
* Minimal Resource Usage: Lightweight time checking only

Core Integration:
* Integrates with Rust's Time-of-Day (TOD) system
* Simple OnMinute hook for time checking
* Automatically handles server restarts and reloads
* Validates configuration values with fallback defaults
* Clear error logging for troubleshooting

Performance Profile:
* CPU Usage: Minimal (one time check per minute)
* Memory Usage: Near zero (no data storage)
* OnMinute Calls: 1,440 per day (standard hook frequency)
* Reliability: Maximum (simple logic = fewer failure points)

===============================================================================
                           End of Documentation
===============================================================================