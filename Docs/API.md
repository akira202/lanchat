# Lanchat 2.1 API

Lanchat uses JSON formatting and UTF-8 encoding to transfer data.

## Connecting

Each node should start a TCP server and handle multiple TCP clients.

After connecting to the TCP server or detecting an incoming connection on the server side:
1. Send handshake
2. Wait for incoming *Handshake* and send AES key info.
3. After received *KeyInfo* node is ready.
3. Send *NodesList*.

## Disconnecting

Before disconnection caused by block or disconnect command node should send *Goodbye* message.
This prevents automatic reconnection.

## Encryption

Messages are encrypted with **256-bit AES** key. 
During exchange AES key info is converted to **base64** encrypted with **2048-bit RSA**. 
RSA public key comes in handshake.

## Data exchange

All packets contains jsons with two keys in root (*Type*, *Data*) and multiple keys in *Data* key.

<table>
    <tr>
        <th>
            Type
        </th>
        <th>
            Keys
        </th>
        <th>
            Description
        </th>
        <th>
            Example
        </th>
    </tr>
    <tr>
        <td>
            Message
        </td>
        <td>
            <ul>
                <li>Data</li>
            </ul>
        </td>
        <td>
            Encrypted message sent to all nodes.
        </td>
        <td>
            <pre lang="json">
{
    "Type":"Message",
    "Data":"559iuSOtpMZZLrmmMTXp9w=="
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            Handshake
        </td>
        <td>
            <ul>
                <li>
                    Data
                    <ul>
                        <li>Nickname</li>
                        <li>
                            PublicKey
                            <ul>
                                <li>RsaModulus</li>
                                <li>RsaExpotent</li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
        </td>
        <td>
            Node nickname and public key.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"Handshake",
   "Data":{
      "Nickname":"test",
      "PublicKey":{
         "RsaModulus":"very long string",
         "RsaExponent":"AQAB"
      }
   }
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            NodesList
        </td>
        <td>
            <ul>
                <li>
                    Data
                    <ul>
                        <li>Array of ip addresses</li>
                    </ul>
                </li>
            </ul>
        </td>
        <td>
            List of nodes already connected with sending node.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"NodesList",
   "Data":[
      "192.168.18.1",
      "192.168.18.1"
   ]
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            KeyInfo
        </td>
        <td>
            <ul>
                <li>Data
                    <ul>
                        <li>AesKey</li>
                        <li>AesIv</li>
                    </ul>
                </li>
            </ul>
        </td>
        <td>
            Symetric encryption key. Encrypted with public key.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"KeyInfo",
   "Data":{
      "AesKey":"very long string",
      "AesIv":"very long string"
   }
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            NicknameUpdate
        </td>
        <td>
            <ul>
                <li>Data</li>
            </ul>
        </td>
        <td>
            New node nickname.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"NicknameUpdate",
   "Data":"test"
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            Goodbye
        </td>
        <td>
            null
        </td>
        <td>
            Sent after disconnect command. Blocking automatic reconnection in node.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"Goodbye",
   "Data":null
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            PrivateMessage
        </td>
        <td>
            <ul>
                <li>Data</li>
            </ul>
        </td>
        <td>
            Encrypted message sent to single node.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"PrivateMessage",
   "Data":"scRzgudmk4I30rU9h\u002BNFyQ=="
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            Status
        </td>
        <td>
            <ul>
                <li>
                    Data (enum)
                    <ul>
                        <li>Online</li>
                        <li>AwayFromKeyboard</li>
                        <li>DoNotDisturb</li>
                    </ul>
                </li>
            </ul>
        </td>
        <td>
            New node user status.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"Status",
   "Data":"Online"
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            Ping
        </td>
        <td>
            null
        </td>
        <td>
            Ping.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"Goodbye",
   "Data":null
}
            </pre>
        </td>
    </tr>
    <tr>
        <td>
            Pong
        </td>
        <td>
            null
        </td>
        <td>
            Reply to ping.
        </td>
        <td>
            <pre lang="json">
{
   "Type":"Goodbye",
   "Data":null
}
            </pre>
        </td>
    </tr>
</table>

## UDP broadcasting
Lanchat nodes can use UDP broadcast to discover each other.

<table>
    <tr>
        <th>
            Type
        </th>
        <th>
            Keys
        </th>
        <th>
            Description
        </th>
        <th>
            Example
        </th>
    </tr>
    <tr>
        <td>
            Broadcast
        </td>
        <td>
            <ul>
                <li>Guid</li>
                <li>Nickname</li>
            </ul>
        </td>
        <td>
            Broadcast message.
        </td>
        <td>
            <pre lang="json">
{
  "Guid":"00000000-0000-0000-0000-000000000000",
  "Nickname":"username"
}
            </pre>
        </td>
    </tr>
</table>