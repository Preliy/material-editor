# Unity Material Palette
A simple Editor Window for the quick editing of materials for selected objects. Has similar functionality as Material Editor in [ProBuilder](https://docs.unity3d.com/Packages/com.unity.probuilder@5.2/manual/tool-panels.html). 

Table of Contents:

- [Install Package](#install-package)
    - [Install via OpenUPM](#install-via-openupm)
    - [Install via Git URL](#install-via-git-url)
- [How to Use](#how-to-use)

## Install Package
The package is available on the [npm registry](https://www.npmjs.com/). It's recommended to install it via [npm-cli](https://docs.npmjs.com/cli).
```
npm i com.preliy.material-editor
```

### Install via Git URL
Open *Packages/manifest.json* with your favorite text editor. Add the following line to the dependencies block.

```json
    {
        "dependencies": {
            "com.preliy.material-editor": "https://github.com/Preliy/material-editor.git"
        }
    }
```

Notice: Unity Package Manager records the current commit to a lock entry of the *manifest.json*. To update to the latest version, change the hash value manually or remove the lock entry to resolve the package.

```json
    "lock": {
      "com.preliy.material-editor": {
        "revision": "master",
        "hash": "..."
      }
    }
```

## How to use
See [usage](./Documentation~/MaterialPalette.md).

