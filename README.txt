===============================================================================
                                rAlwaysDay
===============================================================================

Rust Game | Umod Framework | MIT License | v1.0.40

===============================================================================
OVERVIEW
===============================================================================

rAlwaysDay is a lightweight Rust server plugin that automatically skips
nighttime periods on your server. Configure a time window, and the plugin
handles the rest—seamlessly transitioning from night to morning without
player interaction or server disruption.

===============================================================================
KEY FEATURES
===============================================================================

* Automatic Night Skipping - Triggered by server time within configured window
* Instant Transition - Immediately jumps to morning (07:00 AM by default)
* Fire-and-Forget Setup - Only 3 configuration options needed
* Performance Optimized - Less than 0.1% CPU usage (one check per minute)
* Zero Dependencies - Requires only Oxide/uMod framework
* Reliable Architecture - Simple logic design ensures maximum uptime
* Automatic Configuration - Validates settings with sensible fallback defaults
* Clear Error Logging - Troubleshooting made easy

===============================================================================
QUICK INSTALLATION
===============================================================================

1. Download rAlwaysDay.cs from the releases page
2. Place it in your server's oxide/plugins/ directory
3. Restart your server or use "oxide.reload rAlwaysDay"
4. Configuration file auto-generates at oxide/config/rAlwaysDay.json

===============================================================================
REQUIREMENTS
===============================================================================

* Rust Dedicated Server - Any version
* Oxide/uMod Framework - Required for plugin system
* No external plugins - Zero dependencies

===============================================================================
CONFIGURATION
===============================================================================

The plugin auto-generates a configuration file at oxide/config/rAlwaysDay.json

CONFIGURATION OPTIONS:

Setting                    | Description                      | Default
---------------------------|----------------------------------|----------
Auto-skip start time       | When the skip window opens       | "20:50"
Auto-skip end time         | When the skip window closes      | "21:00"
Time to set after skip     | What time to jump to (morning)   | "07:00"

EXAMPLE CONFIGURATION:

{
  "Auto-Skip Settings": {
    "Auto-skip start time": "20:50",
    "Auto-skip end time": "21:00",
    "Time to set after skip": "07:00"
  }
}

HOW IT WORKS:

1. MONITOR - Plugin checks server time every minute via the OnMinute hook

2. DETECT - When the current time enters your configured window
   (e.g., 20:50-21:00)

3. SKIP ONCE - Server time instantly jumps to the configured morning time
   (e.g., 07:00 AM) - ONLY happens once per cycle

4. RESET - Flag resets when the skip window closes, ready for next night

5. REPEAT - Cycle continues automatically each night

SKIP TIMING DETAILS:

* The skip occurs on the FIRST minute the current time enters the window
* Subsequent minutes in the same window do NOT trigger additional skips
  (prevents repeated jumps)
* The flag resets once the window closes (after your end time)

TARGET TIME CALCULATION:

* If your skip window is 20:50-21:00 and you set the target time to 07:00,
  the plugin jumps to the NEXT DAY'S 07:00 if the current time is already
  past 07:00 today
* This ensures the skip always moves time forward to morning, never backward

CONFIGURATION LIMITATIONS:

The plugin DOES NOT support time windows that cross midnight:

  (OK)  20:50 -> 21:00 works perfectly
  (NO)  23:00 -> 01:00 is not supported

Both start and end times must be on the same calendar day. This keeps the
logic simple and reliable.

===============================================================================
PERFORMANCE & BALANCE
===============================================================================

CPU & MEMORY PROFILE:

* CPU Usage: < 0.1% (one time check per minute)
* Memory Usage: Negligible (stateless operation, no data storage)
* OnMinute Hook Calls: 1,440 per day (standard frequency)
* Server Impact: Minimal - safe for any server size

TECHNICAL ARCHITECTURE:

* Minute-based checks - Evaluates time window via OnMinute hook
* Direct time manipulation - Modifies server time through Rust's TOD_Time
* Stateless operation - No persistence overhead or state management
* Automatic configuration - Validates settings with sensible fallback defaults
* Automatic restart handling - Survives server restarts and plugin reloads
* Clear error logging - Easy troubleshooting

STARTUP INITIALIZATION:

When the plugin starts, it attempts to locate the TOD_Sky component
(Rust's time-of-day system):

* Attempts: Up to 50 tries with 3-second intervals between attempts
* Maximum wait time: ~150 seconds (2.5 minutes)
* If found: Plugin immediately begins monitoring the skip window
* If not found: Plugin logs error and disables (won't function without TOD_Sky)

This ensures the plugin initializes properly even if the game world takes
time to fully load.

CONFIGURATION BEHAVIOR:

If your configuration file contains invalid time values (incorrect format
or syntax):

* Fallback: Plugin automatically reverts to default times (20:50, 21:00, 07:00)
* Logging: An error message logs to server console explaining the issue
* Continue: Plugin still functions with defaults so your server stays working

===============================================================================
TROUBLESHOOTING
===============================================================================

NIGHT IS NOT SKIPPING AT CONFIGURED TIME:

Solution: Verify your Auto-skip start time and Auto-skip end time are:
* In the correct 24-hour format (e.g., "20:50" not "8:50 PM")
* On the same calendar day (midnight crossing not supported)
* Correctly spelled in the JSON (no typos)

After changes, reload the plugin: oxide.reload rAlwaysDay

SERVER TIME SEEMS WRONG OR NOT UPDATING:

Solution:
* Check that the Time to set after skip setting is valid (e.g., "07:00")
* Ensure no other plugins are manipulating server time
* Verify Oxide/uMod is properly installed and plugins have permission

PLUGIN NOT LOADING:

Solution:
* Check server console for errors: Look for [rAlwaysDay] messages
* Verify the .cs file is in oxide/plugins/ directory
* Ensure Oxide/uMod framework is installed and running
* Try manual reload: oxide.reload rAlwaysDay

DOUBLE SKIP WHEN RELOADING DURING SKIP WINDOW:

Issue: If you reload the plugin (oxide.reload rAlwaysDay) while the server
time is within the configured skip window, it may trigger an additional skip.

Explanation: The plugin's skip-once flag resets when the plugin is reloaded.
If you reload while in the window (e.g., 20:55), the flag resets and could
trigger another skip on the next minute.

Solution:
* Reload the plugin outside of the skip window (not between your start and
  end times)
* Or plan configuration changes for off-hours when skips don't occur
* This is edge-case behavior and safe to ignore if reloads happen infrequently

===============================================================================
SUPPORT & COMMUNITY
===============================================================================

* Report Issues - https://github.com/FtuoilXelrash/rAlwaysDay/issues
  Bug reports and feature requests

* Discord Support - https://discord.gg/G8mfZH2TMp
  Join our community for help and discussions

* Download Latest - https://github.com/FtuoilXelrash/rAlwaysDay/releases
  Always get the newest version

===============================================================================
DEVELOPMENT & TESTING SERVER
===============================================================================

Darktidia Solo Only - See rAlwaysDay and other custom plugins in action!

All players are welcome to join our development server where plugins are
tested and refined in a live environment.

Server Details:
* Name: Darktidia Solo Only | Monthly | 2x | 50% Upkeep | No BP Wipes
* Find Server: https://www.battlemetrics.com/servers/rust/33277489

Experience the plugin live, test configurations, and provide feedback in a
real gameplay setting.

===============================================================================
CONTRIBUTING
===============================================================================

Contributions are welcome! Here's how to get started:

1. Fork the repository on GitHub
2. Create a branch for your feature (git checkout -b feature/amazing-feature)
3. Commit your changes (git commit -m 'Add amazing feature')
4. Push to the branch (git push origin feature/amazing-feature)
5. Open a Pull Request with a description of your changes

Please ensure your code follows the existing style and includes clear
commit messages.

===============================================================================
LICENSE
===============================================================================

This plugin is licensed under the MIT License.

For full license details, see the LICENSE file in the repository.

===============================================================================
AUTHOR
===============================================================================

Ftuoil Xelrash

* GitHub: https://github.com/FtuoilXelrash
* Codefling: https://codefling.com/profile/51007-ftuoil-xelrash/content/?type=downloads_file

For questions or feedback, reach out via GitHub or the Rust modding
community.

===============================================================================
                           End of Documentation
===============================================================================
