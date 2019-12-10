Websocket Echo server
=========================== 

Very simple .Net Framework (4.7.2) console application that launches a websocket server at a configurable address that will then output to the console any client message as well when they connect as well as disconnect.

### Configuration

To configure the address the websocket server uses, change it in the projects App.config

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="WSUrl" value="ws://127.0.0.1:4486" />
```


