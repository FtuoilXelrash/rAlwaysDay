# rAlwaysDay

[![Rust](https://img.shields.io/badge/Rust-Game-red)](https://rust.facepunch.com/)
[![Umod](https://img.shields.io/badge/Umod-Framework-blue)](https://umod.org/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
[![Version](https://img.shields.io/badge/Version-1.0.35-brightgreen)](https://github.com/FtuoilXelrash/rAlwaysDay/releases)

## Overview

**rAlwaysDay** is a lightweight Rust server plugin that automatically skips nighttime periods on your server. Configure a time window, and the plugin handles the rest—seamlessly transitioning from night to morning without player interaction or server disruption.

## Key Features

- **Automatic Night Skipping** - Triggered by server time within configured window
- **Instant Transition** - Immediately jumps to morning (07:00 AM by default)
- **Fire-and-Forget Setup** - Only 3 configuration options needed
- **Performance Optimized** - Less than 0.1% CPU usage (one check per minute)
- **Zero Dependencies** - Requires only Oxide/uMod framework
- **Reliable Architecture** - Simple logic design ensures maximum uptime
- **Automatic Configuration** - Validates settings with sensible fallback defaults
- **Clear Error Logging** - Troubleshooting made easy

## Quick Installation

1. Download `rAlwaysDay.cs` from the [releases page](https://github.com/FtuoilXelrash/rAlwaysDay/releases)
2. Place it in your server's `oxide/plugins/` directory
3. Restart your server or use `oxide.reload rAlwaysDay`
4. Configuration file auto-generates at `oxide/config/rAlwaysDay.json`

## Requirements

- **Rust Dedicated Server** - Any version
- **Oxide/uMod Framework** - Required for plugin system
- **No external plugins** - Zero dependencies

## Configuration

The plugin auto-generates a configuration file at `oxide/config/rAlwaysDay.json`:

### Configuration Options

| Setting | Description | Default |
|---------|-------------|---------|
| `Auto-skip start time` | When the skip window opens | `20:50` |
| `Auto-skip end time` | When the skip window closes | `21:00` |
| `Time to set after skip` | What time to jump to (morning) | `07:00` |

### Example Configuration

```json
{
  "Auto-Skip Settings": {
    "Auto-skip start time": "20:50",
    "Auto-skip end time": "21:00",
    "Time to set after skip": "07:00"
  }
}
```

### How It Works

1. **Monitor** - Plugin checks server time every minute
2. **Detect** - When the current time enters your configured window (e.g., 20:50-21:00)
3. **Skip** - Server time instantly jumps to morning (e.g., 07:00 AM)
4. **Repeat** - Cycle continues automatically each night

### Configuration Limitations

**The plugin does not support time windows that cross midnight:**

- ✅ `20:50` → `21:00` works perfectly
- ❌ `23:00` → `01:00` is not supported

Both start and end times must be on the same calendar day. This keeps the logic simple and reliable.

## Performance & Balance

### CPU & Memory Profile

- **CPU Usage**: < 0.1% (one time check per minute)
- **Memory Usage**: Negligible (stateless operation, no data storage)
- **OnMinute Hook Calls**: 1,440 per day (standard frequency)
- **Server Impact**: Minimal - safe for any server size

### Technical Architecture

- **Minute-based checks** - Evaluates time window via `OnMinute` hook
- **Direct time manipulation** - Modifies server time through Rust's `TOD_Time` system
- **Stateless operation** - No persistence overhead or state management
- **Automatic configuration** - Validates settings with sensible fallback defaults
- **Automatic restart handling** - Survives server restarts and plugin reloads
- **Clear error logging** - Easy troubleshooting

## Troubleshooting

### Night is not skipping at configured time

**Solution**: Verify your `Auto-skip start time` and `Auto-skip end time` are:
- In the correct 24-hour format (e.g., `20:50` not `8:50 PM`)
- On the same calendar day (midnight crossing not supported)
- Correctly spelled in the JSON (no typos)

After changes, reload the plugin: `oxide.reload rAlwaysDay`

### Server time seems wrong or not updating

**Solution**:
- Check that the `Time to set after skip` setting is valid (e.g., `07:00`)
- Ensure no other plugins are manipulating server time
- Verify Oxide/uMod is properly installed and plugins have permission to hook time events

### Plugin not loading

**Solution**:
- Check server console for errors: Look for `[rAlwaysDay]` messages
- Verify the `.cs` file is in `oxide/plugins/` directory
- Ensure Oxide/uMod framework is installed and running
- Try manual reload: `oxide.reload rAlwaysDay`

## Support & Community

### Report Issues
Found a bug or have a feature request? Open an issue on [GitHub](https://github.com/FtuoilXelrash/rAlwaysDay/issues)

### Get Help
- Check this README first
- Review the Troubleshooting section above
- Post in [Umod Forums](https://umod.org/plugins) for community help
- Join the [Rust Modding Community](https://www.rust.facepunch.com/)

### Downloads
- [Latest Release](https://github.com/FtuoilXelrash/rAlwaysDay/releases/latest)
- [All Versions](https://github.com/FtuoilXelrash/rAlwaysDay/releases)

## Contributing

Contributions are welcome! Here's how to get started:

1. **Fork** the repository on GitHub
2. **Create a branch** for your feature (`git checkout -b feature/amazing-feature`)
3. **Commit your changes** (`git commit -m 'Add amazing feature'`)
4. **Push to the branch** (`git push origin feature/amazing-feature`)
5. **Open a Pull Request** with a description of your changes

Please ensure your code follows the existing style and includes clear commit messages.

## License

This plugin is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

## Author

**Ftuoil Xelrash**

- GitHub: [@FtuoilXelrash](https://github.com/FtuoilXelrash)
- Codefling: [Downloads](https://codefling.com/profile/51007-ftuoil-xelrash/content/?type=downloads_file)

For questions or feedback, reach out via GitHub or the Rust modding community.
