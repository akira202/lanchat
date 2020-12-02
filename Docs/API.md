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

### Lanchat 2 protocol

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
            List of nodes alredy connected with sending node.
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
</table>