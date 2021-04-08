<a name='assembly'></a>
# Lanchat.Core

## Contents

- [Announce](#T-Lanchat-Core-Models-Announce 'Lanchat.Core.Models.Announce')
  - [Active](#P-Lanchat-Core-Models-Announce-Active 'Lanchat.Core.Models.Announce.Active')
  - [Guid](#P-Lanchat-Core-Models-Announce-Guid 'Lanchat.Core.Models.Announce.Guid')
  - [IpAddress](#P-Lanchat-Core-Models-Announce-IpAddress 'Lanchat.Core.Models.Announce.IpAddress')
  - [Nickname](#P-Lanchat-Core-Models-Announce-Nickname 'Lanchat.Core.Models.Announce.Nickname')
- [ApiHandler\`1](#T-Lanchat-Core-API-ApiHandler`1 'Lanchat.Core.API.ApiHandler`1')
  - [HandledType](#P-Lanchat-Core-API-ApiHandler`1-HandledType 'Lanchat.Core.API.ApiHandler`1.HandledType')
  - [Privileged](#P-Lanchat-Core-API-ApiHandler`1-Privileged 'Lanchat.Core.API.ApiHandler`1.Privileged')
  - [Handle()](#M-Lanchat-Core-API-ApiHandler`1-Handle-System-Object- 'Lanchat.Core.API.ApiHandler`1.Handle(System.Object)')
  - [Handle(data)](#M-Lanchat-Core-API-ApiHandler`1-Handle-`0- 'Lanchat.Core.API.ApiHandler`1.Handle(`0)')
- [Broadcasting](#T-Lanchat-Core-Broadcasting 'Lanchat.Core.Broadcasting')
  - [SendData(data)](#M-Lanchat-Core-Broadcasting-SendData-System-Object- 'Lanchat.Core.Broadcasting.SendData(System.Object)')
  - [SendMessage(message)](#M-Lanchat-Core-Broadcasting-SendMessage-System-String- 'Lanchat.Core.Broadcasting.SendMessage(System.String)')
- [EnumerableExtensions](#T-Lanchat-Core-Extensions-EnumerableExtensions 'Lanchat.Core.Extensions.EnumerableExtensions')
  - [ForEach\`\`1()](#M-Lanchat-Core-Extensions-EnumerableExtensions-ForEach``1-System-Collections-Generic-IEnumerable{``0},System-Action{``0}- 'Lanchat.Core.Extensions.EnumerableExtensions.ForEach``1(System.Collections.Generic.IEnumerable{``0},System.Action{``0})')
- [FileReceiver](#T-Lanchat-Core-FileTransfer-FileReceiver 'Lanchat.Core.FileTransfer.FileReceiver')
  - [Request](#P-Lanchat-Core-FileTransfer-FileReceiver-Request 'Lanchat.Core.FileTransfer.FileReceiver.Request')
  - [AcceptRequest()](#M-Lanchat-Core-FileTransfer-FileReceiver-AcceptRequest 'Lanchat.Core.FileTransfer.FileReceiver.AcceptRequest')
  - [CancelReceive()](#M-Lanchat-Core-FileTransfer-FileReceiver-CancelReceive 'Lanchat.Core.FileTransfer.FileReceiver.CancelReceive')
  - [RejectRequest()](#M-Lanchat-Core-FileTransfer-FileReceiver-RejectRequest 'Lanchat.Core.FileTransfer.FileReceiver.RejectRequest')
- [FileSender](#T-Lanchat-Core-FileTransfer-FileSender 'Lanchat.Core.FileTransfer.FileSender')
  - [Request](#P-Lanchat-Core-FileTransfer-FileSender-Request 'Lanchat.Core.FileTransfer.FileSender.Request')
  - [CreateSendRequest(path)](#M-Lanchat-Core-FileTransfer-FileSender-CreateSendRequest-System-String- 'Lanchat.Core.FileTransfer.FileSender.CreateSendRequest(System.String)')
- [FileTransferException](#T-Lanchat-Core-FileTransfer-FileTransferException 'Lanchat.Core.FileTransfer.FileTransferException')
  - [Request](#P-Lanchat-Core-FileTransfer-FileTransferException-Request 'Lanchat.Core.FileTransfer.FileTransferException.Request')
- [FileTransferRequest](#T-Lanchat-Core-FileTransfer-FileTransferRequest 'Lanchat.Core.FileTransfer.FileTransferRequest')
  - [FileName](#P-Lanchat-Core-FileTransfer-FileTransferRequest-FileName 'Lanchat.Core.FileTransfer.FileTransferRequest.FileName')
  - [FilePath](#P-Lanchat-Core-FileTransfer-FileTransferRequest-FilePath 'Lanchat.Core.FileTransfer.FileTransferRequest.FilePath')
  - [Parts](#P-Lanchat-Core-FileTransfer-FileTransferRequest-Parts 'Lanchat.Core.FileTransfer.FileTransferRequest.Parts')
  - [PartsTransferred](#P-Lanchat-Core-FileTransfer-FileTransferRequest-PartsTransferred 'Lanchat.Core.FileTransfer.FileTransferRequest.PartsTransferred')
  - [Progress](#P-Lanchat-Core-FileTransfer-FileTransferRequest-Progress 'Lanchat.Core.FileTransfer.FileTransferRequest.Progress')
- [IApiHandler](#T-Lanchat-Core-API-IApiHandler 'Lanchat.Core.API.IApiHandler')
  - [HandledType](#P-Lanchat-Core-API-IApiHandler-HandledType 'Lanchat.Core.API.IApiHandler.HandledType')
  - [Privileged](#P-Lanchat-Core-API-IApiHandler-Privileged 'Lanchat.Core.API.IApiHandler.Privileged')
  - [Handle(data)](#M-Lanchat-Core-API-IApiHandler-Handle-System-Object- 'Lanchat.Core.API.IApiHandler.Handle(System.Object)')
- [IConfig](#T-Lanchat-Core-IConfig 'Lanchat.Core.IConfig')
  - [BlockedAddresses](#P-Lanchat-Core-IConfig-BlockedAddresses 'Lanchat.Core.IConfig.BlockedAddresses')
  - [BroadcastPort](#P-Lanchat-Core-IConfig-BroadcastPort 'Lanchat.Core.IConfig.BroadcastPort')
  - [ConnectToReceivedList](#P-Lanchat-Core-IConfig-ConnectToReceivedList 'Lanchat.Core.IConfig.ConnectToReceivedList')
  - [ConnectToSaved](#P-Lanchat-Core-IConfig-ConnectToSaved 'Lanchat.Core.IConfig.ConnectToSaved')
  - [Nickname](#P-Lanchat-Core-IConfig-Nickname 'Lanchat.Core.IConfig.Nickname')
  - [NodesDetection](#P-Lanchat-Core-IConfig-NodesDetection 'Lanchat.Core.IConfig.NodesDetection')
  - [ReceivedFilesDirectory](#P-Lanchat-Core-IConfig-ReceivedFilesDirectory 'Lanchat.Core.IConfig.ReceivedFilesDirectory')
  - [SavedAddresses](#P-Lanchat-Core-IConfig-SavedAddresses 'Lanchat.Core.IConfig.SavedAddresses')
  - [ServerPort](#P-Lanchat-Core-IConfig-ServerPort 'Lanchat.Core.IConfig.ServerPort')
  - [StartServer](#P-Lanchat-Core-IConfig-StartServer 'Lanchat.Core.IConfig.StartServer')
  - [Status](#P-Lanchat-Core-IConfig-Status 'Lanchat.Core.IConfig.Status')
  - [UseIPv6](#P-Lanchat-Core-IConfig-UseIPv6 'Lanchat.Core.IConfig.UseIPv6')
- [INetworkElement](#T-Lanchat-Core-Network-INetworkElement 'Lanchat.Core.Network.INetworkElement')
  - [Endpoint](#P-Lanchat-Core-Network-INetworkElement-Endpoint 'Lanchat.Core.Network.INetworkElement.Endpoint')
  - [Id](#P-Lanchat-Core-Network-INetworkElement-Id 'Lanchat.Core.Network.INetworkElement.Id')
  - [Close()](#M-Lanchat-Core-Network-INetworkElement-Close 'Lanchat.Core.Network.INetworkElement.Close')
  - [Send(text)](#M-Lanchat-Core-Network-INetworkElement-Send-System-String- 'Lanchat.Core.Network.INetworkElement.Send(System.String)')
- [Messaging](#T-Lanchat-Core-Chat-Messaging 'Lanchat.Core.Chat.Messaging')
  - [SendMessage(content)](#M-Lanchat-Core-Chat-Messaging-SendMessage-System-String- 'Lanchat.Core.Chat.Messaging.SendMessage(System.String)')
  - [SendPrivateMessage(content)](#M-Lanchat-Core-Chat-Messaging-SendPrivateMessage-System-String- 'Lanchat.Core.Chat.Messaging.SendPrivateMessage(System.String)')
- [NetworkOutput](#T-Lanchat-Core-API-NetworkOutput 'Lanchat.Core.API.NetworkOutput')
  - [SendData(content)](#M-Lanchat-Core-API-NetworkOutput-SendData-System-Object- 'Lanchat.Core.API.NetworkOutput.SendData(System.Object)')
  - [SendPrivilegedData(content)](#M-Lanchat-Core-API-NetworkOutput-SendPrivilegedData-System-Object- 'Lanchat.Core.API.NetworkOutput.SendPrivilegedData(System.Object)')
- [Node](#T-Lanchat-Core-Node 'Lanchat.Core.Node')
  - [FileReceiver](#F-Lanchat-Core-Node-FileReceiver 'Lanchat.Core.Node.FileReceiver')
  - [FileSender](#F-Lanchat-Core-Node-FileSender 'Lanchat.Core.Node.FileSender')
  - [Messaging](#F-Lanchat-Core-Node-Messaging 'Lanchat.Core.Node.Messaging')
  - [NetworkElement](#F-Lanchat-Core-Node-NetworkElement 'Lanchat.Core.Node.NetworkElement')
  - [NetworkOutput](#F-Lanchat-Core-Node-NetworkOutput 'Lanchat.Core.Node.NetworkOutput')
  - [Resolver](#F-Lanchat-Core-Node-Resolver 'Lanchat.Core.Node.Resolver')
  - [Id](#P-Lanchat-Core-Node-Id 'Lanchat.Core.Node.Id')
  - [Nickname](#P-Lanchat-Core-Node-Nickname 'Lanchat.Core.Node.Nickname')
  - [PreviousNickname](#P-Lanchat-Core-Node-PreviousNickname 'Lanchat.Core.Node.PreviousNickname')
  - [Ready](#P-Lanchat-Core-Node-Ready 'Lanchat.Core.Node.Ready')
  - [ShortId](#P-Lanchat-Core-Node-ShortId 'Lanchat.Core.Node.ShortId')
  - [Status](#P-Lanchat-Core-Node-Status 'Lanchat.Core.Node.Status')
  - [Disconnect()](#M-Lanchat-Core-Node-Disconnect 'Lanchat.Core.Node.Disconnect')
  - [Dispose()](#M-Lanchat-Core-Node-Dispose 'Lanchat.Core.Node.Dispose')
- [NodesDetector](#T-Lanchat-Core-NodesDetection-NodesDetector 'Lanchat.Core.NodesDetection.NodesDetector')
  - [DetectedNodes](#P-Lanchat-Core-NodesDetection-NodesDetector-DetectedNodes 'Lanchat.Core.NodesDetection.NodesDetector.DetectedNodes')
- [P2P](#T-Lanchat-Core-P2P 'Lanchat.Core.P2P')
  - [#ctor()](#M-Lanchat-Core-P2P-#ctor-Lanchat-Core-IConfig- 'Lanchat.Core.P2P.#ctor(Lanchat.Core.IConfig)')
  - [Broadcasting](#P-Lanchat-Core-P2P-Broadcasting 'Lanchat.Core.P2P.Broadcasting')
  - [Nodes](#P-Lanchat-Core-P2P-Nodes 'Lanchat.Core.P2P.Nodes')
  - [NodesDetection](#P-Lanchat-Core-P2P-NodesDetection 'Lanchat.Core.P2P.NodesDetection')
  - [Connect(ipAddress,port)](#M-Lanchat-Core-P2P-Connect-System-Net-IPAddress,System-Nullable{System-Int32}- 'Lanchat.Core.P2P.Connect(System.Net.IPAddress,System.Nullable{System.Int32})')
  - [Start()](#M-Lanchat-Core-P2P-Start 'Lanchat.Core.P2P.Start')
- [Resolver](#T-Lanchat-Core-API-Resolver 'Lanchat.Core.API.Resolver')
  - [RegisterHandler(apiHandler)](#M-Lanchat-Core-API-Resolver-RegisterHandler-Lanchat-Core-API-IApiHandler- 'Lanchat.Core.API.Resolver.RegisterHandler(Lanchat.Core.API.IApiHandler)')
- [Server](#T-Lanchat-Core-Network-Server 'Lanchat.Core.Network.Server')
  - [#ctor(address,port,config)](#M-Lanchat-Core-Network-Server-#ctor-System-Net-IPAddress,System-Int32,Lanchat-Core-IConfig- 'Lanchat.Core.Network.Server.#ctor(System.Net.IPAddress,System.Int32,Lanchat.Core.IConfig)')
  - [IncomingConnections](#P-Lanchat-Core-Network-Server-IncomingConnections 'Lanchat.Core.Network.Server.IncomingConnections')
- [Status](#T-Lanchat-Core-Models-Status 'Lanchat.Core.Models.Status')
  - [AwayFromKeyboard](#F-Lanchat-Core-Models-Status-AwayFromKeyboard 'Lanchat.Core.Models.Status.AwayFromKeyboard')
  - [DoNotDisturb](#F-Lanchat-Core-Models-Status-DoNotDisturb 'Lanchat.Core.Models.Status.DoNotDisturb')
  - [Online](#F-Lanchat-Core-Models-Status-Online 'Lanchat.Core.Models.Status.Online')

<a name='T-Lanchat-Core-Models-Announce'></a>
## Announce `type`

##### Namespace

Lanchat.Core.Models

##### Summary



<a name='P-Lanchat-Core-Models-Announce-Active'></a>
### Active `property`

##### Summary

Node actively sends broadcasts.

<a name='P-Lanchat-Core-Models-Announce-Guid'></a>
### Guid `property`

##### Summary

Guid for ignoring own broadcasts.

<a name='P-Lanchat-Core-Models-Announce-IpAddress'></a>
### IpAddress `property`

##### Summary

Node user nickname.

<a name='P-Lanchat-Core-Models-Announce-Nickname'></a>
### Nickname `property`

##### Summary

Node nickname.

<a name='T-Lanchat-Core-API-ApiHandler`1'></a>
## ApiHandler\`1 `type`

##### Namespace

Lanchat.Core.API

##### Summary

Inherit this class for create custom API handler.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| T | Type of handled model |

<a name='P-Lanchat-Core-API-ApiHandler`1-HandledType'></a>
### HandledType `property`

##### Summary

*Inherit from parent.*

<a name='P-Lanchat-Core-API-ApiHandler`1-Privileged'></a>
### Privileged `property`

##### Summary

*Inherit from parent.*

<a name='M-Lanchat-Core-API-ApiHandler`1-Handle-System-Object-'></a>
### Handle() `method`

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Lanchat-Core-API-ApiHandler`1-Handle-`0-'></a>
### Handle(data) `method`

##### Summary

Handle object converted to required type.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [\`0](#T-`0 '`0') | Converted object. |

<a name='T-Lanchat-Core-Broadcasting'></a>
## Broadcasting `type`

##### Namespace

Lanchat.Core

##### Summary

Send data to all nodes.

<a name='M-Lanchat-Core-Broadcasting-SendData-System-Object-'></a>
### SendData(data) `method`

##### Summary

Broadcast data.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') |  |

<a name='M-Lanchat-Core-Broadcasting-SendMessage-System-String-'></a>
### SendMessage(message) `method`

##### Summary

Broadcast message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| message | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Message content. |

<a name='T-Lanchat-Core-Extensions-EnumerableExtensions'></a>
## EnumerableExtensions `type`

##### Namespace

Lanchat.Core.Extensions

##### Summary

Extension methods for Enumerable

<a name='M-Lanchat-Core-Extensions-EnumerableExtensions-ForEach``1-System-Collections-Generic-IEnumerable{``0},System-Action{``0}-'></a>
### ForEach\`\`1() `method`

##### Summary

ForEach like in Lists

##### Parameters

This method has no parameters.

<a name='T-Lanchat-Core-FileTransfer-FileReceiver'></a>
## FileReceiver `type`

##### Namespace

Lanchat.Core.FileTransfer

##### Summary

File receiving.

<a name='P-Lanchat-Core-FileTransfer-FileReceiver-Request'></a>
### Request `property`

##### Summary

Incoming file request.

<a name='M-Lanchat-Core-FileTransfer-FileReceiver-AcceptRequest'></a>
### AcceptRequest() `method`

##### Summary

Accept incoming file request.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | No awaiting request |

<a name='M-Lanchat-Core-FileTransfer-FileReceiver-CancelReceive'></a>
### CancelReceive() `method`

##### Summary

Cancel current receive request.

##### Parameters

This method has no parameters.

<a name='M-Lanchat-Core-FileTransfer-FileReceiver-RejectRequest'></a>
### RejectRequest() `method`

##### Summary

Reject incoming file request.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | No awaiting request |

<a name='T-Lanchat-Core-FileTransfer-FileSender'></a>
## FileSender `type`

##### Namespace

Lanchat.Core.FileTransfer

##### Summary

File sending.

<a name='P-Lanchat-Core-FileTransfer-FileSender-Request'></a>
### Request `property`

##### Summary

Outgoing file request.

<a name='M-Lanchat-Core-FileTransfer-FileSender-CreateSendRequest-System-String-'></a>
### CreateSendRequest(path) `method`

##### Summary

Send file exchange request.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | File path |

<a name='T-Lanchat-Core-FileTransfer-FileTransferException'></a>
## FileTransferException `type`

##### Namespace

Lanchat.Core.FileTransfer

##### Summary

Throw when file exchange stopped before finish.

<a name='P-Lanchat-Core-FileTransfer-FileTransferException-Request'></a>
### Request `property`

##### Summary

Request that throws error.

<a name='T-Lanchat-Core-FileTransfer-FileTransferRequest'></a>
## FileTransferRequest `type`

##### Namespace

Lanchat.Core.FileTransfer

##### Summary

Class representing single transfer request.

<a name='P-Lanchat-Core-FileTransfer-FileTransferRequest-FileName'></a>
### FileName `property`

##### Summary

File name.

<a name='P-Lanchat-Core-FileTransfer-FileTransferRequest-FilePath'></a>
### FilePath `property`

##### Summary

Path when file will be saved or when is sending from.

<a name='P-Lanchat-Core-FileTransfer-FileTransferRequest-Parts'></a>
### Parts `property`

##### Summary

Size of file in parts.

<a name='P-Lanchat-Core-FileTransfer-FileTransferRequest-PartsTransferred'></a>
### PartsTransferred `property`

##### Summary

Already transferred parts counter.

<a name='P-Lanchat-Core-FileTransfer-FileTransferRequest-Progress'></a>
### Progress `property`

##### Summary

Transfer progress in percent.

<a name='T-Lanchat-Core-API-IApiHandler'></a>
## IApiHandler `type`

##### Namespace

Lanchat.Core.API

##### Summary

Use [ApiHandler\`1](#T-Lanchat-Core-API-ApiHandler`1 'Lanchat.Core.API.ApiHandler`1') instead this.

<a name='P-Lanchat-Core-API-IApiHandler-HandledType'></a>
### HandledType `property`

##### Summary

Type of handled model.

<a name='P-Lanchat-Core-API-IApiHandler-Privileged'></a>
### Privileged `property`

##### Summary

If handler is privileged it will process data even if node is unready.

<a name='M-Lanchat-Core-API-IApiHandler-Handle-System-Object-'></a>
### Handle(data) `method`

##### Summary

Object handler.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| data | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | API model object. |

<a name='T-Lanchat-Core-IConfig'></a>
## IConfig `type`

##### Namespace

Lanchat.Core

##### Summary

Lanchat.Core configuration.

<a name='P-Lanchat-Core-IConfig-BlockedAddresses'></a>
### BlockedAddresses `property`

##### Summary

Blocked IP addresses.

<a name='P-Lanchat-Core-IConfig-BroadcastPort'></a>
### BroadcastPort `property`

##### Summary

Broadcast port.

<a name='P-Lanchat-Core-IConfig-ConnectToReceivedList'></a>
### ConnectToReceivedList `property`

##### Summary

Try connecting with nodes from received list.

<a name='P-Lanchat-Core-IConfig-ConnectToSaved'></a>
### ConnectToSaved `property`

##### Summary

Try connecting with nodes from SavedAddresses

<a name='P-Lanchat-Core-IConfig-Nickname'></a>
### Nickname `property`

##### Summary

User nickname.

<a name='P-Lanchat-Core-IConfig-NodesDetection'></a>
### NodesDetection `property`

##### Summary

Use UDP broadcasting to announce self presence and detect other nodes.

<a name='P-Lanchat-Core-IConfig-ReceivedFilesDirectory'></a>
### ReceivedFilesDirectory `property`

##### Summary

Files download directory.

<a name='P-Lanchat-Core-IConfig-SavedAddresses'></a>
### SavedAddresses `property`

##### Summary

Addresses of previously connected nodes.

<a name='P-Lanchat-Core-IConfig-ServerPort'></a>
### ServerPort `property`

##### Summary

Server port.

<a name='P-Lanchat-Core-IConfig-StartServer'></a>
### StartServer `property`

##### Summary

Disable only in debug mode.

<a name='P-Lanchat-Core-IConfig-Status'></a>
### Status `property`

##### Summary

User status.

<a name='P-Lanchat-Core-IConfig-UseIPv6'></a>
### UseIPv6 `property`

##### Summary

Use IPv6 instead IPv4.

<a name='T-Lanchat-Core-Network-INetworkElement'></a>
## INetworkElement `type`

##### Namespace

Lanchat.Core.Network

##### Summary

Common TCP client and session stuff.

<a name='P-Lanchat-Core-Network-INetworkElement-Endpoint'></a>
### Endpoint `property`

##### Summary

IP endpoint (address + port).

<a name='P-Lanchat-Core-Network-INetworkElement-Id'></a>
### Id `property`

##### Summary

Session or client ID.

<a name='M-Lanchat-Core-Network-INetworkElement-Close'></a>
### Close() `method`

##### Summary

Close client or session.

##### Parameters

This method has no parameters.

<a name='M-Lanchat-Core-Network-INetworkElement-Send-System-String-'></a>
### Send(text) `method`

##### Summary

Send data.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Content. |

<a name='T-Lanchat-Core-Chat-Messaging'></a>
## Messaging `type`

##### Namespace

Lanchat.Core.Chat

##### Summary

Basic chat features.

<a name='M-Lanchat-Core-Chat-Messaging-SendMessage-System-String-'></a>
### SendMessage(content) `method`

##### Summary

Send message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| content | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Message content. |

<a name='M-Lanchat-Core-Chat-Messaging-SendPrivateMessage-System-String-'></a>
### SendPrivateMessage(content) `method`

##### Summary

Send private message.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| content | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Message content. |

<a name='T-Lanchat-Core-API-NetworkOutput'></a>
## NetworkOutput `type`

##### Namespace

Lanchat.Core.API

##### Summary

Send data other of type not belonging to standard Lanchat.Core set.

<a name='M-Lanchat-Core-API-NetworkOutput-SendData-System-Object-'></a>
### SendData(content) `method`

##### Summary

Send data.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| content | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | Object to send. |

<a name='M-Lanchat-Core-API-NetworkOutput-SendPrivilegedData-System-Object-'></a>
### SendPrivilegedData(content) `method`

##### Summary

Send the data before marking the node as ready (Handshake, KeyInfo...).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| content | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | Object to send. |

<a name='T-Lanchat-Core-Node'></a>
## Node `type`

##### Namespace

Lanchat.Core

##### Summary

Connected user.

<a name='F-Lanchat-Core-Node-FileReceiver'></a>
### FileReceiver `constants`

<a name='F-Lanchat-Core-Node-FileSender'></a>
### FileSender `constants`

<a name='F-Lanchat-Core-Node-Messaging'></a>
### Messaging `constants`

<a name='F-Lanchat-Core-Node-NetworkElement'></a>
### NetworkElement `constants`

<a name='F-Lanchat-Core-Node-NetworkOutput'></a>
### NetworkOutput `constants`

<a name='F-Lanchat-Core-Node-Resolver'></a>
### Resolver `constants`

<a name='P-Lanchat-Core-Node-Id'></a>
### Id `property`

##### Summary

ID of TCP client or session.

<a name='P-Lanchat-Core-Node-Nickname'></a>
### Nickname `property`

##### Summary

Node user nickname.

<a name='P-Lanchat-Core-Node-PreviousNickname'></a>
### PreviousNickname `property`

##### Summary

Nickname before last change.

<a name='P-Lanchat-Core-Node-Ready'></a>
### Ready `property`

##### Summary

Node ready. If set to false node won't send or receive messages.

<a name='P-Lanchat-Core-Node-ShortId'></a>
### ShortId `property`

##### Summary

Short ID.

<a name='P-Lanchat-Core-Node-Status'></a>
### Status `property`

##### Summary

Node user status.

<a name='M-Lanchat-Core-Node-Disconnect'></a>
### Disconnect() `method`

##### Summary

Disconnect from node.

##### Parameters

This method has no parameters.

<a name='M-Lanchat-Core-Node-Dispose'></a>
### Dispose() `method`

##### Summary

Dispose node. For safe disconnect use [Disconnect](#M-Lanchat-Core-Node-Disconnect 'Lanchat.Core.Node.Disconnect') instead.

##### Parameters

This method has no parameters.

<a name='T-Lanchat-Core-NodesDetection-NodesDetector'></a>
## NodesDetector `type`

##### Namespace

Lanchat.Core.NodesDetection

##### Summary

Detecting nodes by UDP broadcasts.

<a name='P-Lanchat-Core-NodesDetection-NodesDetector-DetectedNodes'></a>
### DetectedNodes `property`

##### Summary

Detected nodes.

<a name='T-Lanchat-Core-P2P'></a>
## P2P `type`

##### Namespace

Lanchat.Core

##### Summary

Main class representing network in P2P mode.

<a name='M-Lanchat-Core-P2P-#ctor-Lanchat-Core-IConfig-'></a>
### #ctor() `constructor`

##### Summary

Initialize P2P mode.

##### Parameters

This constructor has no parameters.

<a name='P-Lanchat-Core-P2P-Broadcasting'></a>
### Broadcasting `property`

##### Summary

Send data to all nodes.

<a name='P-Lanchat-Core-P2P-Nodes'></a>
### Nodes `property`

##### Summary

List of connected nodes.

<a name='P-Lanchat-Core-P2P-NodesDetection'></a>
### NodesDetection `property`

<a name='M-Lanchat-Core-P2P-Connect-System-Net-IPAddress,System-Nullable{System-Int32}-'></a>
### Connect(ipAddress,port) `method`

##### Summary

Connect to node.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ipAddress | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Node IP address. |
| port | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | Node port. |

<a name='M-Lanchat-Core-P2P-Start'></a>
### Start() `method`

##### Summary

Start server.

##### Parameters

This method has no parameters.

<a name='T-Lanchat-Core-API-Resolver'></a>
## Resolver `type`

##### Namespace

Lanchat.Core.API

##### Summary

Class used to handle received data.

<a name='M-Lanchat-Core-API-Resolver-RegisterHandler-Lanchat-Core-API-IApiHandler-'></a>
### RegisterHandler(apiHandler) `method`

##### Summary

Add data handler for specific model type.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| apiHandler | [Lanchat.Core.API.IApiHandler](#T-Lanchat-Core-API-IApiHandler 'Lanchat.Core.API.IApiHandler') | ApiHandler object. |

<a name='T-Lanchat-Core-Network-Server'></a>
## Server `type`

##### Namespace

Lanchat.Core.Network

<a name='M-Lanchat-Core-Network-Server-#ctor-System-Net-IPAddress,System-Int32,Lanchat-Core-IConfig-'></a>
### #ctor(address,port,config) `constructor`

##### Summary

Create server.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| address | [System.Net.IPAddress](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Net.IPAddress 'System.Net.IPAddress') | Listening IP |
| port | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Listening port |
| config | [Lanchat.Core.IConfig](#T-Lanchat-Core-IConfig 'Lanchat.Core.IConfig') | Lanchat config. |

<a name='P-Lanchat-Core-Network-Server-IncomingConnections'></a>
### IncomingConnections `property`

##### Summary

List of incoming connections.

<a name='T-Lanchat-Core-Models-Status'></a>
## Status `type`

##### Namespace

Lanchat.Core.Models

##### Summary

Node user status.

<a name='F-Lanchat-Core-Models-Status-AwayFromKeyboard'></a>
### AwayFromKeyboard `constants`

##### Summary

Away from keyboard.

<a name='F-Lanchat-Core-Models-Status-DoNotDisturb'></a>
### DoNotDisturb `constants`

##### Summary

Do not disturb.

<a name='F-Lanchat-Core-Models-Status-Online'></a>
### Online `constants`

##### Summary

Online.
