# Title Edit Chronicle

A fork of [Title Edit](https://github.com/RokasKil/TitleEdit) (attick, perchbird & Speedas) that makes the vanilla title screen and idle movie follow **the currently logged-in character's Main Scenario**.

## Why this fork exists

Patch 7.1 added "Title Screen Displayed on Launch". That setting:

- picks the **furthest** character on the service account, not the one you just played
- is **locked** on Free Trial accounts
- often waits for a **full client relaunch** before the new title appears

Chronicle fixes all three.

| | Vanilla 7.5 | Title Edit | Chronicle |
|---|---|---|---|
| Custom 3D title / character select | No | Yes | Yes (inherited) |
| Follow MSQ automatically | Furthest character | Manual preset | **This character** |
| Free Trial | Forced title | Manual preset only | **Automatic, if the client has the files** |
| Apply without relaunch | No | When you pick a preset | **On logout / MSQ change** |

Free Trial still cannot load expansion data you do not have on disk (Endwalker+). ARR through Shadowbringers — the trial's actual story — map correctly.

## How it tracks MSQ

On login, logout, and a short poll while you play, Chronicle:

1. Resolves expansion-start quests from the live `Quest` sheet (`Coming to Ishgard`, `Beyond the Great Wall`, `The Syrcus Trench`, `The Next Hunt` / `Old Sharlayan, New to You`, `A New World to Explore`)
2. Treats a quest as reached if it is **complete or accepted** (story skips included)
3. Falls back to scanning Main Scenario journal genres if a gate name moves
4. Stores the result **per Content ID** and remembers the last played character
5. Points Title Edit's vanilla preset + logo + movie at that expansion
6. Calls `ReloadTitleScreen()` so the lobby you return to is already correct

## Commands

- `/titleedit` or `/te` — configuration
- `/titleedit msq` — print the mapped expansion for this character
- `Ctrl+T` on the title screen — same as original Title Edit

## License

AGPL-3.0, same as Title Edit V3. Credit to attick, perchbird, Speedas / RokasKil, and the original Title Edit contributors.
