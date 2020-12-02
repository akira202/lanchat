# Lanchat 2

Encrypted, P2P, local network chat. 

[API documentation](https://github.com/tofudd/lanchat/blob/master/Docs/API.md)

## Projects
* Lancaht.Core (2.1.0) - library
* Lanchat.Terminal (2.1.0) - console app for Windows and Linux

Check [release](https://github.com/tofudd/lanchat/releases) section for binaries.

**Lanchat.Terminal requires .NET 5 runtime**

## Lanchat.Terminal
You can start terminal client with the following arguments:

| Argument    | Short | Description                                     |
|-------------|-------|-------------------------------------------------|
| --debug     | -d    | Show logs.                                      |
| --loopback  | -l    | Connect with localhost after start (for debug). |
| --server    | -s    | Start only server without UI.                   |
| --no-server | -n    | Start without server.                           |
| --relay     | -r    | Start in relay mode.                            |

## How it works
In P2P mode nodes starts own server and multiple clients. Each node is connected with other by single TCP connection.
It looks something like that.

1. Node A client connects to Node B server.
2. Nodes exchange RSA public keys.
3. Nodes exchange AES keys encrypted with RSA.
4. Nodes exchange previously connected nodes list.
5. Both nodes trying to connect with nodes from list (if they didn't connected already).
6. All done.

TODO: Put some fancy scheme here.