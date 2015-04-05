# Etec

Etec is a utility to manage formula calculation tools, fully compatible with the .NET assemblies.

## Usage

Etec works out of the box, just place a module dll in the same directory as the Etec executable, and Etec will load it right on startup.


## Modules development

The development of Etec modules is simple. It is only necessary to have a class that implements the [Base/Base.fs](IEtecModule) interface in a module's dll, and Etec will recognize and load it.
There are also utilities such as formulas and common GUI components in Etec's base API that can be used by any module.

## License

Copyright (C) 2015 gahag

All rights reserved.

This software may be modified and distributed under the terms
of the BSD license. See the [LICENSE](LICENSE) file for details.
