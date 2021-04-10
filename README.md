# Lanchat 2

Encrypted, P2P, local network chat. 

[Documentation](https://github.com/tofudd/lanchat/wiki)

## Get Lanchat

### Windows & MacOS
* [Self-contained binary](https://github.com/tofudd/lanchat/releases)

### Linux
* Flathub: `flatpak install io.github.tofudd.lanchat.terminal`
* [Self-contained binary](https://github.com/tofudd/lanchat/releases/)

## Lanchat.Terminal
You can start terminal client with the following arguments:

| Argument    | Short | Description                                     |
|-------------|-------|-------------------------------------------------|
| --debug     | -d    | Show logs.                                      |
| --loopback  | -l    | Connect with localhost after start (for debug). |
| --no-server | -n    | Start without server.                           |
| --no-udp    | -b    | Start without broadcasting.                     |

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